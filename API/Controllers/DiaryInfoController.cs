using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using Domain.DiaryExpensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class DiaryInfoController : BaseApiController
    {
        public DiaryInfoController(DataContext context) : base(context)
        {
        }


        [HttpGet("categories")]
        public async Task<ActionResult<List<DiaryCategory>>> getCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("descriptions")]
        public async Task<ActionResult<List<DiaryDescription>>> getDiariesInfo()
        {
            return await _context.Descriptions.Include(d => d.ArbitraryColumns)
                .Include(d => d.NonArbitraryColumns)
                .ToListAsync();
        }
    }
}