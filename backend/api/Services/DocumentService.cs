using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Repositories.Mayan;
using Pims.Av;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentService implementation provides document managing capabilities.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IEdmsDocumentRepository documentRepository;
        private readonly IAvService avService;

        public DocumentService(IEdmsDocumentRepository documentRepository, IAvService avService)
        {
            this.documentRepository = documentRepository;
            this.avService = avService;
        }

        public ExternalResult<QueryResult<DocumentType>> GetDocumentTypes(string ordering = "", int? page = null, int? pageSize = null)
        {
            Task<ExternalResult<QueryResult<DocumentType>>> task = documentRepository.GetDocumentTypesAsync(ordering, page, pageSize);
            task.Wait();
            return task.Result;
        }

        public ExternalResult<QueryResult<DocumentDetail>> GetDocumentList(string ordering = "", int? page = null, int? pageSize = null)
        {
            Task<ExternalResult<QueryResult<DocumentDetail>>> task = documentRepository.GetDocumentsListAsync(ordering, page, pageSize);
            task.Wait();
            return task.Result;
        }

        public ExternalResult<FileDownload> DownloadFile(int documentId, int fileId)
        {
            Task<ExternalResult<FileDownload>> task = documentRepository.DownloadFileAsync(documentId, fileId);
            task.Wait();
            return task.Result;
        }

        public async Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(int documentType, IFormFile fileRaw)
        {
            await this.avService.ScanAsync(fileRaw);
            Task<ExternalResult<DocumentDetail>> task = documentRepository.UploadDocumentAsync(documentType, fileRaw);
            task.Wait();
            return task.Result;
        }
    }
}
