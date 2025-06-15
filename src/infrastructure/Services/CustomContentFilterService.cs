namespace infrastructure.Services
{
    using System.Text.Json;
    using System.Text;

    using Microsoft.Extensions.Configuration;

    public class CustomContentFilterService: ICustomContentFilterService
    {
        public record CustomCategoryAnalysisResponse(CustomCategoryAnalysis customCategoryAnalysis);
        public record CustomCategoryAnalysis(bool detected);
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
                string result= await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<CustomCategoryAnalysisResponse>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if(data?.customCategoryAnalysis?.detected == true)
                {
                    return categoryName;
                }
                return string.Empty;
      
            }
            throw new Exception($"Error analyzing custom category: {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}");
        }
    }
}
