using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using Api.CRUD;
using API.CRUD;
using API.DTO;
using Domain.Diaries;
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

        // [HttpGet("{name}/{count:int}/{date}")]
        // public async Task<IActionResult> GetDayRecords(string name, int count, DateTime date, CancellationToken ct)
        // {

        // }

        [HttpGet]
        public async Task<IActionResult> GetDayRecordsAll(string name, DateTime date, CancellationToken ct)
        {
            var result = await Mediator.Send(new List.Query() { DiaryName = name, Date = date });
            return HandleResultDynamic<BaseDiary>(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(string name, JsonObject body)
        {
            var result = await Mediator.Send(new Create.Command { DiaryName = name, Body = body });
            return HandleResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> EditRecord(string name, int id, JsonObject jsonBody)
        {
            var result = await Mediator.Send(new Edit.Command { DiaryName = name, Id = 1, Body = jsonBody["body"].AsObject() });
            return HandleResult(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRecord(string name, int id)
        {
            var result = await Mediator.Send(new Delete.Command { DiaryName = name, Id = id });
            return HandleResult(result);
        }

    }
}