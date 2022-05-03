using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using API.DTO;
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
        //private readonly UserManager<User> _userManager;
        public RequestController(DataContext context) : base(context)
        {
        }

        [HttpGet("request/{requesTtype:int}/{user?}")]
        public async Task<ActionResult> SendRequest(int requestType, string? userName)
        {
            if (!Enum.IsDefined(typeof(RequestsEnum), requestType))
            {
                BadRequest("wrong request type");
            }
            var request = (RequestsEnum)requestType;
            var userId = getCurrentUserId();
            var user = await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Id == userId);

            User? target = null;
            if (userName is not null)
            {
                target = await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserName == (string)userName);
            }

            if (target == null && request != RequestsEnum.BecomeDoctor || (!target?.isSearching ?? false))
            {
                BadRequest("wrong request type");
            }

            bool isPossible = false;
            int? targetId = target is null ? null : target.Id;
            switch (request)
            {
                case RequestsEnum.ViewAsDoctor:
                    isPossible = (user?.Role.Name != RolesEnum.Patient.GetDescription());
                    break;
                case RequestsEnum.InviteDoctor:
                    isPossible = (target?.Role.Name != RolesEnum.Patient.GetDescription());
                    break;
                case RequestsEnum.BecomeDoctor:
                    isPossible = (user?.Role.Name == RolesEnum.Patient.GetDescription());
                    break;
            }

            if (!isPossible)
            {

                return BadRequest("No access");
            }
            if (request == RequestsEnum.BecomeDoctor && !await isExists(userId, request) ||
                !await isExists(userId, request, target?.Id ?? 0))
            {
                return Conflict();
            }

            await _context.UsersRequests.AddAsync(new UserRequest { UserSourceId = userId, Request = request, UserTargetId = targetId });
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

        [HttpGet("cancel/{request:int}")]
        public async Task<ActionResult> RequestCancel(int requestId)
        {
            int userId = getCurrentUserId();
            var request = _context.UsersRequests.SingleOrDefaultAsync(req => req.Id == requestId && req.UserTargetId == userId);
            if (request is null)
            {
                return BadRequest("wrong request id");
            }
            _context.Remove(request);
            return await saveContext();

        }

        [HttpGet("accept/{request:int}")]
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
                changeContext(request);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            _context.Remove(request);
            return await saveContext();
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
                UserName = request.UserSource.UserName,
                FirstName = request.UserSource.FirstName,
                SecondName = request.UserSource.SecondName,
                RequestId = request.Id
            };
        }

        private async void changeContext(UserRequest request)
        {
            var userSource = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.UserSourceId);
            switch (request.Request)
            {
                case RequestsEnum.BecomeDoctor:
                    userSource!.Role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == RolesEnum.Doctor.GetDescription());
                    _context.Update(userSource);
                    return;
            }
            int targetId = request.UserTargetId ?? 0;
            LastUserView viewUser = new LastUserView { };
            switch (request.Request)
            {
                case RequestsEnum.ViewAsDoctor:
                    viewUser.PatientId = targetId;
                    viewUser.DoctorId = userSource!.Id;
                    break;
                case RequestsEnum.InviteDoctor:
                    viewUser.PatientId = userSource!.Id;
                    viewUser.DoctorId = targetId;
                    break;
            }
            await _context.AddAsync(viewUser);
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