using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Repositories.Mayan;
using Pims.Av;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentService implementation provides document managing capabilities.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IEdmsDocumentRepository documentRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;

        private readonly IAvService avService;

        public DocumentService(IEdmsDocumentRepository documentRepository, IDocumentTypeRepository documentTypeRepository, IAvService avService)
        {
            this.documentRepository = documentRepository;
            this.documentTypeRepository = documentTypeRepository;
            this.avService = avService;
        }

        public ExternalResult<QueryResult<DocumentType>> GetStorageDocumentTypes(string ordering = "", int? page = null, int? pageSize = null)
        {
            Task<ExternalResult<QueryResult<DocumentType>>> task = documentRepository.GetDocumentTypesAsync(ordering, page, pageSize);
            task.Wait();
            return task.Result;
        }

        public ExternalResult<QueryResult<DocumentDetail>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null)
        {
            Task<ExternalResult<QueryResult<DocumentDetail>>> task = documentRepository.GetDocumentsListAsync(ordering, page, pageSize);
            task.Wait();
            return task.Result;
        }

        public ExternalResult<QueryResult<DocumentMetadata>> GetStorageDocumentMetadata(int documentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            Task<ExternalResult<QueryResult<DocumentMetadata>>> task = documentRepository.GetDocumentMetadataAsync(documentId, ordering, page, pageSize);
            task.Wait();
            return task.Result;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileAsync(int documentId, int fileId)
        {
            ExternalResult<FileDownload> downloadResult = await documentRepository.DownloadFileAsync(documentId, fileId);
            return downloadResult;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileLatestAsync(int documentId)
        {
            ExternalResult<DocumentDetail> documentResult = await documentRepository.GetDocumentAsync(documentId);
            ExternalResult<FileDownload> downloadResult = await documentRepository.DownloadFileAsync(documentResult.Payload.Id, documentResult.Payload.FileLatest.Id);
            return downloadResult;
        }

        public async Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(int documentType, IFormFile fileRaw)
        {
            await this.avService.ScanAsync(fileRaw);
            Task<ExternalResult<DocumentDetail>> task = documentRepository.UploadDocumentAsync(documentType, fileRaw);
            task.Wait();
            return task.Result;
        }

        public IEnumerable<PimsDocumentTyp> GetPimsDocumentTypes()
        {
            return documentTypeRepository.GetAll();
        }
    }
}
