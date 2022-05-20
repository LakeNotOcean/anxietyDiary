using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using API.Core;
using API.CRUD;
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

        [HttpGet]
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
    }

}