using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Services;
using Domain.Enums;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly DiaryService _diaryService;
        public UserController(DataContext context, DiaryService diaryService)
        {
            _diaryService = diaryService;
            _context = context;

        }


        [Authorize(Roles = "Doctor, Administrator")]
        [HttpGet("searchUsers")]
        public async Task<ActionResult<List<UserInfoDTO>>> SearchUsers(string str)
        {
            var foundUsers = await _context.Users.Where(u => u.UserName.Contains(str) && u.isSearching).Include(u => u.Role).ToListAsync();
            var result = new List<UserInfoDTO> { };
            foreach (var u in foundUsers)
            {
                result.Add(createUserObject(u));
            }
            return result;
        }

        [HttpGet("searchDoctors")]
        public async Task<ActionResult<List<UserInfoDTO>>> SearchDoctors(string str)
        {
            var foundUsers = await _context.Users.Where(u => u.UserName.Contains(str) && u.isSearching && u.Role.Name == RolesEnum.Doctor.GetDescription()).Include(u => u.Role).ToListAsync();
            var result = new List<UserInfoDTO> { };
            foreach (var u in foundUsers)
            {
                result.Add(createUserObject(u));
            }
            return result;
        }

        [Authorize(Roles = "Doctor,Administrator")]
        [HttpGet("patients")]
        public async Task<ActionResult<List<UserInfoDTO>>> getPatientsList()
        {
            var doctorId = getCurrentUserId();
            var patients = await _context.Users.Where(u =>
                _context.UserDoctors.Any(view => view.DoctorId == doctorId && view.PatientId == u.Id)).ToListAsync();
            return getUsersInfo(ref patients);
        }

        // [Authorize(Roles = "Doctor,Administrator")]
        // [HttpGet("patient/{name:str}")]
        // public async Task<ActionResult<UserInfoDTO>> getPatient(string name)
        // {
        //     var doctor 
        //     var patient = await _context.Users.SingleOrDefaultAsync(u =>
        //         _context.UsersViews.Any(view => view.DoctorId == doctor.Id && view.PatientId == u.Id && u.UserName == name));
        //     if (patient is null)
        //     {
        //         return BadRequest("user not found");
        //     }
        //     return createUserObject(patient);
        // }

        [HttpGet("doctors")]
        public async Task<ActionResult<List<UserInfoDTO>>> getDoctorsList()
        {
            var patientId = getCurrentUserId();
            var doctors = await _context.Users.Where(u =>
                _context.UserDoctors.Any(view => view.DoctorId == u.Id && view.PatientId == patientId)).ToListAsync();
            return getUsersInfo(ref doctors);
        }

        [HttpDelete("remove")]
        public async Task<ActionResult> removeUserView(string name, bool isDoctor = false)
        {
            var userId = getCurrentUserId();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var targetUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == name);
            if (targetUser == null || user == null)
            {
                return BadRequest("Bad user data");
            }

            int doctorId, patientId;
            doctorId = isDoctor ? targetUser.Id : user.Id;
            patientId = isDoctor ? user.Id : targetUser.Id;

            var connect = await _context.UserDoctors.SingleOrDefaultAsync(view => view.DoctorId == doctorId && view.PatientId == patientId);
            if (connect is null)
            {
                return BadRequest("users are not connected");
            }
            var views = await _context.UsersViews.Where(uv => uv.UserDoctorId == connect.Id).ToListAsync();
            _context.RemoveRange(views);
            _context.Remove(connect);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        private List<UserInfoDTO> getUsersInfo(ref List<User> data)
        {
            var result = new List<UserInfoDTO> { };

            foreach (var u in data)
            {
                result.Add(createUserObject(u));
            }

            return result;
        }

        private int getCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }




        private UserInfoDTO createUserObject(User user)
        {
            return new UserInfoDTO
            {
                userName = user.UserName,
                firstName = user.FirstName,
                secondName = user.SecondName,
                description = user.Description
            };
        }

    }
}