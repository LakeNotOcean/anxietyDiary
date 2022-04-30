using System;
using System.Threading.Tasks;
using API.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Persistance;

namespace anxietyDiary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected readonly DataContext _context;
        public BaseApiController(DataContext context)
        {
            _context = context;

        }
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected async Task<ActionResult> saveContext()
        {
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

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result is null)
            {
                return NotFound();
            }
            if (result.isSuccess && result.Value is not null)
            {
                return Ok(result.Value);
            }
            if (result.isSuccess)
            {
                return NotFound();
            }
            return BadRequest(result);
        }

    }
}