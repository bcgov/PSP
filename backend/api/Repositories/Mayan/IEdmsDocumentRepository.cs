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

        Task<ExternalResult<bool>> DeleteDocumentTypeAsync(long documentTypeId);

        Task<ExternalResult<QueryResult<DocumentType>>> GetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> GetDocumentTypeMetadataTypesAsync(long documentId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentDetail>>> GetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<FileDownload>> DownloadFileAsync(int documentId, int fileId);

        Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(int documentType, IFormFile file);

        Task<ExternalResult<DocumentTypeMetadataType>> CreateDocumentTypeMetadataTypeAsync(long documentTypeId, long metadataTypeId, bool isRequired);

        Task<ExternalResult<DocumentTypeMetadataType>> UpdateDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId, bool isRequired);

        Task<ExternalResult<bool>> DeleteDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId);

    }
}
