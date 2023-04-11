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
        private readonly IAcquisitionFileFormRepository _acquisitionFileFormRepository;

        public FormDocumentService(
            ClaimsPrincipal user,
            ILogger<FormDocumentService> logger,
            IMapper mapper,
            IFormTypeRepository formTypeRepository,
            IDocumentService documentService,
            IDocumentRepository documentRepository,
            IAcquisitionFileFormRepository acquisitionFileFormRepository)
            : base(user, logger)
        {
            _logger = logger;
            _mapper = mapper;
            _formTypeRepository = formTypeRepository;
            _documentService = documentService;
            _documentRepository = documentRepository;
            _acquisitionFileFormRepository = acquisitionFileFormRepository;
        }

        public IList<PimsFormType> GetAllFormDocumentTypes()
        {
            _logger.LogInformation("Getting all form document types");
            this.User.ThrowIfNotAuthorized(Permissions.FormView);

            var formTemplates = _formTypeRepository.GetAllFormTypes();
            return formTemplates;
        }

        public IList<PimsFormType> GetFormDocumentTypes(string formTypeCode)
        {
            this.Logger.LogInformation("Getting document types for code {formTypeCode}", formTypeCode);
            this.User.ThrowIfNotAuthorized(Permissions.FormView);

            return new List<PimsFormType>() { _formTypeRepository.GetByFormTypeCode(formTypeCode) };
        }

        public async Task<DocumentUploadRelationshipResponse> UploadFormDocumentTemplateAsync(string formTypeCode, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading template for document form type");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdmin);

            var currentFormType = _formTypeRepository.GetByFormTypeCode(formTypeCode);
            if (currentFormType.DocumentId != null)
            {
                var result = await DeleteFormDocumentTemplateAsync(currentFormType);
                if (result.Status != ExternalResultStatus.Success)
                {
                    throw new InvalidOperationException("Could not remove existing template");
                }
                else
                {
                    // Retrieve the form type again to get the latest after deletion.
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

        public async Task<ExternalResult<string>> DeleteFormDocumentTemplateAsync(PimsFormType formType)
        {
            this.Logger.LogInformation("Deleting form document template");
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

        public PimsAcquisitionFileForm AddAcquisitionForm(PimsFormType formType, long acquisitionFileId)
        {
            _logger.LogInformation("Adding acquisition form ...");
            this.User.ThrowIfNotAuthorized(Permissions.FormAdd);

            var createdFileForm = _acquisitionFileFormRepository.Add(new PimsAcquisitionFileForm() { AcquisitionFileId = acquisitionFileId, FormTypeCode = formType.FormTypeCode });
            _acquisitionFileFormRepository.CommitTransaction();
            return createdFileForm;
        }

        public IEnumerable<PimsAcquisitionFileForm> GetAcquisitionForms(long acquisitionFileId)
        {
            _logger.LogInformation("Getting acquisition forms by acquisition file id ...", acquisitionFileId);
            this.User.ThrowIfNotAuthorized(Permissions.FormView, Permissions.AcquisitionFileView);

            var fileForms = _acquisitionFileFormRepository.GetAllByAcquisitionFileId(acquisitionFileId);
            return fileForms;
        }

        public PimsAcquisitionFileForm GetAcquisitionForm(long fileFormId)
        {
            _logger.LogInformation("Getting acquisition form by form file id ...", fileFormId);
            this.User.ThrowIfNotAuthorized(Permissions.FormView, Permissions.AcquisitionFileView);

            var fileForm = _acquisitionFileFormRepository.GetByAcquisitionFileFormId(fileFormId);
            return fileForm;
        }

        public bool DeleteAcquisitionFileForm(long fileFormId)
        {
            _logger.LogInformation("Deleting acquisition file form id ...", fileFormId);
            this.User.ThrowIfNotAuthorized(Permissions.FormDelete, Permissions.AcquisitionFileEdit);

            var fileFormToDelete = _acquisitionFileFormRepository.TryDelete(fileFormId);
            _acquisitionFileFormRepository.CommitTransaction();
            return fileFormToDelete;
        }
    }
}
