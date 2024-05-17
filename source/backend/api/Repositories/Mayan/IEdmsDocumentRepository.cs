using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// IEdmsDocumentRepository interface, defines the functionality for a document repository.
    /// </summary>
    public interface IEdmsDocumentRepository
    {
        Task<ExternalResponse<DocumentTypeModel>> TryCreateDocumentTypeAsync(DocumentTypeModel documentType);

        Task<ExternalResponse<DocumentTypeModel>> TryUpdateDocumentTypeAsync(DocumentTypeModel documentType);

        Task<ExternalResponse<string>> TryDeleteDocumentTypeAsync(long documentTypeId);

        Task<ExternalResponse<QueryResponse<DocumentTypeModel>>> TryGetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResponse<QueryResponse<DocumentTypeMetadataTypeModel>>> TryGetDocumentTypeMetadataTypesAsync(long documentTypeId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResponse<QueryResponse<DocumentDetailModel>>> TryGetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResponse<QueryResponse<DocumentMetadataModel>>> TryGetDocumentMetadataAsync(long documentId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResponse<DocumentDetailModel>> TryGetDocumentAsync(long documentId);

        Task<ExternalResponse<FileDownloadResponse>> TryDownloadFileAsync(long documentId, long fileId);

        Task<ExternalResponse<string>> TryDeleteDocument(long documentId);

        Task<ExternalResponse<DocumentDetailModel>> TryUploadDocumentAsync(long documentType, IFormFile file);

        Task<ExternalResponse<DocumentMetadataModel>> TryCreateDocumentMetadataAsync(long documentId, long metadataTypeId, string value);

        Task<ExternalResponse<DocumentMetadataModel>> TryUpdateDocumentMetadataAsync(long documentId, long metadataId, string value);

        Task<ExternalResponse<string>> TryDeleteDocumentMetadataAsync(long documentId, long metadataId);

        Task<ExternalResponse<DocumentTypeMetadataTypeModel>> TryCreateDocumentTypeMetadataTypeAsync(long documentTypeId, long metadataTypeId, bool isRequired);

        Task<ExternalResponse<DocumentTypeMetadataTypeModel>> TryUpdateDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId, bool isRequired);

        Task<ExternalResponse<string>> TryDeleteDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId);

        Task<ExternalResponse<QueryResponse<FilePageModel>>> TryGetFilePageListAsync(long documentId, long documentFileId);

        Task<HttpResponseMessage> TryGetFilePageImage(long documentId, long documentFileId, long documentFilePageId);
    }
}
