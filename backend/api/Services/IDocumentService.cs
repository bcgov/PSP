using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentService interface, defines the functionality for document services.
    /// </summary>
    public interface IDocumentService
    {
        ExternalResult<QueryResult<DocumentType>> GetDocumentTypes(string ordering = "", int? page = null, int? pageSize = null);

        ExternalResult<QueryResult<DocumentDetail>> GetDocumentList(string ordering = "", int? page = null, int? pageSize = null);

        ExternalResult<FileDownload> DownloadFile(int documentId, int fileId);

        ExternalResult<DocumentDetail> UploadDocument(int documentType, IFormFile fileRaw);
    }
}
