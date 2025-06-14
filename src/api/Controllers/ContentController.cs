using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using infrastructure.Services;
using api.Model;
namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController(IContentFilterService contentFilterService) : ControllerBase
    {
        //personal-data-protection
        [HttpPost("add/to/block/list")]
        public async Task<IActionResult> Add([FromBody] ContentModel contentModel)
        {
            var response = await contentFilterService.AddBlockText(contentModel.content, contentModel.blockListName, contentModel.blockListDescription);
            return Ok(response);
        }

        [HttpGet("list/{blockListName}")]
        public async Task<IActionResult> Add(string blockListName)
        {
            var response = await contentFilterService.GetBlockedText(blockListName);
            return Ok(response);
        }
        [HttpGet("analyse/{text}")]
        public async Task<IActionResult> Analyse(string text)
        {
            var response = await contentFilterService.Analyze(text);
            return Ok(response);
        }
    }
}
