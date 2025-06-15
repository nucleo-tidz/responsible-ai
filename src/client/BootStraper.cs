using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace client
{
    internal class BootStrapper(Kernel _kernel) : IBootStrapper
    {
        public async Task Start()
        {
            var _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            Console.WriteLine("I am ready");
            while (true)
            {
                string query = Console.ReadLine();
                var result = await _kernel.InvokePromptAsync(query);
                Console.WriteLine(result.ToString());
            }
        }
    }
}
