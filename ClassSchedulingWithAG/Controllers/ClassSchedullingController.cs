using Microsoft.AspNetCore.Mvc;

namespace ClassSchedulingWithAG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassSchedullingController : ControllerBase
    {
      
        private readonly ILogger<ClassSchedullingController> _logger;

        public ClassSchedullingController(ILogger<ClassSchedullingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Post()
        {


            return Ok();
        }
    }
}
