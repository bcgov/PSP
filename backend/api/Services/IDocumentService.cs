using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
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
        ExternalResult<QueryResult<DocumentType>> GetStorageDocumentTypes(string ordering = "", int? page = null, int? pageSize = null);

        ExternalResult<QueryResult<DocumentDetail>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null);

        ExternalResult<QueryResult<DocumentMetadata>> GetStorageDocumentMetadata(int documentId, string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<FileDownload>> DownloadFileAsync(int documentId, int fileId);

        Task<ExternalResult<FileDownload>> DownloadFileLatestAsync(int documentId);

        Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(int documentType, IFormFile fileRaw);

        IEnumerable<PimsDocumentTyp> GetPimsDocumentTypes();
    }
}
