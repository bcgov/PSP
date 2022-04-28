using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;

namespace Pims.Api.Repositories.EDMS
{
    /// <summary>
    /// IDocumentRepository interface, defines the functionality for a document repository.
    /// </summary>
    public interface IDocumentRepository
    {
        Task<ExternalResult<QueryResult<DocumentType>>> GetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<QueryResult<DocumentDetail>>> GetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null);

        Task<ExternalResult<FileDownload>> DownloadFileAsync(int documentId, int fileId);

        Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(int documentType, IFormFile file);

    }
}
