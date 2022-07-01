using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Mayan.Sync;
using Pims.Api.Repositories.Mayan;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentService implementation provides document managing capabilities.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IEdmsDocumentRepository documentRepository;
        private readonly IEdmsMetadataRepository metadataRepository;

        public DocumentService(IEdmsDocumentRepository documentRepository, IEdmsMetadataRepository metadataRepository)
        {
            this.documentRepository = documentRepository;
            this.metadataRepository = metadataRepository;
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

        public ExternalResult<DocumentDetail> UploadDocument(int documentType, IFormFile fileRaw)
        {
            Task<ExternalResult<DocumentDetail>> task = documentRepository.UploadDocumentAsync(documentType, fileRaw);
            task.Wait();
            return task.Result;
        }

        public bool SyncMetadataTypes(SyncModel model)
        {
            Task<ExternalResult<QueryResult<MetadataType>>> retrieveTask = metadataRepository.GetMetadataTypesAsync(pageSize: 5000);
            retrieveTask.Wait();
            var metadata = retrieveTask.Result;

            // Add the metadata types not in mayan
            IList<Task<ExternalResult<MetadataType>>> createTasks = new List<Task<ExternalResult<MetadataType>>>();
            foreach (var metadataTypeLabel in model.MetadataTypes)
            {
                if (metadata.Payload.Results.FirstOrDefault(x => x.Label == metadataTypeLabel) == null)
                {
                    var newMetadataType = new MetadataType() { Label = metadataTypeLabel, Name = metadataTypeLabel.Replace(' ', '_') };
                    createTasks.Add(metadataRepository.CreateMetadataTypeAsync(newMetadataType));
                }
            }

            Task.WaitAll(createTasks.ToArray());

            if (model.RemoveLingeringMetadataTypes)
            {
                // Delete the metadata types that are not on the sync model
                IList<Task<ExternalResult<bool>>> deleteTasks = new List<Task<ExternalResult<bool>>>();
                foreach (var metadataType in metadata.Payload.Results)
                {
                    if (model.MetadataTypes.IndexOf(metadataType.Label) < 0)
                    {
                        deleteTasks.Add(metadataRepository.DeleteMetadataTypeAsync(metadataType.Id));
                    }
                }

                Task.WaitAll(deleteTasks.ToArray());
            }

            return true;
        }

        public bool SyncDocumentTypes(SyncModel model)
        {
            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = documentRepository.GetDocumentTypesAsync(pageSize: 5000);
            documentTypeTask.Wait();

            ExternalResult<QueryResult<DocumentType>> documentTypesRetrieved = documentTypeTask.Result;

            // Add the document types not in mayan
            IList<Task<ExternalResult<DocumentType>>> createTasks = new List<Task<ExternalResult<DocumentType>>>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                if (documentTypesRetrieved.Payload.Results.FirstOrDefault(x => x.Label == documentTypeModel.Label) == null)
                {
                    createTasks.Add(documentRepository.CreateDocumentTypeAsync(new DocumentType() { Label = documentTypeModel.Label }));
                }
            }
            Task.WaitAll(createTasks.ToArray());

            if (model.RemoveLingeringDocumentTypes)
            {
                // Delete the document types that are not on the sync model
                IList<Task<ExternalResult<bool>>> deleteTasks = new List<Task<ExternalResult<bool>>>();
                foreach (var documentType in documentTypesRetrieved.Payload.Results)
                {
                    if (model.DocumentTypes.FirstOrDefault(x => x.Label == documentType.Label) == null)
                    {
                        deleteTasks.Add(documentRepository.DeleteDocumentTypeAsync(documentType.Id));
                    }
                }

                Task.WaitAll(deleteTasks.ToArray());
            }

            SyncDocumentTypeMetadataTypes(model);

            return true;
        }

        private void SyncDocumentTypeMetadataTypes(SyncModel model)
        {
            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = documentRepository.GetDocumentTypesAsync(pageSize: 5000);
            Task<ExternalResult<QueryResult<MetadataType>>> metadataTypesTask = metadataRepository.GetMetadataTypesAsync(pageSize: 5000);
            Task.WaitAll(documentTypeTask, metadataTypesTask);

            var retrievedMetadataTypes = metadataTypesTask.Result;

            // Check that the metadata types on the documents are the same as the ones in the specified in the model
            IList<Task<bool>> linkTasks = new List<Task<bool>>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                linkTasks.Add(AsyncLinkDocumentMetadata(documentTypeModel, documentTypeTask.Result.Payload.Results, retrievedMetadataTypes.Payload.Results));
            }

            Task.WaitAll(linkTasks.ToArray());
        }

        private async Task<bool> AsyncLinkDocumentMetadata(DocumentTypeModel documentTypeModel, IList<DocumentType> retrievedDocumentTypes, IList<MetadataType> retrievedMetadataTypes)
        {
            var documentType = retrievedDocumentTypes.FirstOrDefault(x => x.Label == documentTypeModel.Label);
            ExternalResult<QueryResult<DocumentTypeMetadataType>> documentTypeMetadataTypes = await documentRepository.GetDocumentTypeMetadataTypesAsync(documentType.Id, pageSize: 5000);

            // Update the document type's metadata types
            IList<Task<ExternalResult<DocumentTypeMetadataType>>> documentTypeMetadataTypeTasks = new List<Task<ExternalResult<DocumentTypeMetadataType>>>();
            var retrievedLinks = documentTypeMetadataTypes.Payload.Results;
            foreach (var metadataTypeModel in documentTypeModel.MetadataTypes)
            {
                DocumentTypeMetadataType existingDocumentTypeMetadaType = retrievedLinks.FirstOrDefault(x => x.MetadataType.Label == metadataTypeModel.Label);
                if (existingDocumentTypeMetadaType == null)
                {
                    MetadataType metadataType = retrievedMetadataTypes.FirstOrDefault(x => x.Label == metadataTypeModel.Label);
                    documentTypeMetadataTypeTasks.Add(documentRepository.CreateDocumentTypeMetadataTypeAsync(documentType.Id, metadataType.Id, metadataTypeModel.Required));
                }
                else
                {
                    if (existingDocumentTypeMetadaType.Required != metadataTypeModel.Required)
                    {
                        documentTypeMetadataTypeTasks.Add(documentRepository.UpdateDocumentTypeMetadataTypeAsync(documentType.Id, existingDocumentTypeMetadaType.Id, metadataTypeModel.Required));
                    }
                }
            }
            Task.WaitAll(documentTypeMetadataTypeTasks.ToArray());

            // Get metadata types not on sync model
            IList<Task> deleteTasks = new List<Task>();
            foreach (var documentTypeMetadataType in documentTypeMetadataTypes.Payload.Results)
            {
                if (documentTypeModel.MetadataTypes.FirstOrDefault(x => x.Label == documentTypeMetadataType.MetadataType.Label) == null)
                {
                    deleteTasks.Add(documentRepository.DeleteDocumentTypeMetadataTypeAsync(documentType.Id, documentTypeMetadataType.Id));
                }
            }

            Task.WaitAll(deleteTasks.ToArray());

            return true;
        }
    }
}
