using Azure;
using Azure.AI.ContentSafety;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            });
            return services;
        }

    }
}
