using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
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
        private readonly IMapper mapper;

        public DocumentFileService(
            ClaimsPrincipal user,
            ILogger<DocumentFileService> logger,
            IAcquisitionFileDocumentRepository acquisitionFileDocumentRepository,
            IResearchFileDocumentRepository researchFileDocumentRepository,
            IDocumentService documentService,
            IMapper mapper)
            : base(user, logger)
        {
            this.acquisitionFileDocumentRepository = acquisitionFileDocumentRepository;
            this.researchFileDocumentRepository = researchFileDocumentRepository;
            this.documentService = documentService;
            this.mapper = mapper;
        }

        public IList<T> GetFileDocuments<T>(FileType fileType, long fileId)
            where T : PimsFileDocument
        {
            this.Logger.LogInformation("Retrieving PIMS documents related to the file of type $fileType", fileType);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            switch (fileType)
            {
                case FileType.Research:
                    this.User.ThrowIfNotAuthorized(Permissions.ResearchFileView);
                    return researchFileDocumentRepository.GetAllByResearchFile(fileId).Select(f => f as T).ToArray();
                case FileType.Acquisition:
                    this.User.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
                    return acquisitionFileDocumentRepository.GetAllByAcquisitionFile(fileId).Select(f => f as T).ToArray();
                default:
                    throw new BadRequestException("FileT type not valid to get documents.");
            }
        }

        public async Task<DocumentUploadRelationshipResponse> UploadResearchDocumentAsync(long researchFileId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single research file");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd, Permissions.ResearchFileAdd);

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
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);

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

        public async Task<ExternalResult<string>> DeleteResearchDocumentAsync(PimsResearchFileDocument researchFileDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single research file");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            IList<PimsResearchFileDocument> existingResearchFileDocuments = researchFileDocumentRepository.GetAllByDocument(researchFileDocument.DocumentId);
            if (existingResearchFileDocuments.Count == 1)
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

        public async Task<ExternalResult<string>> DeleteAcquisitionDocumentAsync(PimsAcquisitionFileDocument acquisitionFileDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single acquisition file");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            IList<PimsAcquisitionFileDocument> existingAcquisitionFileDocuments = acquisitionFileDocumentRepository.GetAllByDocument(acquisitionFileDocument.DocumentId);
            if (existingAcquisitionFileDocuments.Count == 1)
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
    }
}
