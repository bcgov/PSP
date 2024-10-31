using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.PimsSync
{
    public class DocumentTypeModel
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }

        [JsonPropertyName("metadata_types")]
        public IList<DocumentMetadataTypeModel> MetadataTypes { get; set; }

        [JsonPropertyName("display_order")]
        public int DisplayOrder { get; set; }

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }
    }
}
