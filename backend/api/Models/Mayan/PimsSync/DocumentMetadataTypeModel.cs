using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Sync
{
    public class DocumentMetadataTypeModel
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }
}