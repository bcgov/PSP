using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Core.Api.Exceptions;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
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
            Logger.LogInformation("Retrieving PIMS documents related to the file of type ${fileType}", fileType);
            User.ThrowIfNotAuthorized(Permissions.DocumentView);

            switch (fileType)
            {
                case FileType.Research:
                    User.ThrowIfNotAuthorized(Permissions.ResearchFileView);
                    return researchFileDocumentRepository.GetAllByResearchFile(fileId).Select(f => f as T).ToArray();
                case FileType.Acquisition:
                    User.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
                    return acquisitionFileDocumentRepository.GetAllByAcquisitionFile(fileId).Select(f => f as T).ToArray();
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

        public async Task<DocumentUploadRelationshipResponse> UploadResearchDocumentAsync(long researchFileId, DocumentUploadRequest uploadRequest)
        {
            Logger.LogInformation("Uploading document for single research file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ResearchFileEdit);

            // Do not call Mayan if uploaded file is empty (zero-size)
            ValidateZeroLengthFile(uploadRequest);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new DocumentUploadRelationshipResponse()
            {
                UploadResponse = uploadResult,
            };

            // Throw an error if Mayan returns a null document. This means it wasn't able to store it.
            ValidateDocumentUploadResponse(uploadResult);

            if (uploadResult.Document is not null && uploadResult.Document.Id != 0)
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
            Logger.LogInformation("Uploading document for single acquisition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);

            // Do not call Mayan if uploaded file is empty (zero-size)
            ValidateZeroLengthFile(uploadRequest);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new DocumentUploadRelationshipResponse()
            {
                UploadResponse = uploadResult,
            };

            // Throw an error if Mayan returns a null document. This means it wasn't able to store it.
            ValidateDocumentUploadResponse(uploadResult);

            if (uploadResult.Document is not null && uploadResult.Document.Id != 0)
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
            Logger.LogInformation("Uploading document for single Project");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ProjectEdit);

            // Do not call Mayan if uploaded file is empty (zero-size)
            ValidateZeroLengthFile(uploadRequest);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new()
            {
                UploadResponse = uploadResult,
            };

            // Throw an error if Mayan returns a null document. This means it wasn't able to store it.
            ValidateDocumentUploadResponse(uploadResult);

            if (uploadResult.Document is not null && uploadResult.Document.Id != 0)
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
            Logger.LogInformation("Uploading document for single Lease");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.LeaseEdit);

            // Do not call Mayan if uploaded file is empty (zero-size)
            ValidateZeroLengthFile(uploadRequest);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new()
            {
                UploadResponse = uploadResult,
            };

            // Throw an error if Mayan returns a null document. This means it wasn't able to store it.
            ValidateDocumentUploadResponse(uploadResult);

            if (uploadResult.Document is not null && uploadResult.Document.Id != 0)
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
            Logger.LogInformation("Uploading document for single Property Activity");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.ManagementEdit);

            // Do not call Mayan if uploaded file is empty (zero-size)
            ValidateZeroLengthFile(uploadRequest);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new()
            {
                UploadResponse = uploadResult,
            };

            // Throw an error if Mayan returns a null document. This means it wasn't able to store it.
            ValidateDocumentUploadResponse(uploadResult);

            if (uploadResult.Document is not null && uploadResult.Document.Id != 0)
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
            Logger.LogInformation("Uploading document for single disposition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentAdd, Permissions.DispositionEdit);

            // Do not call Mayan if uploaded file is empty (zero-size)
            ValidateZeroLengthFile(uploadRequest);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new()
            {
                UploadResponse = uploadResult,
            };

            // Throw an error if Mayan returns a null document. This means it wasn't able to store it.
            ValidateDocumentUploadResponse(uploadResult);

            if (uploadResult.Document is not null && uploadResult.Document.Id != 0)
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

        public async Task<ExternalResponse<string>> DeleteResearchDocumentAsync(PimsResearchFileDocument researchFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single research file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ResearchFileEdit);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(researchFileDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(researchFileDocument.Document);
            }
            else
            {
                researchFileDocumentRepository.DeleteResearch(researchFileDocument);
                researchFileDocumentRepository.CommitTransaction();
                return new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            }
        }

        public async Task<ExternalResponse<string>> DeleteProjectDocumentAsync(PimsProjectDocument projectDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single Project");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ProjectEdit);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(projectDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(projectDocument.Document);
            }
            else
            {
                _projectRepository.DeleteProjectDocument(projectDocument.ProjectDocumentId);
                _projectRepository.CommitTransaction();
                return new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            }
        }

        public async Task<ExternalResponse<string>> DeleteAcquisitionDocumentAsync(PimsAcquisitionFileDocument acquisitionFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single acquisition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.AcquisitionFileEdit);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(acquisitionFileDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(acquisitionFileDocument.Document);
            }
            else
            {
                acquisitionFileDocumentRepository.DeleteAcquisition(acquisitionFileDocument);
                acquisitionFileDocumentRepository.CommitTransaction();
                return new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            }
        }

        public async Task<ExternalResponse<string>> DeleteLeaseDocumentAsync(PimsLeaseDocument leaseDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single lease");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.LeaseEdit);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(leaseDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(leaseDocument.Document);
            }
            else
            {
                _leaseRepository.DeleteLeaseDocument(leaseDocument.LeaseDocumentId);
                _leaseRepository.CommitTransaction();
                return new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            }
        }

        public async Task<ExternalResponse<string>> DeletePropertyActivityDocumentAsync(PimsPropertyActivityDocument propertyActivityDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single Property Activity");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.ManagementEdit);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(propertyActivityDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(propertyActivityDocument.Document);
            }
            else
            {
                _propertyActivityDocumentRepository.DeletePropertyActivityDocument(propertyActivityDocument);
                _propertyActivityDocumentRepository.CommitTransaction();
                return new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            }
        }

        public async Task<ExternalResponse<string>> DeleteDispositionDocumentAsync(PimsDispositionFileDocument dispositionFileDocument)
        {
            Logger.LogInformation("Deleting PIMS document for single disposition file");
            User.ThrowIfNotAllAuthorized(Permissions.DocumentDelete, Permissions.DispositionEdit);

            var relationshipCount = _documentRepository.DocumentRelationshipCount(dispositionFileDocument.DocumentId);
            if (relationshipCount == 1)
            {
                return await documentService.DeleteDocumentAsync(dispositionFileDocument.Document);
            }
            else
            {
                _dispositionFileDocumentRepository.DeleteDispositionDocument(dispositionFileDocument);
                _dispositionFileDocumentRepository.CommitTransaction();
                return new ExternalResponse<string>() { Status = ExternalResponseStatus.NotExecuted };
            }
        }

        private static void ValidateZeroLengthFile(DocumentUploadRequest uploadRequest)
        {
            if (uploadRequest.File is not null && uploadRequest.File.Length == 0)
            {
                throw new BadRequestException("The submitted file is empty");
            }
        }

        private static void ValidateDocumentUploadResponse(DocumentUploadResponse uploadResult)
        {
            if (uploadResult.Document is null)
            {
                throw new BadRequestException("Unexpected exception uploading file", new System.Exception(uploadResult.DocumentExternalResponse.Message));
            }
        }
    }
}
