using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Mayan.PimsSync;
using Pims.Api.Models.Mayan.Sync;
using Pims.Api.Repositories.Mayan;
using Pims.Core.Http.Configuration;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

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

            Task<ExternalResult<QueryResult<MetadataType>>> retrieveTask = mayanMetadataRepository.TryGetMetadataTypesAsync(pageSize: 5000);
            retrieveTask.Wait();
            var metadata = retrieveTask.Result;

            // Add the metadata types not in mayan
            IList<Task<ExternalResult<MetadataType>>> createTasks = new List<Task<ExternalResult<MetadataType>>>();
            IList<Task<ExternalResult<MetadataType>>> updateTasks = new List<Task<ExternalResult<MetadataType>>>();
            foreach (var metadataTypeLabel in model.MetadataTypes)
            {
                var matchingMetadataResult = metadata.Payload.Results.FirstOrDefault(x => x.Name == metadataTypeLabel.Name);
                if (matchingMetadataResult == null)
                {
                    var newMetadataType = new MetadataType() { Label = metadataTypeLabel.Label, Name = metadataTypeLabel.Name };
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
                IList<Task<ExternalResult<string>>> deleteTasks = new List<Task<ExternalResult<string>>>();
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

            Task<ExternalResult<QueryResult<MetadataType>>> retrieveTask = mayanMetadataRepository.TryGetMetadataTypesAsync(pageSize: 5000);
            retrieveTask.Wait();
            var metadata = retrieveTask.Result;

            // Add the metadata types not in mayan
            IList<Task<ExternalResult<MetadataType>>> updateTasks = new List<Task<ExternalResult<MetadataType>>>();
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
                    var newPimsDocType = new PimsDocumentTyp()
                    {
                        DocumentType = documentTypeModel.Name,
                        DocumentTypeDescription = documentTypeModel.Label,
                        DisplayOrder = documentTypeModel.DisplayOrder,
                        PimsDocumentCategorySubtypes = subcategories,
                    };
                    createdDocumentTypes.Add(documentTypeRepository.Add(newPimsDocType));
                }
                else
                {
                    var subtypeCodes = matchingDocumentType.PimsDocumentCategorySubtypes.Select(x => x.DocumentCategoryTypeCode).ToList();

                    var needsLabelUpdate = matchingDocumentType.DocumentTypeDescription != documentTypeModel.Label;
                    var needsCategoryUpdate = !(subtypeCodes.All(documentTypeModel.Categories.Contains) && subtypeCodes.Count == documentTypeModel.Categories.Count);
                    var needsOrderUpdate = matchingDocumentType.DisplayOrder != documentTypeModel.DisplayOrder;
                    if (needsLabelUpdate || needsCategoryUpdate || needsOrderUpdate)
                    {
                        matchingDocumentType.DocumentTypeDescription = documentTypeModel.Label;
                        matchingDocumentType.PimsDocumentCategorySubtypes = subcategories;
                        matchingDocumentType.DisplayOrder = documentTypeModel.DisplayOrder;

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

            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = mayanDocumentRepository.TryGetDocumentTypesAsync(pageSize: 5000);
            documentTypeTask.Wait();

            ExternalResult<QueryResult<DocumentType>> mayanDocumentTypes = documentTypeTask.Result;

            IList<PimsDocumentTyp> pimsDocumentTypes = documentTypeRepository.GetAll();

            // Add the document types not in mayan
            IList<AddDocumentToMayanWithNameResponseModel> createMayanDocumentTypeTasks = new List<AddDocumentToMayanWithNameResponseModel>();
            IList<Task<ExternalResult<DocumentType>>> updateTasks = new List<Task<ExternalResult<DocumentType>>>();
            foreach (var pimsDocumentTyp in pimsDocumentTypes)
            {
                var matchingTypeFromMayan = mayanDocumentTypes.Payload.Results.FirstOrDefault(x => x.Id == pimsDocumentTyp.MayanId);
                if (matchingTypeFromMayan == null)
                {
                    createMayanDocumentTypeTasks.Add(new AddDocumentToMayanWithNameResponseModel() { AddDocumentTypeTask = mayanDocumentRepository.TryCreateDocumentTypeAsync(new DocumentType() { Label = pimsDocumentTyp.DocumentTypeDescription }), Name = pimsDocumentTyp.DocumentType });
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
            IList<Task<ExternalResult<string>>> deleteTasks = new List<Task<ExternalResult<string>>>();
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

        private void AddMayanIdToPimsDocumentTypes(IList<AddDocumentToMayanWithNameResponseModel> createMayanDocumentTypeTasks, IList<PimsDocumentTyp> pimsDocumentTypes, ref ExternalBatchResult batchResult)
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
            Task<ExternalResult<QueryResult<DocumentType>>> documentTypeTask = mayanDocumentRepository.TryGetDocumentTypesAsync(pageSize: 5000);
            Task<ExternalResult<QueryResult<MetadataType>>> metadataTypesTask = mayanMetadataRepository.TryGetMetadataTypesAsync(pageSize: 5000);
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

        private async Task<ExternalBatchResult> AsyncLinkDocumentMetadata(DocumentTypeModel documentTypeModel, PimsDocumentTyp pimsDocumentTyp, IList<DocumentType> retrievedDocumentTypes, IList<MetadataType> retrievedMetadataTypes)
        {
            ExternalBatchResult batchResult = new ExternalBatchResult();

            var mayanDocumentType = retrievedDocumentTypes.FirstOrDefault(x => x.Id == pimsDocumentTyp.MayanId);
            if (mayanDocumentType?.Id != null)
            {
                ExternalResult<QueryResult<DocumentTypeMetadataType>> documentTypeMetadataTypes = await mayanDocumentRepository.TryGetDocumentTypeMetadataTypesAsync(mayanDocumentType.Id, pageSize: 5000);

                // Update the document type's metadata types
                IList<Task<ExternalResult<DocumentTypeMetadataType>>> documentTypeMetadataTypeTasks = new List<Task<ExternalResult<DocumentTypeMetadataType>>>();
                var retrievedLinks = documentTypeMetadataTypes.Payload.Results;
                foreach (var metadataTypeModel in documentTypeModel.MetadataTypes)
                {
                    DocumentTypeMetadataType existingDocumentTypeMetadaType = retrievedLinks.FirstOrDefault(x => x.MetadataType.Name == metadataTypeModel.Name);
                    if (existingDocumentTypeMetadaType == null)
                    {
                        MetadataType metadataType = retrievedMetadataTypes.FirstOrDefault(x => x.Name == metadataTypeModel.Name);
                        if (metadataType != null)
                        {
                            documentTypeMetadataTypeTasks.Add(mayanDocumentRepository.TryCreateDocumentTypeMetadataTypeAsync(mayanDocumentType.Id, metadataType.Id, metadataTypeModel.Required));
                        }
                        else
                        {
                            batchResult.LinkedDocumentMetadataTypes.Add(
                                new ExternalResult<DocumentTypeMetadataType>()
                                {
                                    Message = $"Metadata with name [{metadataTypeModel.Name}] does not exist in Mayan",
                                    Status = ExternalResultStatus.Error,
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
                IList<Task<ExternalResult<string>>> deleteTasks = new List<Task<ExternalResult<string>>>();
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
