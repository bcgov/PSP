using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.PimsSync;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Repositories.Mayan;
using Pims.Core.Extensions;
using Pims.Core.Http.Configuration;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentSyncService implementation provides document syncronization between different data sources.
    /// </summary>
    public class DocumentSyncService : BaseService, IDocumentSyncService
    {
        private readonly IEdmsDocumentRepository mayanDocumentRepository;
        private readonly IEdmsMetadataRepository mayanMetadataRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IOptionsMonitor<AuthClientOptions> keycloakOptions;

        public DocumentSyncService(
            ClaimsPrincipal user,
            ILogger<DocumentSyncService> logger,
            IEdmsDocumentRepository edmsDocumentRepository,
            IEdmsMetadataRepository emdMetadataRepository,
            IDocumentTypeRepository documentTypeRepository,
            IOptionsMonitor<AuthClientOptions> options)
            : base(user, logger)
        {
            this.mayanDocumentRepository = edmsDocumentRepository;
            this.mayanMetadataRepository = emdMetadataRepository;
            this.documentTypeRepository = documentTypeRepository;
            this.keycloakOptions = options;
        }

        public ExternalBatchResult SyncMayanMetadataTypes(SyncModel model)
        {
            this.Logger.LogInformation("Synchronizing Pims metadata types");
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.DocumentAdmin, this.keycloakOptions);

            ExternalBatchResult batchResult = new ExternalBatchResult();

            Task<ExternalResponse<QueryResponse<MetadataTypeModel>>> retrieveTask = mayanMetadataRepository.TryGetMetadataTypesAsync(pageSize: 5000);
            retrieveTask.Wait();
            var metadata = retrieveTask.Result;

            // Add the metadata types not in mayan
            IList<Task<ExternalResponse<MetadataTypeModel>>> createTasks = new List<Task<ExternalResponse<MetadataTypeModel>>>();
            IList<Task<ExternalResponse<MetadataTypeModel>>> updateTasks = new List<Task<ExternalResponse<MetadataTypeModel>>>();
            foreach (var metadataTypeLabel in model.MetadataTypes)
            {
                var matchingMetadataResult = metadata.Payload.Results.FirstOrDefault(x => x.Name == metadataTypeLabel.Name);
                if (matchingMetadataResult == null)
                {
                    var newMetadataType = new MetadataTypeModel() { Label = metadataTypeLabel.Label, Name = metadataTypeLabel.Name };
                    createTasks.Add(mayanMetadataRepository.TryCreateMetadataTypeAsync(newMetadataType));
                }
                else if (matchingMetadataResult.Label != metadataTypeLabel.Label)
                {
                    matchingMetadataResult.Label = metadataTypeLabel.Label;
                    updateTasks.Add(mayanMetadataRepository.TryUpdateMetadataTypeAsync(matchingMetadataResult));
                }
            }

            Task.WaitAll(createTasks.Concat(updateTasks).ToArray());
            foreach (var task in createTasks)
            {
                batchResult.CreatedMetadata.Add(task.Result);
            }

            foreach (var task in updateTasks)
            {
                batchResult.UpdatedMetadata.Add(task.Result);
            }

            if (model.RemoveLingeringMetadataTypes)
            {
                // Delete the metadata types that are not on the sync model
                IList<Task<ExternalResponse<string>>> deleteTasks = new List<Task<ExternalResponse<string>>>();
                foreach (var metadataType in metadata.Payload.Results)
                {
                    if (!model.MetadataTypes.Any(m => m.Name == metadataType.Name))
                    {
                        deleteTasks.Add(mayanMetadataRepository.TryDeleteMetadataTypeAsync(metadataType.Id));
                    }
                }

                Task.WaitAll(deleteTasks.ToArray());
                foreach (var task in deleteTasks)
                {
                    batchResult.DeletedMetadata.Add(task.Result);
                }
            }

            return batchResult;
        }

        public ExternalBatchResult MigrateMayanMetadataTypes(SyncModel model)
        {
            this.Logger.LogInformation("Migrating Pims metadata types");
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.DocumentAdmin, this.keycloakOptions);

            ExternalBatchResult batchResult = new ExternalBatchResult();

            Task<ExternalResponse<QueryResponse<MetadataTypeModel>>> retrieveTask = mayanMetadataRepository.TryGetMetadataTypesAsync(pageSize: 5000);
            retrieveTask.Wait();
            var metadata = retrieveTask.Result;

            // Add the metadata types not in mayan
            IList<Task<ExternalResponse<MetadataTypeModel>>> updateTasks = new List<Task<ExternalResponse<MetadataTypeModel>>>();
            foreach (var metadataTypeLabel in model.MetadataTypes)
            {
                var oldMetadataName = metadataTypeLabel.Label.Replace(' ', '_');
                var matchingMetadataResult = metadata.Payload.Results.FirstOrDefault(x => x.Name.Equals(oldMetadataName, StringComparison.CurrentCultureIgnoreCase));
                if (matchingMetadataResult?.Label != null)
                {
                    matchingMetadataResult.Name = metadataTypeLabel.Name;
                    matchingMetadataResult.Label = metadataTypeLabel.Label;
                    updateTasks.Add(mayanMetadataRepository.TryUpdateMetadataTypeAsync(matchingMetadataResult));
                }
            }

            Task.WaitAll(updateTasks.ToArray());
            foreach (var task in updateTasks)
            {
                batchResult.UpdatedMetadata.Add(task.Result);
            }

            return batchResult;
        }

        public DocumentSyncResponse SyncPimsDocumentTypes(SyncModel model)
        {
            this.Logger.LogInformation("Synchronizing PIMS document types");
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.DocumentAdmin, this.keycloakOptions);

            IList<PimsDocumentTyp> pimsDocumentTypes = documentTypeRepository.GetAll();

            // Add the document to the pims db, update any existing values with the same name.
            IList<PimsDocumentTyp> createdDocumentTypes = new List<PimsDocumentTyp>();
            IList<PimsDocumentTyp> updatedDocumentTypes = new List<PimsDocumentTyp>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                var subcategories = documentTypeModel.Categories.Select(
                        x => new PimsDocumentCategorySubtype()
                        {
                            DocumentCategoryTypeCode = x,
                        }).ToList();

                var matchingDocumentType = pimsDocumentTypes.FirstOrDefault(x => x.DocumentType == documentTypeModel.Name);
                if (matchingDocumentType == null)
                {
                    // New documents
                    var newPimsDocType = new PimsDocumentTyp()
                    {
                        DocumentType = documentTypeModel.Name,
                        DocumentTypeDescription = documentTypeModel.Label,
                        DocumentTypeDefinition = documentTypeModel.Purpose,
                        DisplayOrder = documentTypeModel.DisplayOrder,
                        PimsDocumentCategorySubtypes = subcategories,
                    };
                    createdDocumentTypes.Add(documentTypeRepository.Add(newPimsDocType));
                }
                else
                {
                    // Existing documents
                    var subtypeCodes = matchingDocumentType.PimsDocumentCategorySubtypes.Select(x => x.DocumentCategoryTypeCode).ToList();

                    var needsLabelUpdate = matchingDocumentType.DocumentTypeDescription != documentTypeModel.Label;
                    var needsPurposeUpdate = matchingDocumentType.DocumentTypeDefinition != documentTypeModel.Purpose;
                    var needsCategoryUpdate = !(subtypeCodes.All(documentTypeModel.Categories.Contains) && subtypeCodes.Count == documentTypeModel.Categories.Count);
                    var needsOrderUpdate = matchingDocumentType.DisplayOrder != documentTypeModel.DisplayOrder;
                    var needsToBeEnabled = matchingDocumentType.IsDisabled == true;
                    if (needsLabelUpdate || needsPurposeUpdate|| needsCategoryUpdate || needsOrderUpdate || needsToBeEnabled)
                    {
                        matchingDocumentType.DocumentTypeDescription = documentTypeModel.Label;
                        matchingDocumentType.DocumentTypeDefinition = documentTypeModel.Purpose;
                        matchingDocumentType.PimsDocumentCategorySubtypes = subcategories;
                        matchingDocumentType.DisplayOrder = documentTypeModel.DisplayOrder;
                        matchingDocumentType.IsDisabled = false;

                        updatedDocumentTypes.Add(documentTypeRepository.Update(matchingDocumentType));
                    }
                }
            }

            IList<PimsDocumentTyp> deletedDocumentTypes = new List<PimsDocumentTyp>();
            if (model.RemoveLingeringDocumentTypes)
            {
                // Disable the document types that are not on the sync model
                foreach (var documentType in pimsDocumentTypes)
                {
                    if (!model.DocumentTypes.Any(x => x.Name == documentType.DocumentType))
                    {
                        documentType.IsDisabled = true;
                        deletedDocumentTypes.Add(documentTypeRepository.Update(documentType));
                    }
                }
            }
            documentTypeRepository.CommitTransaction();

            return new DocumentSyncResponse() { Added = createdDocumentTypes, Updated = updatedDocumentTypes, Deleted = deletedDocumentTypes };
        }

        public ExternalBatchResult SyncPimsToMayan(SyncModel model)
        {
            this.Logger.LogInformation("Synchronizing Pims DB and Mayan document types (as well as attached metadata)");
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.DocumentAdmin, this.keycloakOptions);

            ExternalBatchResult batchResult = new();

            Task<ExternalResponse<QueryResponse<Models.Mayan.Document.DocumentTypeModel>>> documentTypeTask = mayanDocumentRepository.TryGetDocumentTypesAsync(pageSize: 5000);
            documentTypeTask.Wait();

            ExternalResponse<QueryResponse<Models.Mayan.Document.DocumentTypeModel>> mayanDocumentTypes = documentTypeTask.Result;

            IList<PimsDocumentTyp> pimsDocumentTypes = documentTypeRepository.GetAll();

            // Add the document types not in mayan
            IList<AddDocumentToMayanWithNameTaskWrapper> createMayanDocumentTypeTasks = new List<AddDocumentToMayanWithNameTaskWrapper>();
            IList<Task<ExternalResponse<Models.Mayan.Document.DocumentTypeModel>>> updateTasks = new List<Task<ExternalResponse<Models.Mayan.Document.DocumentTypeModel>>>();
            foreach (var pimsDocumentTyp in pimsDocumentTypes)
            {
                var matchingTypeFromMayan = mayanDocumentTypes.Payload.Results.FirstOrDefault(x => x.Id == pimsDocumentTyp.MayanId);
                if (matchingTypeFromMayan == null)
                {
                    createMayanDocumentTypeTasks.Add(new AddDocumentToMayanWithNameTaskWrapper()
                    {
                        AddDocumentTypeTask = mayanDocumentRepository.TryCreateDocumentTypeAsync(new Models.Mayan.Document.DocumentTypeModel() { Label = pimsDocumentTyp.DocumentTypeDescription }),
                        Name = pimsDocumentTyp.DocumentType,
                    });
                }
                else if (matchingTypeFromMayan.Label != pimsDocumentTyp.DocumentTypeDescription)
                {
                    // if the label has changed, updated the label in mayan.
                    matchingTypeFromMayan.Label = pimsDocumentTyp.DocumentTypeDescription;
                    updateTasks.Add(mayanDocumentRepository.TryUpdateDocumentTypeAsync(matchingTypeFromMayan));
                }
            }
            Task.WaitAll(createMayanDocumentTypeTasks.Select(x => x.AddDocumentTypeTask).Concat(updateTasks).ToArray());
            foreach (var task in updateTasks)
            {
                batchResult.UpdatedDocumentType.Add(task.Result);
            }
            AddMayanIdToPimsDocumentTypes(createMayanDocumentTypeTasks, pimsDocumentTypes, ref batchResult);

            pimsDocumentTypes = documentTypeRepository.GetAll(); // re-retrieve the document list from pims as it may have been updated.
            IList<Task<ExternalResponse<string>>> deleteTasks = new List<Task<ExternalResponse<string>>>();
            if (model.RemoveLingeringDocumentTypes)
            {
                // Delete the document types that are not in the PIMS db
                foreach (var mayanDocumentTypeToRemove in mayanDocumentTypes.Payload.Results)
                {
                    if (pimsDocumentTypes.FirstOrDefault(x => x.MayanId == mayanDocumentTypeToRemove.Id) == null)
                    {
                        deleteTasks.Add(mayanDocumentRepository.TryDeleteDocumentTypeAsync(mayanDocumentTypeToRemove.Id));
                    }
                }
            }

            Task.WaitAll(deleteTasks.ToArray());
            foreach (var task in deleteTasks)
            {
                batchResult.DeletedDocumentType.Add(task.Result);
            }

            SyncDocumentTypeMetadataTypes(model, pimsDocumentTypes, ref batchResult);

            return batchResult;
        }

        private void AddMayanIdToPimsDocumentTypes(IList<AddDocumentToMayanWithNameTaskWrapper> createMayanDocumentTypeTasks, IList<PimsDocumentTyp> pimsDocumentTypes, ref ExternalBatchResult batchResult)
        {
            foreach (var task in createMayanDocumentTypeTasks)
            {
                if (task != null && task.AddDocumentTypeTask.Result.HttpStatusCode == System.Net.HttpStatusCode.Created)
                {
                    var matchingPimsType = pimsDocumentTypes.FirstOrDefault(x => x.DocumentType == task.Name);
                    matchingPimsType.MayanId = task.AddDocumentTypeTask.Result.Payload.Id;
                    documentTypeRepository.Update(matchingPimsType);
                }
                batchResult.CreatedDocumentType.Add(task.AddDocumentTypeTask.Result);
            }
            documentTypeRepository.CommitTransaction();
        }

        private void SyncDocumentTypeMetadataTypes(SyncModel model, IList<PimsDocumentTyp> pimsDocumentTypes, ref ExternalBatchResult batchResult)
        {
            Task<ExternalResponse<QueryResponse<Models.Mayan.Document.DocumentTypeModel>>> documentTypeTask = mayanDocumentRepository.TryGetDocumentTypesAsync(pageSize: 5000);
            Task<ExternalResponse<QueryResponse<MetadataTypeModel>>> metadataTypesTask = mayanMetadataRepository.TryGetMetadataTypesAsync(pageSize: 5000);
            Task.WaitAll(documentTypeTask, metadataTypesTask);

            var retrievedMetadataTypes = metadataTypesTask.Result;

            // Check that the metadata types on the documents are the same as the ones in the specified in the model
            IList<Task<ExternalBatchResult>> linkTasks = new List<Task<ExternalBatchResult>>();
            foreach (var documentTypeModel in model.DocumentTypes)
            {
                var pimsDocumentType = pimsDocumentTypes.FirstOrDefault(d => d.DocumentType == documentTypeModel.Name);
                if (pimsDocumentType != null)
                {
                    linkTasks.Add(AsyncLinkDocumentMetadata(documentTypeModel, pimsDocumentType, documentTypeTask.Result.Payload.Results, retrievedMetadataTypes.Payload.Results));
                }
            }

            Task.WaitAll(linkTasks.ToArray());
            foreach (var task in linkTasks)
            {
                batchResult.DeletedDocumentTypeMetadataType.AddRange(task.Result.DeletedDocumentTypeMetadataType);
                batchResult.LinkedDocumentMetadataTypes.AddRange(task.Result.LinkedDocumentMetadataTypes);
            }
        }

        private async Task<ExternalBatchResult> AsyncLinkDocumentMetadata(Models.PimsSync.DocumentTypeModel documentTypeModel, PimsDocumentTyp pimsDocumentTyp, IList<Models.Mayan.Document.DocumentTypeModel> retrievedDocumentTypes, IList<MetadataTypeModel> retrievedMetadataTypes)
        {
            ExternalBatchResult batchResult = new ExternalBatchResult();

            var mayanDocumentType = retrievedDocumentTypes.FirstOrDefault(x => x.Id == pimsDocumentTyp.MayanId);
            if (mayanDocumentType?.Id != null)
            {
                ExternalResponse<QueryResponse<DocumentTypeMetadataTypeModel>> documentTypeMetadataTypes = await mayanDocumentRepository.TryGetDocumentTypeMetadataTypesAsync(mayanDocumentType.Id, pageSize: 5000);

                // Update the document type's metadata types
                IList<Task<ExternalResponse<DocumentTypeMetadataTypeModel>>> documentTypeMetadataTypeTasks = new List<Task<ExternalResponse<DocumentTypeMetadataTypeModel>>>();
                var retrievedLinks = documentTypeMetadataTypes.Payload.Results;
                foreach (var metadataTypeModel in documentTypeModel.MetadataTypes)
                {
                    DocumentTypeMetadataTypeModel existingDocumentTypeMetadaType = retrievedLinks.FirstOrDefault(x => x.MetadataType.Name == metadataTypeModel.Name);
                    if (existingDocumentTypeMetadaType == null)
                    {
                        MetadataTypeModel metadataType = retrievedMetadataTypes.FirstOrDefault(x => x.Name == metadataTypeModel.Name);
                        if (metadataType != null)
                        {
                            documentTypeMetadataTypeTasks.Add(mayanDocumentRepository.TryCreateDocumentTypeMetadataTypeAsync(mayanDocumentType.Id, metadataType.Id, metadataTypeModel.Required));
                        }
                        else
                        {
                            batchResult.LinkedDocumentMetadataTypes.Add(
                                new ExternalResponse<DocumentTypeMetadataTypeModel>()
                                {
                                    Message = $"Metadata with name [{metadataTypeModel.Name}] does not exist in Mayan",
                                    Status = ExternalResponseStatus.Error,
                                });
                        }
                    }
                    else
                    {
                        if (existingDocumentTypeMetadaType.Required != metadataTypeModel.Required)
                        {
                            documentTypeMetadataTypeTasks.Add(mayanDocumentRepository.TryUpdateDocumentTypeMetadataTypeAsync(mayanDocumentType.Id, existingDocumentTypeMetadaType.Id, metadataTypeModel.Required));
                        }
                    }
                }
                Task.WaitAll(documentTypeMetadataTypeTasks.ToArray());
                foreach (var task in documentTypeMetadataTypeTasks)
                {
                    batchResult.LinkedDocumentMetadataTypes.Add(task.Result);
                }

                // Get metadata types not on sync model
                IList<Task<ExternalResponse<string>>> deleteTasks = new List<Task<ExternalResponse<string>>>();
                foreach (var documentTypeMetadataType in documentTypeMetadataTypes.Payload.Results)
                {
                    if (documentTypeModel.MetadataTypes.FirstOrDefault(x => x.Name == documentTypeMetadataType.MetadataType.Name) == null)
                    {
                        deleteTasks.Add(mayanDocumentRepository.TryDeleteDocumentTypeMetadataTypeAsync(mayanDocumentType.Id, documentTypeMetadataType.Id));
                    }
                }

                Task.WaitAll(deleteTasks.ToArray());
                foreach (var task in deleteTasks)
                {
                    batchResult.DeletedDocumentTypeMetadataType.Add(task.Result);
                }
            }

            return batchResult;
        }
    }
}
