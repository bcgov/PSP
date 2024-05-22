using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Pims.Api.Models.CodeTypes;

using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Requests.Document.UpdateMetadata;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentService interface, defines the functionality for document services.
    /// </summary>
    public interface IDocumentService
    {
        Task<ExternalResponse<QueryResponse<DocumentTypeModel>>> GetStorageDocumentTypes(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResponse<QueryResponse<DocumentTypeMetadataTypeModel>>> GetDocumentTypeMetadataType(long mayanDocumentTypeId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResponse<QueryResponse<DocumentDetailModel>>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResponse<QueryResponse<DocumentMetadataModel>>> GetStorageDocumentMetadata(long mayanDocumentId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResponse<FileDownloadResponse>> DownloadFileAsync(long mayanDocumentId, long mayanFileId);

        Task<ExternalResponse<FileDownloadResponse>> DownloadFileLatestAsync(long mayanDocumentId);

        IList<PimsDocumentTyp> GetPimsDocumentTypes();

        IList<PimsDocumentTyp> GetPimsDocumentTypes(DocumentRelationType relationshipType);

        Task<DocumentUploadResponse> UploadDocumentAsync(DocumentUploadRequest uploadRequest);

        Task<DocumentUpdateResponse> UpdateDocumentAsync(DocumentUpdateRequest updateRequest);

        Task<ExternalResponse<string>> DeleteDocumentAsync(PimsDocument document);

        Task<ExternalResponse<DocumentDetailModel>> GetStorageDocumentDetail(long mayanDocumentId);

        Task<ExternalResponse<QueryResponse<FilePageModel>>> GetDocumentFilePageListAsync(long documentId, long documentFileId);

        Task<HttpResponseMessage> DownloadFilePageImageAsync(long mayanDocumentId, long mayanFileId, long mayanFilePageId);
    }
}
