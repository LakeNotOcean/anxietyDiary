using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using API.Core;
using API.CRUD;
using API.DTO;
using Domain.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DatesController : BaseApiController
    {
        public DatesController(DataContext context) : base(context)
        {

        }

        [HttpGet("diary")]
        public async Task<IActionResult> GetDaysOfRecords(string name, string userName = "")
        {
            var currUserName = User.FindFirstValue(ClaimTypes.Name);
            if (!String.IsNullOrEmpty(userName))
            {
                var check = await _context.UserDoctors.Where(r => r.Doctor.UserName == currUserName && r.Patient.UserName == userName).SingleOrDefaultAsync();
                if (check is null)
                {
                    return HandleResult<List<DateTime>>(Result<List<DateTime>>.Failure("Wrong users Names"));
                }
                currUserName = userName;
            }
            var result = await Mediator.Send(new Dates.Query() { DiaryName = name, UserName = currUserName });
            return HandleResult(result);
        }


        [Authorize(Roles = "Doctor,Administrator")]
        [HttpGet("notview")]
        public async Task<IActionResult> GetNotViewDates()
        {
            var currUserName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Users.Where(u => u.UserName == currUserName).SingleOrDefaultAsync();

            var doctorPatients = await _context.UserDoctors.Where(ud => ud.DoctorId == user.Id).Include(ud => ud.Patient).ToListAsync();
            var result = new List<UserViewDTO> { };
            var diariesViewTasks = new List<Task<List<DiaryViewDTO>>>();
            foreach (var p in doctorPatients)
            {
                diariesViewTasks.Add(Mediator.Send(new LastDatesDiaries.Query() { userDoctor = p }));

            };
            var diaryViewList = new List<List<DiaryViewDTO>>();
            foreach (var task in diariesViewTasks)
            {
                diaryViewList.Add(await task);
            }
            for (int i = 0; i < doctorPatients.Count; ++i)
            {
                result.Add(new UserViewDTO
                {
                    userName = doctorPatients[i].Patient.UserName,
                    diariesViews = diaryViewList[i]
                });
            }
            return HandleResult(Result<List<UserViewDTO>>.Success(result));
        }

        [Authorize(Roles = "Doctor,Administrator")]
        [HttpGet("view")]

        public async Task<IActionResult> ViewDiary(string userName, string diaryName)
        {
            var currUserName = User.FindFirstValue(ClaimTypes.Name);
            var doctor = await _context.Users.Where(u => u.UserName == currUserName).SingleOrDefaultAsync();

            var userDoctor = await _context.UserDoctors.Include(ud => ud.Patient).Where(ud => ud.DoctorId == doctor.Id && ud.Patient.UserName == userName).SingleOrDefaultAsync();
            if (userDoctor is null)
            {
                return HandleResult(Result<Unit>.Failure("wrong user"));
            }

            var lastView = await _context.UsersViews.AsTracking().Where(uv => uv.DiaryName == diaryName && uv.UserDoctorId == userDoctor.Id).SingleOrDefaultAsync();
            lastView.LastViewDate = DateTime.UtcNow;
            _context.Entry(lastView).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync() > 0;
            if (!result)
            {
                return HandleResult(Result<Unit>.Failure("fail to update record"));
            }
            return HandleResult(Result<Unit>.Success(Unit.Value));
        }
    }

}