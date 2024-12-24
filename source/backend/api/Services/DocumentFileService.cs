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
        private readonly IAcquisitionFileDocumentRepository _acquisitionFileDocumentRepository;
        private readonly IResearchFileDocumentRepository _researchFileDocumentRepository;
        private readonly IDocumentService _documentService;
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentQueueRepository _documentQueueRepository;
        private readonly ILeaseRepository _leaseRepository;
        private readonly IPropertyActivityDocumentRepository _propertyActivityDocumentRepository;
        private readonly IDispositionFileDocumentRepository _dispositionFileDocumentRepository;

        public DocumentFileService(
            ClaimsPrincipal user,
            ILogger<DocumentFileService> logger,
            IAcquisitionFileDocumentRepository acquisitionFileDocumentRepository,
            IResearchFileDocumentRepository researchFileDocumentRepository,
            IDocumentService documentService,
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            ILeaseRepository leaseRepository,
            IPropertyActivityDocumentRepository propertyActivityDocumentRepository,
            IDispositionFileDocumentRepository dispositionFileDocumentRepository,
            IDocumentQueueRepository documentQueueRepository)
            : base(user, logger)
        {
            _acquisitionFileDocumentRepository = acquisitionFileDocumentRepository;
            _researchFileDocumentRepository = researchFileDocumentRepository;
            _documentService = documentService;
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _leaseRepository = leaseRepository;
            _propertyActivityDocumentRepository = propertyActivityDocumentRepository;
            _dispositionFileDocumentRepository = dispositionFileDocumentRepository;
            _documentQueueRepository = documentQueueRepository;
        }

        public IList<T> GetFileDocuments<T>(FileType fileType, long fileId)
            where T : PimsFileDocument
        {
            Logger.LogInformation("Retrieving PIMS documents related to the file of type ${fileType}", fileType);
            User.ThrowIfNotAuthorized(Permissions.DocumentView);

            switch (fileType)
            {
                case FileType.Research:
                    User.ThrowIfNotAuthorized(Permissions.ResearchFileView);
                    return _researchFileDocumentRepository.GetAllByResearchFile(fileId).Select(f => f as T).ToArray();
                case FileType.Acquisition:
                    User.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
                    return _acquisitionFileDocumentRepository.GetAllByAcquisitionFile(fileId).Select(f => f as T).ToArray();
                case FileType.Project:
                    User.ThrowIfNotAuthorized(Permissions.ProjectView);
                    return _projectRepository.GetAllProjectDocuments(fileId).Select(f => f as T).ToArray();
                case FileType.Lease:
                    User.ThrowIfNotAuthorized(Permissions.LeaseView);
                    return _leaseRepository.GetAllLeaseDocuments(fileId).Select(f => f as T).ToArray();
                case FileType.Management:
                    User.ThrowIfNotAuthorized(Permissions.ManagementView);
                    return _propertyActivityDocumentRepository.GetAllByPropertyActivity(fileId).Select(f => f as T).ToArray();
                case FileType.Disposition:
                    User.ThrowIfNotAuthorized(Permissions.DispositionView);
                    return _dispositionFileDocumentRepository.GetAllByDispositionFile(fileId).Select(f => f as T).ToArray();
                default:
                    throw new BadRequestException("FileT type not valid to get documents.");
            }
        }

        public async Task UploadAcquisitionDocument(long acquisitionFileId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single acquisition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            uploadRequest.ThrowInvalidFileSize();

            PimsDocument pimsDocument = CreatePimsDocument(uploadRequest);
            _documentQueueRepository.SaveChanges();

            PimsAcquisitionFileDocument newAcquisitionDocument = new()
            {
                AcquisitionFileId = acquisitionFileId,
                DocumentId = pimsDocument.DocumentId,
            };
            _acquisitionFileDocumentRepository.AddAcquisition(newAcquisitionDocument);

            await GenerateQueuedDocumentItem(pimsDocument.DocumentId, uploadRequest);
            _acquisitionFileDocumentRepository.CommitTransaction();

            return;
        }

        public async Task UploadResearchDocument(long researchFileId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single research file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            uploadRequest.ThrowInvalidFileSize();

            PimsDocument pimsDocument = CreatePimsDocument(uploadRequest);
            _documentQueueRepository.SaveChanges();

            PimsResearchFileDocument newFileDocument = new()
            {
                ResearchFileId = researchFileId,
                DocumentId = pimsDocument.DocumentId,
            };
            _researchFileDocumentRepository.AddResearch(newFileDocument);

            await GenerateQueuedDocumentItem(pimsDocument.DocumentId, uploadRequest);
            _documentQueueRepository.CommitTransaction();

            return;
        }

        public async Task UploadProjectDocument(long projectId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single Project");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ProjectEdit);
            uploadRequest.ThrowInvalidFileSize();

            PimsDocument pimsDocument = CreatePimsDocument(uploadRequest);
            _documentQueueRepository.SaveChanges();

            PimsProjectDocument newFileDocument = new()
            {
                ProjectId = projectId,
                DocumentId = pimsDocument.DocumentId,
            };
            _projectRepository.AddProjectDocument(newFileDocument);

            await GenerateQueuedDocumentItem(pimsDocument.DocumentId, uploadRequest);
            _documentQueueRepository.CommitTransaction();

            return;
        }

        public async Task UploadLeaseDocument(long leaseId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single Lease");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.LeaseEdit);
            uploadRequest.ThrowInvalidFileSize();

            PimsDocument pimsDocument = CreatePimsDocument(uploadRequest);
            _documentQueueRepository.SaveChanges();

            PimsLeaseDocument newFileDocument = new()
            {
                LeaseId = leaseId,
                DocumentId = pimsDocument.DocumentId,
            };
            _leaseRepository.AddLeaseDocument(newFileDocument);

            await GenerateQueuedDocumentItem(pimsDocument.DocumentId, uploadRequest);
            _documentQueueRepository.CommitTransaction();

            return;
        }

        public async Task UploadPropertyActivityDocument(long propertyActivityId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single Property Activity");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ManagementEdit);
            uploadRequest.ThrowInvalidFileSize();

            PimsDocument pimsDocument = CreatePimsDocument(uploadRequest);
            _documentQueueRepository.SaveChanges();

            PimsPropertyActivityDocument newFileDocument = new()
            {
                PimsPropertyActivityId = propertyActivityId,
                DocumentId = pimsDocument.DocumentId,
            };
            _propertyActivityDocumentRepository.AddPropertyActivityDocument(newFileDocument);

            await GenerateQueuedDocumentItem(pimsDocument.DocumentId, uploadRequest);
            _documentQueueRepository.CommitTransaction();

            return;
        }

        public async Task UploadDispositionDocument(long dispositionFileId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single disposition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.DispositionEdit);
            uploadRequest.ThrowInvalidFileSize();

            PimsDocument pimsDocument = CreatePimsDocument(uploadRequest);
            _documentQueueRepository.SaveChanges();

            PimsDispositionFileDocument newFileDocument = new()
            {
                DispositionFileId = dispositionFileId,
                DocumentId = pimsDocument.DocumentId,
            };
            _dispositionFileDocumentRepository.AddDispositionDocument(newFileDocument);

            await GenerateQueuedDocumentItem(pimsDocument.DocumentId, uploadRequest);
            _documentQueueRepository.CommitTransaction();

            return;
        }

        public async Task<ExternalResponse<string>> DeleteResearchDocumentAsync(PimsResearchFileDocument researchFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single research file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ResearchFileEdit);

            var result = new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            PimsDocument currentDocument = _documentRepository.Find(researchFileDocument.Document.DocumentId);

            if (currentDocument.MayanId.HasValue && currentDocument.MayanId.Value > 0)
            {
                result = await DeleteMayanDocument((long)currentDocument.MayanId);
                currentDocument = RemoveDocumentMayanID(currentDocument);
                _documentRepository.CommitTransaction();
            }

            using var transaction = _documentRepository.BeginTransaction();

            DeleteQueuedDocumentItem(currentDocument.DocumentId);
            _researchFileDocumentRepository.DeleteResearch(researchFileDocument);
            DeleteDocument(currentDocument);

            _documentRepository.SaveChanges();
            transaction.Commit();

            return result;
        }

        public async Task<ExternalResponse<string>> DeleteProjectDocumentAsync(PimsProjectDocument projectDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single Project");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ProjectEdit);

            var result = new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            PimsDocument currentDocument = _documentRepository.Find(projectDocument.Document.DocumentId);

            if (currentDocument.MayanId.HasValue && currentDocument.MayanId.Value > 0)
            {
                result = await DeleteMayanDocument((long)currentDocument.MayanId);
                currentDocument = RemoveDocumentMayanID(currentDocument);
                _documentRepository.CommitTransaction();
            }

            using var transaction = _documentRepository.BeginTransaction();

            DeleteQueuedDocumentItem(currentDocument.DocumentId);
            _projectRepository.DeleteProjectDocument(projectDocument.ProjectDocumentId);
            DeleteDocument(currentDocument);

            _documentRepository.SaveChanges();
            transaction.Commit();

            return result;
        }

        public async Task<ExternalResponse<string>> DeleteAcquisitionDocumentAsync(PimsAcquisitionFileDocument acquisitionFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single acquisition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.AcquisitionFileEdit);

            var result = new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            PimsDocument currentDocument = _documentRepository.Find(acquisitionFileDocument.Document.DocumentId);

            if (currentDocument.MayanId.HasValue && currentDocument.MayanId.Value > 0)
            {
                result = await DeleteMayanDocument((long)acquisitionFileDocument.Document.MayanId);
                currentDocument = RemoveDocumentMayanID(currentDocument);
                _documentRepository.CommitTransaction();
            }

            using var transaction = _documentRepository.BeginTransaction();

            DeleteQueuedDocumentItem(acquisitionFileDocument.DocumentId);

            _acquisitionFileDocumentRepository.DeleteAcquisition(acquisitionFileDocument);

            DeleteDocument(currentDocument);

            _documentRepository.SaveChanges();
            transaction.Commit();

            return result;
        }

        public async Task<ExternalResponse<string>> DeleteLeaseDocumentAsync(PimsLeaseDocument leaseDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single lease");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.LeaseEdit);

            var result = new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            PimsDocument currentDocument = _documentRepository.Find(leaseDocument.Document.DocumentId);

            // 1 - Delete Mayan first.
            if (currentDocument.MayanId.HasValue && currentDocument.MayanId.Value > 0)
            {
                result = await DeleteMayanDocument((long)currentDocument.MayanId);
                currentDocument = RemoveDocumentMayanID(currentDocument);
                _documentRepository.CommitTransaction();
            }

            using var transaction = _documentRepository.BeginTransaction();

            DeleteQueuedDocumentItem(currentDocument.DocumentId);
            _leaseRepository.DeleteLeaseDocument(leaseDocument.LeaseDocumentId);

            DeleteDocument(currentDocument);

            _documentRepository.SaveChanges();
            transaction.Commit();

            return result;
        }

        public async Task<ExternalResponse<string>> DeletePropertyActivityDocumentAsync(PimsPropertyActivityDocument propertyActivityDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single Property Activity");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ManagementEdit);

            var result = new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            PimsDocument currentDocument = _documentRepository.Find(propertyActivityDocument.Document.DocumentId);

            if (currentDocument.MayanId.HasValue && currentDocument.MayanId.Value > 0)
            {
                result = await DeleteMayanDocument((long)currentDocument.MayanId);
                currentDocument = RemoveDocumentMayanID(currentDocument);
                _documentRepository.CommitTransaction();
            }

            using var transaction = _documentRepository.BeginTransaction();

            DeleteQueuedDocumentItem(currentDocument.DocumentId);

            _propertyActivityDocumentRepository.DeletePropertyActivityDocument(propertyActivityDocument);
            DeleteDocument(currentDocument);

            _documentRepository.SaveChanges();
            transaction.Commit();

            return result;
        }

        public async Task<ExternalResponse<string>> DeleteDispositionDocumentAsync(PimsDispositionFileDocument dispositionFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single disposition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.DispositionEdit);

            var result = new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            PimsDocument currentDocument = _documentRepository.Find(dispositionFileDocument.DocumentId);

            if (currentDocument.MayanId.HasValue && currentDocument.MayanId.Value > 0)
            {
                result = await DeleteMayanDocument((long)currentDocument.MayanId);
                currentDocument = RemoveDocumentMayanID(currentDocument);
                _documentRepository.CommitTransaction(); // leave trace when mayan document deleted.
            }

            using var transaction = _documentRepository.BeginTransaction();

            DeleteQueuedDocumentItem(currentDocument.DocumentId);

            _dispositionFileDocumentRepository.DeleteDispositionDocument(dispositionFileDocument);

            DeleteDocument(currentDocument);

            _documentRepository.SaveChanges();
            transaction.Commit();

            return result;
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
            if (documentQueuedItem.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.PENDING.ToString()
                || documentQueuedItem.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.PROCESSING.ToString())
            {
                throw new BadRequestException("Doucment in process can not be deleted");
            }

            bool deleted = _documentQueueRepository.Delete(documentQueuedItem);
            if(!deleted)
            {
                Logger.LogWarning("Failed to delete Queued Document {documentId}", documentId);
                throw new InvalidOperationException("Could not delete document queue item");
            }
        }

        private void DeleteDocument(PimsDocument document)
        {
            bool deleted = _documentRepository.DeleteDocument(document);
            if (!deleted)
            {
                Logger.LogWarning("Failed to delete Document {documentId}", document.DocumentId);
                throw new InvalidOperationException("Could not delete document");
            }
        }
    }
}
