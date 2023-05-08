using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Download;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// IEdmsDocumentRepository interface, defines the functionality for a document repository.
    /// </summary>
    public interface IEdmsDocumentRepository
    {
        Task<ExternalResult<DocumentType>> TryCreateDocumentTypeAsync(DocumentType documentType);

        Task<ExternalResult<DocumentType>> TryUpdateDocumentTypeAsync(DocumentType documentType);

        Task<ExternalResult<string>> TryDeleteDocumentTypeAsync(long documentTypeId);

        Task<ExternalResult<QueryResult<DocumentType>>> TryGetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> TryGetDocumentTypeMetadataTypesAsync(long documentTypeId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentDetail>>> TryGetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentMetadata>>> TryGetDocumentMetadataAsync(long documentId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<DocumentDetail>> TryGetDocumentAsync(long documentId);

        Task<ExternalResult<FileDownload>> TryDownloadFileAsync(long documentId, long fileId);

        Task<ExternalResult<string>> TryDeleteDocument(long documentId);

        Task<ExternalResult<DocumentDetail>> TryUploadDocumentAsync(long documentType, IFormFile file);

        Task<ExternalResult<DocumentMetadata>> TryCreateDocumentMetadataAsync(long documentId, long metadataTypeId, string value);

        Task<ExternalResult<DocumentMetadata>> TryUpdateDocumentMetadataAsync(long documentId, long metadataId, string value);

        Task<ExternalResult<string>> TryDeleteDocumentMetadataAsync(long documentId, long metadataId);

        Task<ExternalResult<DocumentTypeMetadataType>> TryCreateDocumentTypeMetadataTypeAsync(long documentTypeId, long metadataTypeId, bool isRequired);

        Task<ExternalResult<DocumentTypeMetadataType>> TryUpdateDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId, bool isRequired);

        Task<ExternalResult<string>> TryDeleteDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId);
    }
}
