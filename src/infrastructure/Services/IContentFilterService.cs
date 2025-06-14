using Azure.AI.ContentSafety;

namespace infrastructure.Services
{
    public interface IContentFilterService
    {
        Task<IReadOnlyList<TextBlocklistItem>> AddBlockText(IEnumerable<string> blackList, string blockListName, string blockListDescription);
        Task<IEnumerable<string>> GetBlockedText(string blockListName);
        Task<IEnumerable<string>> Analyse(string text);
    }
}
