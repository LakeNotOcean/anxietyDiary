using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace anxietyDiary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> _logger;
        public BaseApiController(ILogger<BaseApiController> logger)
        {
            _logger = logger;

        }
    }
}