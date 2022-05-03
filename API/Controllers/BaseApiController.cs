using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Core;
using API.Extensions;
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

        protected ActionResult HandleResultDynamic<T>(Result<T> result)
        {
            if (result is null)
            {
                return NotFound();
            }
            if (result.isSuccess && result.Value is not null)
            {
                var realResult = Convert.ChangeType(result.Value, result.Value.GetType());
                return Ok(realResult);
            }
            if (result.isSuccess)
            {
                return NotFound();
            }
            return BadRequest(result);
        }
        protected ActionResult HandleResultDynamic<T>(Result<PageList<T>> result)
        {
            if (result is null)
            {
                return NotFound();
            }
            if (result.isSuccess && result.Value.Count != 0)
            {
                var resultType = result.Value[0].GetType();
                Type listType = typeof(List<>).MakeGenericType(new[] { resultType });
                IList list = (IList)Activator.CreateInstance(listType);
                foreach (var el in result.Value)
                {
                    list.Add(el);
                }
                Response.AddPaginationHeader(result.Value.CurrentPage,
                                             result.Value.PageSize,
                                             result.Value.TotalCount,
                                             result.Value.TotalPages);
                return Ok(list);
            }
            if (result.isSuccess)
            {
                return NotFound();
            }
            return BadRequest(result);
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