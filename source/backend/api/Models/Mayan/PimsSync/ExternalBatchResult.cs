using System.Collections.Generic;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;

namespace Pims.Api.Models.Mayan.Sync
{
    public class ExternalBatchResult
    {
        public ExternalBatchResult()
        {
            DeletedMetadata = new List<ExternalResult<string>>();
            CreatedMetadata = new List<ExternalResult<MetadataType>>();
            UpdatedMetadata = new List<ExternalResult<MetadataType>>();
            DeletedDocumentType = new List<ExternalResult<string>>();
            CreatedDocumentType = new List<ExternalResult<DocumentType>>();
            UpdatedDocumentType = new List<ExternalResult<DocumentType>>();
            DeletedDocumentTypeMetadataType = new List<ExternalResult<string>>();
            LinkedDocumentMetadataTypes = new List<ExternalResult<DocumentTypeMetadataType>>();
        }

        public List<ExternalResult<string>> DeletedMetadata { get; set; }

        public List<ExternalResult<MetadataType>> CreatedMetadata { get; set; }

        public List<ExternalResult<MetadataType>> UpdatedMetadata { get; set; }

        public List<ExternalResult<string>> DeletedDocumentType { get; set; }

        public List<ExternalResult<DocumentType>> CreatedDocumentType { get; set; }

        public List<ExternalResult<DocumentType>> UpdatedDocumentType { get; set; }

        public List<ExternalResult<string>> DeletedDocumentTypeMetadataType { get; set; }

        public List<ExternalResult<DocumentTypeMetadataType>> LinkedDocumentMetadataTypes { get; set; }
    }
}
