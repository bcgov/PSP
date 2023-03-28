using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Lookup;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class FormService : BaseService, IFormService
    {
        private readonly ILogger _logger;
        private readonly IAcquisitionFileFormRepository _acquisitionFileFormRepository;

        public FormService(
            ClaimsPrincipal user,
            ILogger<ActivityService> logger,
            IAcquisitionFileFormRepository acquisitionFileFormRepository)
            : base(user, logger)
        {
            _logger = logger;
            _acquisitionFileFormRepository = acquisitionFileFormRepository;
        }

        public PimsAcquisitionFileForm AddAcquisitionForm(LookupModel<string> formType, long acquisitionFileId)
        {
            _logger.LogInformation("Adding acquisition form {formType} to file {acquisitionFileId} ...", acquisitionFileId, formType);
            this.User.ThrowIfNotAuthorized(Permissions.FormAdd, Permissions.AcquisitionFileEdit);

            var createdFileForm = _acquisitionFileFormRepository.Add(new PimsAcquisitionFileForm() { AcquisitionFileId = acquisitionFileId, FormTypeCode = formType.Id });
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
