using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ThrivoHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        [HttpGet]
        //test
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}
