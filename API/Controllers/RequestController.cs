using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using API.DTO;
using API.Services;
using Domain.Enums;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;
#nullable enable

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : BaseApiController
    {
        private readonly DiaryService _diaryService;
        public RequestController(DataContext context, DiaryService diaryService) : base(context)
        {
            _diaryService = diaryService;
        }

        [HttpPost("request")]
        public async Task<ActionResult> SendRequest(SendRequestDTO req)
        {
            if (!Enum.IsDefined(typeof(RequestsEnum), req.requestType))
            {
                return BadRequest("wrong request type");
            }
            var request = (RequestsEnum)req.requestType;
            var userId = getCurrentUserId();
            var user = await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Id == userId);

            User? target = null;
            if (req.userName is not null)
            {
                target = await _context.Users.Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.UserName == (string)req.userName);
                if (target is null)
                {
                    return BadRequest("wrong target user");
                }
            }
            var check = await checkIfRequestHasMeaning(user!, request, target);
            if (!check)
            {
                return BadRequest("impossible request");
            }
            await _context.UsersRequests.AddAsync(new UserRequest { UserSourceId = userId, Request = request, UserTargetId = target?.Id });
            return await saveContext();
        }

        [HttpGet("doctors")]
        public async Task<ActionResult<List<RequestDTO>>> RequestDoctors()
        {
            var userId = getCurrentUserId();
            Expression<Func<UserRequest, bool>> whereClause = req =>
                req.UserTargetId == userId && req.Request == RequestsEnum.ViewAsDoctor;
            return await getRequests(whereClause);
        }


        [Authorize(Roles = "Doctor,Administrator")]
        [HttpGet("patients")]
        public async Task<ActionResult<List<RequestDTO>>> RequestPatients()
        {
            var userId = getCurrentUserId();
            Expression<Func<UserRequest, bool>> whereClause = (req =>
                req.UserTargetId == userId && req.Request == RequestsEnum.InviteDoctor);
            return await getRequests(whereClause);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("doctor")]
        public async Task<ActionResult<List<RequestDTO>>> RequestBecomeDoctor()
        {
            Expression<Func<UserRequest, bool>> whereClause = req =>
                req.Request == RequestsEnum.BecomeDoctor;
            return await getRequests(whereClause);
        }

        [HttpDelete("cancel")]
        public async Task<ActionResult> RequestCancel(int requestId)
        {
            int userId = getCurrentUserId();
            var request = await _context.UsersRequests.SingleOrDefaultAsync(req => req.Id == requestId && (req.UserTargetId == userId || req.UserSourceId == userId));
            if (request is null)
            {
                return BadRequest("wrong request id");
            }
            _context.Remove(request);
            return await saveContext();

        }

        [HttpGet("accept")]
        public async Task<ActionResult> RequestAccept(int requestId)
        {
            int userId = getCurrentUserId();
            var request = await _context.UsersRequests.SingleOrDefaultAsync(req => req.Id == requestId && req.UserTargetId == userId);
            if (request is null)
            {
                return BadRequest("wrong request id");
            }

            try
            {
                await changeContextAsync(request);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            _context.Remove(request);
            return await saveContext();
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<RequestDTO>>> getUserRequests()
        {
            int userId = getCurrentUserId();
            Expression<Func<UserRequest, bool>> whereClause = r => r.UserSourceId == userId;
            return await getRequests(whereClause);
        }

        private int getCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        private async Task<bool> isExists(int userId, RequestsEnum requests, int targetUserId)
        {
            return await _context.UsersRequests.AnyAsync(req => req.UserSourceId == userId && req.UserTargetId == targetUserId && req.Request == requests);
        }
        private async Task<bool> isExists(int userId, RequestsEnum requests)
        {
            return await _context.UsersRequests.AnyAsync(req => req.UserSourceId == userId && req.Request == requests);
        }
        private RequestDTO createRequestObject(UserRequest request)
        {
            return new RequestDTO
            {
                sourceUser = new UserInfoDTO
                {
                    userName = request.UserSource.UserName,
                    firstName = request.UserSource.FirstName,
                    secondName = request.UserSource.SecondName,
                    description = request.UserSource.Description
                },
                targetUser = request.UserTarget is null ? null : new UserInfoDTO
                {
                    userName = request.UserTarget.UserName,
                    firstName = request.UserTarget.FirstName,
                    secondName = request.UserTarget.SecondName,
                    description = request.UserTarget.Description
                },
                requestId = request.Id,
                requestType = request.Request
            };
        }

        private async Task<bool> checkIfRequestHasMeaning(User source, RequestsEnum request, User? target)
        {
            if (target is null && request != RequestsEnum.BecomeDoctor || (target is null && !(target?.isSearching ?? false)))
            {
                return false;
            }
            if (target!.UserName == source.UserName)
            {
                return false;
            }


            if ((request == RequestsEnum.BecomeDoctor && await isExists(source.Id, request)) ||
                await isExists(source.Id, request, target?.Id ?? 0))
            {
                return false;
            }

            bool isPossible = false;

            switch (request)
            {
                case RequestsEnum.ViewAsDoctor:
                    isPossible = (source.Role.Name != RolesEnum.Patient.GetDescription() &&
                    !await _context.UserDoctors.AnyAsync(ud => ud.DoctorId == source.Id && ud.PatientId == target!.Id));
                    break;
                case RequestsEnum.InviteDoctor:
                    isPossible = (source.Role.Name != RolesEnum.Patient.GetDescription() &&
                    !await _context.UserDoctors.AnyAsync(ud => ud.DoctorId == target!.Id && ud.PatientId == source.Id));
                    break;
                case RequestsEnum.BecomeDoctor:
                    isPossible = (source.Role.Name == RolesEnum.Patient.GetDescription());
                    break;
            }


            return isPossible;
        }

        private async Task changeContextAsync(UserRequest request)
        {
            var userSource = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.UserSourceId);
            switch (request.Request)
            {
                case RequestsEnum.BecomeDoctor:
                    userSource!.Role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == RolesEnum.Doctor.GetDescription());
                    return;
            }
            int targetId = request.UserTargetId ?? 0;
            var userDoctor = new UserDoctor { };
            switch (request.Request)
            {
                case RequestsEnum.ViewAsDoctor:
                    userDoctor.PatientId = targetId;
                    userDoctor.DoctorId = userSource!.Id;
                    break;
                case RequestsEnum.InviteDoctor:
                    userDoctor.PatientId = userSource!.Id;
                    userDoctor.DoctorId = targetId;
                    break;
            }

            await _context.AddAsync(userDoctor);
            await _context.SaveChangesAsync();

            var diaresNames = _diaryService.getDiariesNames();
            foreach (var name in diaresNames)
            {
                await _context.AddAsync(new LastUserView { DiaryName = name, UserDoctorId = userDoctor.Id });
            }
            await _context.SaveChangesAsync();
        }

        private async Task<List<RequestDTO>> getRequests(Expression<Func<UserRequest, bool>> whereClause)
        {
            var requests = await _context.UsersRequests.Where(whereClause)
                .Include(req => req.UserSource)
                .ToListAsync();
            var result = new List<RequestDTO> { };
            foreach (var req in requests)
            {
                result.Add(createRequestObject(req));
            }
            return result;
        }

    }
}