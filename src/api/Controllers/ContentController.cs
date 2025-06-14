using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        [HttpPost("add/to/block/list")]
        public async Task<IActionResult> Add(string content)
        {
            return Created();
        }
    }
}
