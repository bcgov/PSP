using System.Collections.Generic;
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
    /// DocumentActivityService implementation provides document managing capabilities for activities.
    /// </summary>
    public class DocumentActivityService : BaseService, IDocumentActivityService
    {
        private readonly IDocumentActivityRepository documentActivityRepository;
        private readonly IDocumentActivityTemplateRepository documentActivityTemplateRepository;
        private readonly IDocumentService documentService;
        private readonly IMapper mapper;

        public DocumentActivityService(
            ClaimsPrincipal user,
            ILogger<DocumentActivityService> logger,
            IDocumentActivityRepository documentActivityRepository,
            IAcquisitionFileDocumentRepository documentFileRepository,
            IDocumentActivityTemplateRepository documentActivityTemplateRepository,
            IDocumentService documentService,
            IMapper mapper)
            : base(user, logger)
        {
            this.documentActivityRepository = documentActivityRepository;
            this.documentActivityTemplateRepository = documentActivityTemplateRepository;
            this.documentService = documentService;
            this.mapper = mapper;
        }

        public IList<PimsActivityInstanceDocument> GetFileActivityDocuments(FileType fileType, long fileId)
        {
            this.Logger.LogInformation("Retrieving PIMS document for activities related to the file of type $fileType", fileType);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            switch (fileType)
            {
                case FileType.Research:
                    return documentActivityRepository.GetAllByResearchFileActivities(fileId);
                case FileType.Acquisition:
                    return documentActivityRepository.GetAllByAcquisitionFileActivities(fileId);
                default:
                    throw new BadRequestException("FileT type not valid to get documents.");
            }
        }

        public IList<PimsActivityInstanceDocument> GetActivityDocuments(long activityId)
        {
            this.Logger.LogInformation("Retrieving PIMS document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            return documentActivityRepository.GetAllByActivity(activityId);
        }

        public IList<PimsActivityTemplateDocument> GetActivityTemplateDocuments(long activityTemplateId)
        {
            this.Logger.LogInformation("Retrieving PIMS document for single activity template");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            return documentActivityTemplateRepository.GetAllByActivityTemplate(activityTemplateId);
        }

        public async Task<DocumentUploadRelationshipResponse> UploadActivityDocumentAsync(long activityId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new DocumentUploadRelationshipResponse()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.Document.Id != 0)
            {
                // Create the pims document activity relationship
                PimsActivityInstanceDocument newActivityDocument = new PimsActivityInstanceDocument()
                {
                    ActivityInstanceId = activityId,
                    DocumentId = uploadResult.Document.Id,
                };
                newActivityDocument = documentActivityRepository.Add(newActivityDocument);
                documentActivityRepository.CommitTransaction();

                relationshipResponse.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newActivityDocument);
            }

            return relationshipResponse;
        }

        public async Task<DocumentUploadRelationshipResponse> UploadActivityTemplateDocumentAsync(long activityTemplateId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdmin);

            DocumentUploadResponse uploadResult = await documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new DocumentUploadRelationshipResponse()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.DocumentExternalResult.Status == ExternalResultStatus.Success && uploadResult.Document != null && uploadResult.Document.Id != 0)
            {
                // Create the pims document activity relationship
                PimsActivityTemplateDocument newActivityTemplateDocument = new PimsActivityTemplateDocument()
                {
                    ActivityTemplateId = activityTemplateId,
                    DocumentId = uploadResult.Document.Id,
                };
                newActivityTemplateDocument = documentActivityTemplateRepository.Add(newActivityTemplateDocument);
                documentActivityRepository.CommitTransaction();

                relationshipResponse.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newActivityTemplateDocument);
            }

            return relationshipResponse;
        }

        public async Task<ExternalResult<string>> DeleteActivityDocumentAsync(PimsActivityInstanceDocument activityDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            IList<PimsActivityInstanceDocument> existingActivityDocuments = documentActivityRepository.GetAllByDocument(activityDocument.DocumentId);
            if (existingActivityDocuments.Count == 1)
            {
                return await documentService.DeleteDocumentAsync(activityDocument.Document);
            }
            else
            {
                documentActivityRepository.Delete(activityDocument);
                documentActivityRepository.CommitTransaction();
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }

        public async Task<ExternalResult<string>> DeleteActivityTemplateDocumentAsync(PimsActivityTemplateDocument templateDocument)
        {
            this.Logger.LogInformation("Deleting PIMS document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdmin);

            IList<PimsActivityTemplateDocument> existingActivityTemplateDocuments = documentActivityTemplateRepository.GetAllByDocument(templateDocument.DocumentId);
            if (existingActivityTemplateDocuments.Count == 1)
            {
                return await documentService.DeleteDocumentAsync(templateDocument.Document);
            }
            else
            {
                documentActivityTemplateRepository.Delete(templateDocument);
                documentActivityTemplateRepository.CommitTransaction();
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }
    }
}
