using infrastructure.Services;
using Microsoft.SemanticKernel;

namespace infrastructure.Filters
{
    public class InputFilter(IContentFilterService contentFilterService) : IPromptRenderFilter
    {
        public async Task OnPromptRenderAsync(PromptRenderContext context, Func<PromptRenderContext, Task> next)
        {
            await next(context);
            IEnumerable<string> categories = await contentFilterService.Analyze(context.RenderedPrompt);
            if (categories.Any())
            {
                throw new Exception($"{string.Join(",", categories)} content detected. Operation is denied.");
            }
        }
    }
}
