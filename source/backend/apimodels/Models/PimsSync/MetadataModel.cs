using System.Text.Json.Serialization;

namespace Pims.Api.Models.PimsSync
{
    public class MetadataModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}
