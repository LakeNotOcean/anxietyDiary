using System;
using System.Threading;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]

    public class DiaryController : BaseApiController
    {
        public DiaryController(DataContext context) : base(context)
        {

        }

        [HttpGet("{name}/{count:int}/{date}")]
        public async Task<IActionResult> GetDayRecords(string name, int count, DateTime date, CancellationToken ct)
        {

        }
        [HttpGet("{name}/{date}")]
        public async Task<IActionResult> GetDayRecordsAll(string diaryName, DateTime date, CancellationToken ct)
        {
            return await Mediator.Send(new )
        }

    }
}