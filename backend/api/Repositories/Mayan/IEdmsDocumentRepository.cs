using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// IEdmsDocumentRepository interface, defines the functionality for a document repository.
    /// </summary>
    public interface IEdmsDocumentRepository
    {
        Task<ExternalResult<DocumentType>> CreateDocumentTypeAsync(DocumentType documentType);

        Task<ExternalResult<string>> DeleteDocumentTypeAsync(long documentTypeId);

        Task<ExternalResult<QueryResult<DocumentType>>> GetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> GetDocumentTypeMetadataTypesAsync(long documentTypeId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentDetail>>> GetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentMetadata>>> GetDocumentMetadataAsync(long documentId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<DocumentDetail>> GetDocumentAsync(long documentId);

        Task<ExternalResult<FileDownload>> DownloadFileAsync(long documentId, long fileId);

        Task<ExternalResult<string>> DeleteDocument(long documentId);

        Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(long documentType, IFormFile file);

        Task<ExternalResult<DocumentTypeMetadataType>> CreateDocumentTypeMetadataTypeAsync(long documentTypeId, long metadataTypeId, bool isRequired);

        Task<ExternalResult<DocumentTypeMetadataType>> UpdateDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId, bool isRequired);

        Task<ExternalResult<string>> DeleteDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId);
    }
}
