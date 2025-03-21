using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("[Action]")]
        public IActionResult Test()
        {
            return Ok("Test endpoint works.");
        }
    }
}
