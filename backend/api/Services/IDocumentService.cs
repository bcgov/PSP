using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentService interface, defines the functionality for document services.
    /// </summary>
    public interface IDocumentService
    {
        Task<ExternalResult<QueryResult<DocumentType>>> GetStorageDocumentTypes(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> GetDocumentTypeMetadataType(long mayanDocumentTypeId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentDetail>>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentMetadata>>> GetStorageDocumentMetadata(long mayanDocumentId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<FileDownload>> DownloadFileAsync(long mayanDocumentId, long mayanFileId);

        Task<ExternalResult<FileDownload>> DownloadFileLatestAsync(long mayanDocumentId);

        Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(long documentType, IFormFile fileRaw);

        IList<PimsDocumentTyp> GetPimsDocumentTypes();

        IList<PimsActivityInstanceDocument> GetActivityDocuments(long activityId);

        Task<bool> DeleteDocumentAsync(PimsDocument document, bool commitTransaction = true);

        Task<bool> DeleteActivityDocumentAsync(PimsActivityInstanceDocument activityDocument, bool commitTransaction = true);

        Task<DocumentUploadResponse> UploadActivityDocumentAsync(long activityId, DocumentUploadRequest uploadRequest);

        Task<DocumentUpdateResponse> UpdateActivityDocumentMetadataAsync(long documentId, DocumentUpdateRequest updateRequest);
    }
}
