using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class FormDocumentService : BaseService, IFormDocumentService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IFormTypeRepository _formTypeRepository;
        private readonly IDocumentService _documentService;
        private readonly IDocumentRepository _documentRepository;

        public FormDocumentService(
            ClaimsPrincipal user,
            ILogger<FormDocumentService> logger,
            IMapper mapper,
            IFormTypeRepository formTypeRepository,
            IDocumentService documentService,
            IDocumentRepository documentRepository)
            : base(user, logger)
        {
            _logger = logger;
            _mapper = mapper;
            _formTypeRepository = formTypeRepository;
            _documentService = documentService;
            _documentRepository = documentRepository;
        }

        public IList<PimsFormType> GetAllFormDocumentTypes()
        {
            _logger.LogInformation("Getting form document types");
            //this.User.ThrowIfNotAuthorized(Permissions.FormDocumentView);

            var formTemplates = _formTypeRepository.GetAllFormTypes();
            return formTemplates;
        }

        public IList<PimsFormType> GetFormDocuments(string formTypeCode)
        {
            this.Logger.LogInformation("Retrieving PIMS document for single activity template");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            return new List<PimsFormType>() { _formTypeRepository.GetByFormTypeCode(formTypeCode) };
        }

        public async Task<DocumentUploadRelationshipResponse> UploadFormDocumentAsync(string formTypeCode, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for document form type");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdmin);

            var currentFormType = _formTypeRepository.GetByFormTypeCode(formTypeCode);
            if (currentFormType.DocumentId != null)
            {
                var result = await DeleteFormDocumentAsync(currentFormType);
                if (result.Status != ExternalResultStatus.Success)
                {
                    throw new InvalidOperationException("Could not remove existing document");
                }
                else
                {
                    currentFormType = _formTypeRepository.GetByFormTypeCode(formTypeCode);
                }
            }

            DocumentUploadResponse uploadResult = await _documentService.UploadDocumentAsync(uploadRequest);

            DocumentUploadRelationshipResponse relationshipResponse = new DocumentUploadRelationshipResponse()
            {
                UploadResponse = uploadResult,
            };

            if (uploadResult.DocumentExternalResult.Status == ExternalResultStatus.Success && uploadResult.Document != null && uploadResult.Document.Id != 0)
            {
                currentFormType.DocumentId = uploadResult.Document.Id;
                var updatedFormType = _formTypeRepository.SetFormTypeDocument(currentFormType);
                _formTypeRepository.CommitTransaction();


                relationshipResponse.DocumentRelationship = _mapper.Map<DocumentRelationshipModel>(updatedFormType);
            }

            return relationshipResponse;
        }


        public async Task<ExternalResult<string>> DeleteFormDocumentAsync(PimsFormType formType)
        {
            this.Logger.LogInformation("Deleting PIMS document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdmin);

            long? documentId = formType.DocumentId;
            if (documentId != null)
            {
                int currentCount = _documentRepository.DocumentRelationshipCount((long)documentId);
                if (currentCount == 1)
                {
                    return await _documentService.DeleteDocumentAsync(formType.Document);
                }
                else
                {
                    formType.DocumentId = null;
                    _formTypeRepository.SetFormTypeDocument(formType);
                    _formTypeRepository.CommitTransaction();
                    return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
                }
            }
            else
            {
                return new ExternalResult<string>() { Status = ExternalResultStatus.NotExecuted };
            }
        }
    }
}
