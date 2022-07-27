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
        private readonly IDocumentActivityRepository documentActivityRespostory;
        private readonly IEdmsDocumentRepository documentStorageRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IAvService avService;

        public DocumentService(IDocumentActivityRepository documentActivityRespostory, IEdmsDocumentRepository documentRepository, IDocumentTypeRepository documentTypeRepository, IAvService avService)
        {
            this.documentActivityRespostory = documentActivityRespostory;
            this.documentStorageRepository = documentRepository;
            this.documentTypeRepository = documentTypeRepository;
            this.avService = avService;
        }

        public IList<PimsDocumentTyp> GetPimsDocumentTypes()
        {
            return documentTypeRepository.GetAll();
        }

        public IList<PimsActivityInstanceDocument> GetActivityDocuments(long activityId)
        {
            return documentActivityRespostory.GetAll(activityId);
        }

        public async Task<ExternalResult<QueryResult<DocumentType>>> GetStorageDocumentTypes(string ordering = "", int? page = null, int? pageSize = null)
        {
            ExternalResult<QueryResult<DocumentType>> result = await documentStorageRepository.GetDocumentTypesAsync(ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentDetail>>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null)
        {
            ExternalResult<QueryResult<DocumentDetail>> result = await documentStorageRepository.GetDocumentsListAsync(ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentMetadata>>> GetStorageDocumentMetadata(long mayanDocumentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            ExternalResult<QueryResult<DocumentMetadata>> result = await documentStorageRepository.GetDocumentMetadataAsync(mayanDocumentId, ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileAsync(long mayanDocumentId, long mayanFileId)
        {
            ExternalResult<FileDownload> downloadResult = await documentStorageRepository.DownloadFileAsync(mayanDocumentId, mayanFileId);
            return downloadResult;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileLatestAsync(long mayanDocumentId)
        {
            ExternalResult<DocumentDetail> documentResult = await documentStorageRepository.GetDocumentAsync(mayanDocumentId);
            if (documentResult.Status == ExternalResultStatus.Success)
            {
                if (documentResult.Payload != null)
                {
                    ExternalResult<FileDownload> downloadResult = await documentStorageRepository.DownloadFileAsync(documentResult.Payload.Id, documentResult.Payload.FileLatest.Id);
                    return downloadResult;
                }
                else
                {
                    return new ExternalResult<FileDownload>()
                    {
                        Status = ExternalResultStatus.Error,
                        Message = $"No document with id ${mayanDocumentId} found in the storage",
                    };
                }
            }
            else
            {
                return new ExternalResult<FileDownload>()
                {
                    Status = documentResult.Status,
                    Message = documentResult.Message,
                    HttpStatusCode = documentResult.HttpStatusCode,
                };
            }
        }

        public async Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(int documentType, IFormFile fileRaw)
        {
            await this.avService.ScanAsync(fileRaw);
            ExternalResult<DocumentDetail> result = await documentStorageRepository.UploadDocumentAsync(documentType, fileRaw);
            return result;
        }
    }
}
