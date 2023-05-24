using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Constants;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Download;
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

        IList<PimsDocumentTyp> GetPimsDocumentTypes();

        IList<PimsDocumentTyp> GetPimsDocumentTypes(DocumentRelationType relationshipType);

        Task<DocumentUploadResponse> UploadDocumentAsync(DocumentUploadRequest uploadRequest);

        Task<DocumentUpdateResponse> UpdateDocumentAsync(DocumentUpdateRequest updateRequest);

        Task<ExternalResult<string>> DeleteDocumentAsync(PimsDocument document);

        Task<ExternalResult<DocumentDetail>> GetStorageDocumentDetail(long mayanDocumentId);
    }
}
