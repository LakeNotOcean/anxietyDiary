using System.Threading.Tasks;
using anxietyDiary.Controllers;
using API.CRUD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class DatesController : BaseApiController
    {
        public DatesController(DataContext context) : base(context)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetDaysOfRecords(string name)
        {
            var result = await Mediator.Send(new Dates.Query() { DiaryName = name });
            return HandleResult(result);
        }
    }

}