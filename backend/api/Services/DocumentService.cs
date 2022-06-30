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

            // Get metadata types not in mayan
            IList<MetadataType> metadataTypesToAdd = new List<MetadataType>();
            foreach (var metadataTypeLabel in model.MetadataTypes)
            {
                if (metadata.Payload.Results.FirstOrDefault(x => x.Label == metadataTypeLabel) == null)
                {
                    metadataTypesToAdd.Add(new MetadataType() { Label = metadataTypeLabel, Name = metadataTypeLabel.Replace(' ', '_') });
                }
            }

            // Get the metadata types on mayan but not on the model
            IList<MetadataType> metadataTypesToDelete = new List<MetadataType>();
            foreach (var metadataType in metadata.Payload.Results)
            {
                if (model.MetadataTypes.IndexOf(metadataType.Label) < 0)
                {
                    metadataTypesToDelete.Add(metadataType);
                }
            }

            if (model.RemoveLingeringMetadataTypes)
            {
                //remove the ones that do not exist on the metadata
            }

            IList<Task<ExternalResult<MetadataType>>> createTasks = new List<Task<ExternalResult<MetadataType>>>();
            foreach (var metadataType in metadataTypesToAdd)
            {
                createTasks.Add(metadataRepository.CreateMetadataTypeAsync(metadataType));
            }

            Task.WaitAll(createTasks.ToArray());

            return true;
        }

        public bool SyncDocumentTypes(SyncModel model)
        {
            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = documentRepository.GetDocumentTypesAsync(pageSize: 5000);
            documentTypeTask.Wait();

            ExternalResult<QueryResult<DocumentType>> documentTypesRetrieved = documentTypeTask.Result;

            // Get document types not in mayan
            IList<DocumentType> documentTypesToAdd = new List<DocumentType>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                if (documentTypesRetrieved.Payload.Results.FirstOrDefault(x => x.Label == documentTypeModel.Label) == null)
                {
                    documentTypesToAdd.Add(new DocumentType() { Label = documentTypeModel.Label });
                }
            }

            // Get the document types on mayan but not on the model
            IList<DocumentType> documentTypesToDelete = new List<DocumentType>();
            foreach (var documentType in documentTypesRetrieved.Payload.Results)
            {
                if (model.DocumentTypes.FirstOrDefault(x => x.Label == documentType.Label) == null)
                {
                    documentTypesToDelete.Add(documentType);
                }
            }

            if (model.RemoveLingeringDocumentTypes)
            {
                //remove the ones that do not exist on the metadata
            }

            IList<Task<ExternalResult<DocumentType>>> createTasks = new List<Task<ExternalResult<DocumentType>>>();
            foreach (var documentType in documentTypesToAdd)
            {
                createTasks.Add(documentRepository.CreateDocumentTypeAsync(documentType));
            }

            Task.WaitAll(createTasks.ToArray());

            SyncDocumentTypeMetadataTypes(model);

            return true;
        }

        /*private void SyncDocumentTypeMetadataTypes(SyncModel model)
        {
            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = documentRepository.GetDocumentTypesAsync(pageSize: 5000);
            Task<ExternalResult<QueryResult<MetadataType>>> metadataTypesTask = metadataRepository.GetMetadataTypesAsync(pageSize: 5000);
            documentTypeTask.Wait();
            metadataTypesTask.Wait();

            var retrievedMetadataTypes = metadataTypesTask.Result;

            // Check that the metadata types on the documents are the same as the ones in the specified in the model
            IList<DocumentTypeMetadataType> typesToBeLinked = new List<DocumentTypeMetadataType>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                var documentType = documentTypeTask.Result.Payload.Results.FirstOrDefault(x => x.Label == documentTypeModel.Label);
                SyncSingleDocumentTypeMetadataTypes(documentTypeModel, retrievedMetadataTypes.Payload.Results, )
                Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> documentTypeMetadataTypesTask = documentRepository.GetDocumentTypeMetadataTypesAsync(documentType.Id, pageSize: 5000);
                documentTypeMetadataTypesTask.Wait();

                // Get the metadata types not on the document
                var retrivedstuff = documentTypeMetadataTypesTask.Result.Payload.Results;
                foreach (var metadataTypeModel in documentTypeModel.MetadataTypes)
                {
                    if (retrivedstuff.FirstOrDefault(x => x.MetadataType.Label == metadataTypeModel.Label) == null)
                    {
                        MetadataType metadataType = retrievedMetadataTypes.Payload.Results.FirstOrDefault(x => x.Label == metadataTypeModel.Label);
                        typesToBeLinked.Add(new DocumentTypeMetadataType()
                        {
                            DocumentTypeId = documentType.Id,
                            MetadataType = metadataType,
                            Required = metadataTypeModel.Required,
                        });
                    }
                }
            }

            IList<Task<ExternalResult<DocumentTypeMetadataType>>> linkTasks = new List<Task<ExternalResult<DocumentTypeMetadataType>>>();
            foreach (var docmetaType in typesToBeLinked)
            {
                linkTasks.Add(documentRepository.LinkDocumentTypeMetadataTypeAsync(docmetaType.DocumentTypeId, docmetaType.MetadataType.Id, docmetaType.Required));
            }
            Task.WaitAll(linkTasks.ToArray());
        }*/

        private void SyncDocumentTypeMetadataTypes(SyncModel model)
        {
            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = documentRepository.GetDocumentTypesAsync(pageSize: 5000);
            Task<ExternalResult<QueryResult<MetadataType>>> metadataTypesTask = metadataRepository.GetMetadataTypesAsync(pageSize: 5000);
            documentTypeTask.Wait();
            metadataTypesTask.Wait();

            var retrievedMetadataTypes = metadataTypesTask.Result;

            // Check that the metadata types on the documents are the same as the ones in the specified in the model
            IList<Task<bool>> linkTasks = new List<Task<bool>>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                linkTasks.Add(AsyncLinkSingleDocument(documentTypeModel, documentTypeTask.Result.Payload.Results, retrievedMetadataTypes.Payload.Results));
            }

            Task.WaitAll(linkTasks.ToArray());
        }

        private async Task<bool> AsyncLinkSingleDocument(DocumentTypeModel documentTypeModel, IList<DocumentType> retrievedDocumentTypes, IList<MetadataType> retrievedMetadataTypes)
        {
            var documentType = retrievedDocumentTypes.FirstOrDefault(x => x.Label == documentTypeModel.Label);
            ExternalResult<QueryResult<DocumentTypeMetadataType>> documentTypeMetadataTypes = await documentRepository.GetDocumentTypeMetadataTypesAsync(documentType.Id, pageSize: 5000);

            // Get the metadata types not on the document
            IList<Task<ExternalResult<DocumentTypeMetadataType>>> linkTasks = new List<Task<ExternalResult<DocumentTypeMetadataType>>>();
            var retrievedLinks = documentTypeMetadataTypes.Payload.Results;
            foreach (var metadataTypeModel in documentTypeModel.MetadataTypes)
            {
                if (retrievedLinks.FirstOrDefault(x => x.MetadataType.Label == metadataTypeModel.Label) == null)
                {
                    MetadataType metadataType = retrievedMetadataTypes.FirstOrDefault(x => x.Label == metadataTypeModel.Label);
                    linkTasks.Add(documentRepository.LinkDocumentTypeMetadataTypeAsync(documentType.Id, metadataType.Id, metadataTypeModel.Required));
                }
            }
            Task.WaitAll(linkTasks.ToArray());

            return true;
        }
    }
}
