using Azure;
using Azure.AI.ContentSafety;
using infrastructure.Filters;
using infrastructure.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.Net;

namespace infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContentSafety(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(x =>
            {
                x.AddContentSafetyClient(new Uri(configuration["ContentSafety:URI"]), new AzureKeyCredential(configuration["ContentSafety:Key"]));
                x.AddBlocklistClient(new Uri(configuration["ContentSafety:URI"]), new AzureKeyCredential(configuration["ContentSafety:Key"]));
            });
            services.AddTransient<IContentFilterService, ContentFilterService>();
            return services;
        }
        public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddTransient<Kernel>(_ =>
            {
                IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.Services.AddSingleton<IPromptRenderFilter, InputFilter>();
                kernelBuilder.Services.AddCustomCategorySafety(configuration);
                kernelBuilder.Services.AddContentSafety(configuration);
  
                kernelBuilder.Services.AddAzureOpenAIChatCompletion("o4-mini",
                      configuration["o4-mini-url"],
                      configuration["o4-mini"],
                       "o4-mini",
                       "o4-mini");
                return kernelBuilder.Build();
            });
        }
        public static IServiceCollection AddCustomCategorySafety(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICustomContentFilterService, CustomContentFilterService>();

            services.AddHttpClient<ICustomContentFilterService, CustomContentFilterService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ContentSafety:URI"]);
            });
            return services;

        }
    }
}
