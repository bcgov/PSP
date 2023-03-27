using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Sync
{
    public class DocumentTypeModel
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("metadata_types")]
        public IList<DocumentMetadataTypeModel> MetadataTypes { get; set; }

        [JsonPropertyName("display_order")]
        public int DisplayOrder { get; set; }
    }
}
