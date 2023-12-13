using System.Collections.Generic;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Models.PimsSync
{
    public class ExternalBatchResult
    {
        public ExternalBatchResult()
        {
            DeletedMetadata = new List<ExternalResponse<string>>();
            CreatedMetadata = new List<ExternalResponse<MetadataTypeModel>>();
            UpdatedMetadata = new List<ExternalResponse<MetadataTypeModel>>();
            DeletedDocumentType = new List<ExternalResponse<string>>();
            CreatedDocumentType = new List<ExternalResponse<Mayan.Document.DocumentTypeModel>>();
            UpdatedDocumentType = new List<ExternalResponse<Mayan.Document.DocumentTypeModel>>();
            DeletedDocumentTypeMetadataType = new List<ExternalResponse<string>>();
            LinkedDocumentMetadataTypes = new List<ExternalResponse<DocumentTypeMetadataTypeModel>>();
        }

        public List<ExternalResponse<string>> DeletedMetadata { get; set; }

        public List<ExternalResponse<MetadataTypeModel>> CreatedMetadata { get; set; }

        public List<ExternalResponse<MetadataTypeModel>> UpdatedMetadata { get; set; }

        public List<ExternalResponse<string>> DeletedDocumentType { get; set; }

        public List<ExternalResponse<Mayan.Document.DocumentTypeModel>> CreatedDocumentType { get; set; }

        public List<ExternalResponse<Mayan.Document.DocumentTypeModel>> UpdatedDocumentType { get; set; }

        public List<ExternalResponse<string>> DeletedDocumentTypeMetadataType { get; set; }

        public List<ExternalResponse<DocumentTypeMetadataTypeModel>> LinkedDocumentMetadataTypes { get; set; }
    }
}
