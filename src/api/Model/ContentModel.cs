using System.Text.Json.Serialization;

namespace api.Model
{
    public record ContentModel
    {
        public string[] content { get; set; }
        public string blockListName { get; set; }
        public string blockListDescription { get; set; }
    }
}
