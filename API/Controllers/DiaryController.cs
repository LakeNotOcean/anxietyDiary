using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using Api.CRUD;
using API.Core;
using API.CRUD;
using Domain.Diaries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class DiaryController : BaseApiController
    {
        public DiaryController(DataContext context) : base(context)
        {

        }


        [HttpGet]
        public async Task<IActionResult> GetDayRecords(string name, DateTime date, [FromQuery] PagingParams param, string timezone, CancellationToken ct, string userName = "")
        {
            TimeZoneInfo timeZone;
            try
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            }
            catch (Exception)
            {
                return HandleResult<BaseDiary>(Result<BaseDiary>.Failure("timezone not found"));
            }
            var currUserName = User.FindFirstValue(ClaimTypes.Name);
            if (!String.IsNullOrEmpty(userName))
            {
                var check = await _context.UserDoctors.Where(r => r.Doctor.UserName == currUserName && r.Patient.UserName == userName).SingleOrDefaultAsync();
                if (check is null)
                {
                    return HandleResult<BaseDiary>(Result<BaseDiary>.Failure("Wrong usersNames"));
                }
                currUserName = userName;
            }
            var result = await Mediator.Send(new List.Query() { DiaryName = name, Date = date, Params = param, TimeZone = timeZone, UserName = currUserName });
            return HandleResultDynamic<BaseDiary>(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(JsonObject jsonParams)
        {
            var result = await Mediator.Send(new Create.Command { DiaryName = jsonParams["name"].ToString(), Body = jsonParams["body"].AsObject(), UserName = User.FindFirstValue(ClaimTypes.Name) });
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> EditRecord(string name, int id, JsonObject jsonBody)
        {

            var result = await Mediator.Send(new Edit.Command { DiaryName = name, Id = id, Body = jsonBody["body"].AsObject(), UserName = User.FindFirstValue(ClaimTypes.Name) });
            return HandleResult(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRecord(string name, int id)
        {
            var result = await Mediator.Send(new Delete.Command { DiaryName = name, Id = id, UserName = User.FindFirstValue(ClaimTypes.Name) });
            return HandleResult(result);
        }

    }
}