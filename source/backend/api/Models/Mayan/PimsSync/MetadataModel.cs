using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Sync
{
    public class MetadataModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}
