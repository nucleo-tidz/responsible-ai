namespace infrastructure.Services
{
    using System.Text.Json;
    using System.Text;

    using Microsoft.Extensions.Configuration;

    public class CustomContentFilterService: ICustomContentFilterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _subscriptionKey;

        public CustomContentFilterService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _subscriptionKey = configuration["ContentSafety:Key"];
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
        }
        public async Task<string> AnalyzeCustomCategoryAsync(string text, string categoryName)
        {
            var requestBody = new
            {
                Text = text,
                CategoryName = categoryName,
                Version = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/contentsafety/text:analyzeCustomCategory?api-version=2024-02-15-preview", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed with status {response.StatusCode}: {error}");
        }
    }
}
