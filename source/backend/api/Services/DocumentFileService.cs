using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentFileService implementation provides document managing capabilities for files.
    /// </summary>
    public class DocumentFileService : BaseService, IDocumentFileService
    {
        private readonly IDocumentService _documentService;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentQueueRepository _documentQueueRepository;
        private readonly IDocumentRelationshipRepository<PimsAcquisitionFileDocument> _acquisitionFileDocumentRepository;
        private readonly IDocumentRelationshipRepository<PimsResearchFileDocument> _researchFileDocumentRepository;
        private readonly IDocumentRelationshipRepository<PimsMgmtActivityDocument> _managementActivityDocumentRepository;
        private readonly IDocumentRelationshipRepository<PimsLeaseDocument> _leaseFileDocumentRepository;
        private readonly IDocumentRelationshipRepository<PimsProjectDocument> _projectDocumentRepository;
        private readonly IDocumentRelationshipRepository<PimsDispositionFileDocument> _dispositionFileDocumentRepository;
        private readonly IDocumentRelationshipRepository<PimsManagementFileDocument> _managementFileDocumentRepository;
        private readonly IDocumentRelationshipRepository<PimsPropertyDocument> _propertyDocumentRepository;

        public DocumentFileService(
            ClaimsPrincipal user,
            ILogger<DocumentFileService> logger,
            IDocumentRepository documentRepository,
            IDocumentService documentService,
            IDocumentRelationshipRepository<PimsAcquisitionFileDocument> acquisitionFileDocumentRepository,
            IDocumentRelationshipRepository<PimsResearchFileDocument> researchFileDocumentRepository,
            IDocumentRelationshipRepository<PimsLeaseDocument> leaseFileDocumentRepository,
            IDocumentRelationshipRepository<PimsProjectDocument> projectDocumentRepository,
            IDocumentRelationshipRepository<PimsMgmtActivityDocument> managementActivityDocumentRepository,
            IDocumentRelationshipRepository<PimsDispositionFileDocument> dispositionFileDocumentRepository,
            IDocumentRelationshipRepository<PimsManagementFileDocument> managementFileDocumentRepository,
            IDocumentRelationshipRepository<PimsPropertyDocument> propertyDocumentRepository,
            IDocumentQueueRepository documentQueueRepository)
            : base(user, logger)
        {
            _documentService = documentService;
            _documentRepository = documentRepository;
            _acquisitionFileDocumentRepository = acquisitionFileDocumentRepository;
            _researchFileDocumentRepository = researchFileDocumentRepository;
            _leaseFileDocumentRepository = leaseFileDocumentRepository;
            _projectDocumentRepository = projectDocumentRepository;
            _managementActivityDocumentRepository = managementActivityDocumentRepository;
            _dispositionFileDocumentRepository = dispositionFileDocumentRepository;
            _managementFileDocumentRepository = managementFileDocumentRepository;
            _propertyDocumentRepository = propertyDocumentRepository;
            _documentQueueRepository = documentQueueRepository;
        }

        public IList<T> GetFileDocuments<T>(FileType fileType, long fileId)
            where T : PimsFileDocument
        {
            Logger.LogInformation("Retrieving PIMS documents related to the file of type {FileType}", fileType);
            User.ThrowIfNotAuthorized(Permissions.DocumentView);

            switch (fileType)
            {
                case FileType.Research:
                    User.ThrowIfNotAuthorized(Permissions.ResearchFileView);
                    return _researchFileDocumentRepository.GetAllByParentId(fileId).Select(f => f as T).ToArray();
                case FileType.Acquisition:
                    User.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
                    return _acquisitionFileDocumentRepository.GetAllByParentId(fileId).Select(f => f as T).ToArray();
                case FileType.Project:
                    User.ThrowIfNotAuthorized(Permissions.ProjectView);
                    return _projectDocumentRepository.GetAllByParentId(fileId).Select(f => f as T).ToArray();
                case FileType.Lease:
                    User.ThrowIfNotAuthorized(Permissions.LeaseView);
                    return _leaseFileDocumentRepository.GetAllByParentId(fileId).Select(f => f as T).ToArray();
                case FileType.ManagementActivity:
                    User.ThrowIfNotAuthorized(Permissions.ManagementView);
                    return _managementActivityDocumentRepository.GetAllByParentId(fileId).Select(f => f as T).ToArray();
                case FileType.ManagementFile:
                    User.ThrowIfNotAuthorized(Permissions.ManagementView);
                    return _managementFileDocumentRepository.GetAllByParentId(fileId).Select(f => f as T).ToArray();
                case FileType.Disposition:
                    User.ThrowIfNotAuthorized(Permissions.DispositionView);
                    return _dispositionFileDocumentRepository.GetAllByParentId(fileId).Select(f => f as T).ToArray();
                case FileType.Property:
                    User.ThrowIfNotAuthorized(Permissions.PropertyView);
                    return _propertyDocumentRepository.GetAllByParentId(fileId).Select(f => f as T).ToArray();
                default:
                    throw new BadRequestException("File type not valid to get documents.");
            }
        }

        public async Task UploadAcquisitionDocument(long acquisitionFileId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single acquisition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            await UploadDocument(acquisitionFileId, uploadRequest, _acquisitionFileDocumentRepository);
        }

        public async Task UploadResearchDocument(long researchFileId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single research file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            await UploadDocument(researchFileId, uploadRequest, _researchFileDocumentRepository);
        }

        public async Task UploadProjectDocument(long projectId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single Project");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ProjectEdit);
            await UploadDocument(projectId, uploadRequest, _projectDocumentRepository);
        }

        public async Task UploadLeaseDocument(long leaseId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single Lease");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.LeaseEdit);
            await UploadDocument(leaseId, uploadRequest, _leaseFileDocumentRepository);
        }

        public async Task UploadManagementActivityDocument(long managementActivityId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single Management Activity");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ManagementEdit);
            await UploadDocument(managementActivityId, uploadRequest, _managementActivityDocumentRepository);
        }

        public async Task UploadManagementFileDocument(long managementFileId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single management file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ManagementEdit);
            await UploadDocument(managementFileId, uploadRequest, _managementFileDocumentRepository);
        }

        public async Task UploadDispositionDocument(long dispositionFileId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single disposition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.DispositionEdit);
            await UploadDocument(dispositionFileId, uploadRequest, _dispositionFileDocumentRepository);
        }

        public async Task UploadPropertyDocument(long propertyId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single property file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.PropertyEdit);
            await UploadDocument(propertyId, uploadRequest, _propertyDocumentRepository);
        }

        public async Task<ExternalResponse<string>> DeleteResearchDocumentAsync(PimsResearchFileDocument researchFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single research file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ResearchFileEdit);
            return await DeletePropertyDocumentAsync(researchFileDocument, _researchFileDocumentRepository);
        }

        public async Task<ExternalResponse<string>> DeleteProjectDocumentAsync(PimsProjectDocument projectDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single Project");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ProjectEdit);
            return await DeletePropertyDocumentAsync(projectDocument, _projectDocumentRepository);
        }

        public async Task<ExternalResponse<string>> DeleteAcquisitionDocumentAsync(PimsAcquisitionFileDocument acquisitionFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single acquisition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.AcquisitionFileEdit);
            return await DeletePropertyDocumentAsync(acquisitionFileDocument, _acquisitionFileDocumentRepository);
        }

        public async Task<ExternalResponse<string>> DeleteLeaseDocumentAsync(PimsLeaseDocument leaseDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single lease");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.LeaseEdit);
            return await DeletePropertyDocumentAsync(leaseDocument, _leaseFileDocumentRepository);
        }

        public async Task<ExternalResponse<string>> DeleteManagementActivityDocumentAsync(PimsMgmtActivityDocument managementActivityDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single Management Activity");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ManagementEdit);
            return await DeletePropertyDocumentAsync(managementActivityDocument, _managementActivityDocumentRepository);
        }

        public async Task<ExternalResponse<string>> DeleteManagementFileDocumentAsync(PimsManagementFileDocument managementFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single management file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ManagementEdit);
            return await DeletePropertyDocumentAsync(managementFileDocument, _managementFileDocumentRepository);
        }

        public async Task<ExternalResponse<string>> DeleteDispositionDocumentAsync(PimsDispositionFileDocument dispositionFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single disposition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.DispositionEdit);
            return await DeletePropertyDocumentAsync(dispositionFileDocument, _dispositionFileDocumentRepository);
        }

        public async Task<ExternalResponse<string>> DeletePropertyDocumentAsync(PimsPropertyDocument propertyDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single property file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.PropertyEdit);
            return await DeletePropertyDocumentAsync(propertyDocument, _propertyDocumentRepository);
        }

        private static void ValidateDocumentUpload(DocumentUploadRequest uploadRequest)
        {
            uploadRequest.ThrowInvalidFileSize();

            if (!DocumentService.IsValidDocumentExtension(uploadRequest.File.FileName))
            {
                throw new BusinessRuleViolationException("This file has an invalid file extension.");
            }
        }

        private async Task UploadDocument<T>(long parentId, DocumentUploadRequest uploadRequest, IDocumentRelationshipRepository<T> documentRelationshipRepository)
            where T : PimsFileDocument, new()
        {
            ValidateDocumentUpload(uploadRequest);

            PimsDocument pimsDocument = CreatePimsDocument(uploadRequest);
            _documentQueueRepository.SaveChanges();

            T newFileDocument = new()
            {
                FileId = parentId,
                InternalDocumentId = pimsDocument.DocumentId,
            };
            documentRelationshipRepository.AddDocument(newFileDocument);

            await GenerateQueuedDocumentItem(pimsDocument.DocumentId, uploadRequest);
            _documentQueueRepository.CommitTransaction();

            return;
        }

        private PimsDocument CreatePimsDocument(DocumentUploadRequest uploadRequest, string documentExternalId = null)
        {
            // Create the pims document
            PimsDocument newPimsDocument = new()
            {
                FileName = uploadRequest.File.FileName,
                DocumentTypeId = uploadRequest.DocumentTypeId,
                DocumentStatusTypeCode = uploadRequest.DocumentStatusCode,
                MayanId = null,
                DocumentExternalId = documentExternalId,
            };

            _documentRepository.Add(newPimsDocument);

            return newPimsDocument;
        }

        private async Task GenerateQueuedDocumentItem(long documentId, DocumentUploadRequest uploadRequest)
        {
            PimsDocumentQueue queueDocument = new()
            {
                DocumentId = documentId,
                Document = await uploadRequest.File.GetBytes(),
                DocumentMetadata = uploadRequest.DocumentMetadata != null ? JsonSerializer.Serialize(uploadRequest.DocumentMetadata) : null,
            };
            _documentQueueRepository.Add(queueDocument);
        }

        private async Task<ExternalResponse<string>> DeletePropertyDocumentAsync<T>(T propertyDocument, IDocumentRelationshipRepository<T> documentRelationshipRepository)
            where T : PimsFileDocument
        {
            var result = new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            PimsDocument currentDocument = _documentRepository.Find(propertyDocument.InternalDocumentId);

            if (currentDocument.MayanId.HasValue && currentDocument.MayanId.Value > 0)
            {
                result = await DeleteMayanDocument((long)currentDocument.MayanId);
                currentDocument = RemoveDocumentMayanID(currentDocument);
                _documentRepository.CommitTransaction(); // leave trace when mayan document deleted.
            }

            using var transaction = _documentRepository.BeginTransaction();

            DeleteQueuedDocumentItem(currentDocument.DocumentId);

            documentRelationshipRepository.DeleteDocument(propertyDocument);
            _documentRepository.SaveChanges();

            DeletePimsDocument(currentDocument);
            await transaction.CommitAsync();

            return result;
        }

        private async Task<ExternalResponse<string>> DeleteMayanDocument(long mayanDocumentId)
        {
            var result = await _documentService.DeleteMayanStorageDocumentAsync(mayanDocumentId);
            if (result.HttpStatusCode == HttpStatusCode.NotFound)
            {
                result.Status = ExternalResponseStatus.Success;
            }

            return result;
        }

        private PimsDocument RemoveDocumentMayanID(PimsDocument doc)
        {
            doc.MayanId = null;
            return _documentRepository.Update(doc, false);
        }

        private void DeleteQueuedDocumentItem(long documentId)
        {
            var documentQueuedItem = _documentQueueRepository.GetByDocumentId(documentId);
            if (documentQueuedItem == null)
            {
                Logger.LogWarning("Document Queue item not found for document {DocumentId}", documentId);
                return;
            }
            if (documentQueuedItem.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.PENDING.ToString()
                || documentQueuedItem.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.PROCESSING.ToString())
            {
                throw new BadRequestException("Document in process can not be deleted");
            }

            bool deleted = _documentQueueRepository.Delete(documentQueuedItem);
            if (!deleted)
            {
                Logger.LogWarning("Failed to delete Queued Document {DocumentId}", documentId);
                throw new InvalidOperationException("Could not delete document queue item");
            }
        }

        private void DeletePimsDocument(PimsDocument document)
        {
            bool deleted = _documentRepository.DeleteDocument(document);
            if (!deleted)
            {
                Logger.LogWarning("Failed to delete Document {DocumentId}", document.DocumentId);
                throw new InvalidOperationException("Could not delete document");
            }
        }
    }
}
