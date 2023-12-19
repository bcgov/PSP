using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Api.Concepts.CodeTypes;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Concepts.Document.Upload;
using Pims.Api.Models.Concepts.Http;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentFileService implementation provides document managing capabilities for files.
    /// </summary>
    public class DocumentFileService : BaseService, IDocumentFileService
    {
        private readonly IAcquisitionFileDocumentRepository acquisitionFileDocumentRepository;
        private readonly IResearchFileDocumentRepository researchFileDocumentRepository;
        private readonly IDocumentService documentService;
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILeaseRepository _leaseRepository;
        private readonly IPropertyActivityDocumentRepository _propertyActivityDocumentRepository;
        private readonly IDispositionFileDocumentRepository _dispositionFileDocumentRepository;
        private readonly IMapper mapper;

        public DocumentFileService(
            ClaimsPrincipal user,
            ILogger<DocumentFileService> logger,
            IAcquisitionFileDocumentRepository acquisitionFileDocumentRepository,
            IResearchFileDocumentRepository researchFileDocumentRepository,
            IDocumentService documentService,
            IMapper mapper,
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            ILeaseRepository leaseRepository,
            IPropertyActivityDocumentRepository propertyActivityDocumentRepository,
            IDispositionFileDocumentRepository dispositionFileDocumentRepository)
            : base(user, logger)
        {
            this.acquisitionFileDocumentRepository = acquisitionFileDocumentRepository;
            this.researchFileDocumentRepository = researchFileDocumentRepository;
            this.documentService = documentService;
            this.mapper = mapper;
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _leaseRepository = leaseRepository;
            _propertyActivityDocumentRepository = propertyActivityDocumentRepository;
            _dispositionFileDocumentRepository = dispositionFileDocumentRepository;
        }

        public IList<T> GetFileDocuments<T>(FileType fileType, long fileId)
            where T : PimsFileDocument
        {
            Logger.LogInformation("Retrieving PIMS documents related to the file of type $fileType", fileType);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            switch (fileType)
            {
                case FileType.Research:
                    this.User.ThrowIfNotAuthorized(Permissions.ResearchFileView);
                    return researchFileDocumentRepository.GetAllByResearchFile(fileId).Select(f => f as T).ToArray();
                case FileType.Acquisition:
                    this.User.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
                    return acquisitionFileDocumentRepository.GetAllByAcquisitionFile(fileId).Select(f => f as T).ToArray();
                case FileType.Project:
                    this.User.ThrowIfNotAuthorized(Permissions.ProjectView);
                    return _projectRepository.GetAllProjectDocuments(fileId).Select(f => f as T).ToArray();
                case FileType.Lease:
                    this.User.ThrowIfNotAuthorized(Permissions.LeaseView);
                    return _leaseRepository.GetAllLeaseDocuments(fileId).Select(f => f as T).ToArray();
                case FileType.Management:
                    this.User.ThrowIfNotAuthorized(Permissions.ManagementView);
                    return _propertyActivityDocumentRepository.GetAllByPropertyActivity(fileId).Select(f => f as T).ToArray();
                case FileType.Disposition:
                    this.User.ThrowIfNotAuthorized(Permissions.DispositionView);
                    return _dispositionFileDocumentRepository.GetAllByDispositionFile(fileId).Select(f => f as T).ToArray();
                default:
                    throw new BadRequestException("FileT type not valid to get documents.");
            }
        }

        public async Task<DocumentUploadRelationshipResponse> UploadResearchDocumentAsync(long researchFileId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single research file");
            this.User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ResearchFileEdit);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new DocumentUploadRelationshipResponse()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.Document.Id != 0)
            {
                // Create the pims document research file relationship
                PimsResearchFileDocument newResearchFileDocument = new PimsResearchFileDocument()
                {
                    ResearchFileId = researchFileId,
                    DocumentId = uploadResult.Document.Id,
                };
                newResearchFileDocument = researchFileDocumentRepository.AddResearch(newResearchFileDocument);
                researchFileDocumentRepository.CommitTransaction();

                relationshipResponse.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newResearchFileDocument);
            }

            return relationshipResponse;
        }

        public async Task<DocumentUploadRelationshipResponse> UploadAcquisitionDocumentAsync(long acquisitionFileId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single acquisition file");
            this.User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new DocumentUploadRelationshipResponse()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.Document.Id != 0)
            {
                // Create the pims document acquisition file relationship
                PimsAcquisitionFileDocument newAcquisitionDocument = new PimsAcquisitionFileDocument()
                {
                    AcquisitionFileId = acquisitionFileId,
                    DocumentId = uploadResult.Document.Id,
                };
                newAcquisitionDocument = acquisitionFileDocumentRepository.AddAcquisition(newAcquisitionDocument);
                acquisitionFileDocumentRepository.CommitTransaction();

                relationshipResponse.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newAcquisitionDocument);
            }

            return relationshipResponse;
        }

        public async Task<DocumentUploadRelationshipResponse> UploadProjectDocumentAsync(long projectId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single Project");
            this.User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ProjectEdit);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.Document.Id != 0)
            {
                PimsProjectDocument newProjectDocument = new()
                {
                    ProjectId = projectId,
                    DocumentId = uploadResult.Document.Id,
                };
                newProjectDocument = _projectRepository.AddProjectDocument(newProjectDocument);
                _projectRepository.CommitTransaction();

                relationshipResponse.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newProjectDocument);
            }

            return relationshipResponse;
        }

        public async Task<DocumentUploadRelationshipResponse> UploadLeaseDocumentAsync(long leaseId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single Lease");
            this.User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.LeaseEdit);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.Document.Id != 0)
            {
                PimsLeaseDocument newDocument = new()
                {
                    LeaseId = leaseId,
                    DocumentId = uploadResult.Document.Id,
                };
                newDocument = _leaseRepository.AddLeaseDocument(newDocument);
                _leaseRepository.CommitTransaction();

                relationshipResponse.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newDocument);
            }

            return relationshipResponse;
        }

        public async Task<DocumentUploadRelationshipResponse> UploadPropertyActivityDocumentAsync(long propertyActivityId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single Property Activity");
            this.User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ManagementEdit);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.Document.Id != 0)
            {
                PimsPropertyActivityDocument newDocument = new()
                {
                    PimsPropertyActivityId = propertyActivityId,
                    DocumentId = uploadResult.Document.Id,
                };
                newDocument = _propertyActivityDocumentRepository.AddPropertyActivityDocument(newDocument);
                _propertyActivityDocumentRepository.CommitTransaction();

                relationshipResponse.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newDocument);
            }

            return relationshipResponse;
        }

        public async Task<DocumentUploadRelationshipResponse> UploadDispositionDocumentAsync(long dispositionFileId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single disposition file");
            this.User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.DispositionEdit);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.Document.Id != 0)
            {
                PimsDispositionFileDocument newDocument = new()
                {
                    DispositionFileId = dispositionFileId,
                    DocumentId = uploadResult.Document.Id,
                };
                newDocument = _dispositionFileDocumentRepository.AddDispositionDocument(newDocument);
                _dispositionFileDocumentRepository.CommitTransaction();

                relationshipResponse.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newDocument);
            }

            return relationshipResponse;
        }

        public async Task<ExternalResult<string>> DeleteResearchDocumentAsync(PimsResearchFileDocument researchFileDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single research file");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(researchFileDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(researchFileDocument.Document);
            }
            else
            {
                researchFileDocumentRepository.DeleteResearch(researchFileDocument);
                researchFileDocumentRepository.CommitTransaction();
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }

        public async Task<ExternalResult<string>> DeleteProjectDocumentAsync(PimsProjectDocument projectDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single Project");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(projectDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(projectDocument.Document);
            }
            else
            {
                _projectRepository.DeleteProjectDocument(projectDocument.ProjectDocumentId);
                _projectRepository.CommitTransaction();
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }

        public async Task<ExternalResult<string>> DeleteAcquisitionDocumentAsync(PimsAcquisitionFileDocument acquisitionFileDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single acquisition file");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(acquisitionFileDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(acquisitionFileDocument.Document);
            }
            else
            {
                acquisitionFileDocumentRepository.DeleteAcquisition(acquisitionFileDocument);
                acquisitionFileDocumentRepository.CommitTransaction();
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }

        public async Task<ExternalResult<string>> DeleteLeaseDocumentAsync(PimsLeaseDocument leaseDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single lease");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(leaseDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(leaseDocument.Document);
            }
            else
            {
                _leaseRepository.DeleteLeaseDocument(leaseDocument.LeaseDocumentId);
                _leaseRepository.CommitTransaction();
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }

        public async Task<ExternalResult<string>> DeletePropertyActivityDocumentAsync(PimsPropertyActivityDocument propertyActivityDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single Property Activity");
            this.User.ThrowIfNotAuthorized(Permissions.ManagementDelete);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(propertyActivityDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(propertyActivityDocument.Document);
            }
            else
            {
                _propertyActivityDocumentRepository.DeletePropertyActivityDocument(propertyActivityDocument);
                _propertyActivityDocumentRepository.CommitTransaction();
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }

        public async Task<ExternalResult<string>> DeleteDispositionDocumentAsync(PimsDispositionFileDocument dispositionFileDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single disposition file");
            this.User.ThrowIfNotAuthorized(Permissions.DispositionDelete);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(dispositionFileDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(dispositionFileDocument.Document);
            }
            else
            {
                _dispositionFileDocumentRepository.DeleteDispositionDocument(dispositionFileDocument);
                _dispositionFileDocumentRepository.CommitTransaction();
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }
    }
}
