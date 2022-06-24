using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Sync
{
    public class SyncModel
    {
        [JsonPropertyName("document_types")]
        public IList<DocumentTypeModel> DocumentTypes { get; set; }

        [JsonPropertyName("metadata_types")]
        public IList<string> MetadataTypes { get; set; }
    }
}