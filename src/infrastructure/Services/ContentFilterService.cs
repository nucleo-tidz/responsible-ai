using Azure;
using Azure.AI.ContentSafety;
using Azure.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace infrastructure.Services
{
    public class ContentFilterService(ContentSafetyClient contentSafetyClient, BlocklistClient blocklistClient) : IContentFilterService
    {
        private async Task<(int, string)> UpsertBlockList(string name, string description)
        {
            var data = new
            {
                description = description,
            };
            var response = await blocklistClient.CreateOrUpdateTextBlocklistAsync(name, RequestContent.Create(data));
            return (response.Status, response.ReasonPhrase);
        }
        public async Task<IReadOnlyList<TextBlocklistItem>> AddBlockText(IEnumerable<string> blackList, string blockListName, string blockListDescription)
        {
            await this.UpsertBlockList(blockListName, blockListDescription);
            TextBlocklistItem[] blockItems = blackList.Select(_ => new TextBlocklistItem(_)).ToArray();
            var response = await blocklistClient.AddOrUpdateBlocklistItemsAsync(blockListName, new AddOrUpdateTextBlocklistItemsOptions(blockItems));
            return response.Value.BlocklistItems;
        }
        public async Task<IEnumerable<string>> GetBlockedText(string blockListName)
        {
            List<string> blockedText = new List<string>();
            var response = blocklistClient.GetTextBlocklistItems(blockListName);
            foreach (var blocklistItem in response)
            {
                blockedText.Add(blocklistItem.Text);
            }
            return await Task.FromResult(blockedText);
        }
        public async Task<IEnumerable<string>> Analyse(string text)
        {
            var request = new AnalyzeTextOptions(text);
            var blockLists = this.GetBlockList();
            foreach (var item in blockLists)
            {
                request.BlocklistNames.Add(item);
            }
            request.HaltOnBlocklistHit = true;
            Response<AnalyzeTextResult> analysisResult = await contentSafetyClient.AnalyzeTextAsync(request);
            foreach (var analysis in analysisResult.Value.CategoriesAnalysis)
            {
                if (analysis.Severity > 0)
                {
                    return new string[] { analysis.Category.ToString() };
                }
            }
            if (analysisResult.Value.BlocklistsMatch != null)
            {
                return analysisResult.Value.BlocklistsMatch.Select(_ => _.BlocklistName);
            }
            return Enumerable.Empty<string>();
        }
        private IEnumerable<string> GetBlockList()
        {
            var response = blocklistClient.GetTextBlocklists();
            return response.Select(blocklist => blocklist.Name).ToList();
        }
    }
}
