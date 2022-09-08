using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;

namespace Pims.Api.Services
{
    public class ActivityService : BaseService, IActivityService
    {
        private readonly ILogger _logger;
        private readonly IActivityRepository _activityRepository;
        private readonly IActivityTemplateRepository _activityTemplateRepository;
        private readonly IResearchFileService _researchFileService;
        private readonly IAcquisitionFileService _acquisitionFileService;
        private readonly IDocumentService _documentService;
        private readonly INoteService _noteService;

        public ActivityService(
            ClaimsPrincipal user,
            ILogger<ActivityService> logger,
            IActivityRepository activityRepository,
            IActivityTemplateRepository activityTemplateRepository,
            IAcquisitionFileService acquisitionFileService,
            IResearchFileService researchFileService,
            INoteService noteService,
            IDocumentService documentService)
            : base(user, logger)
        {
            _logger = logger;
            _activityRepository = activityRepository;
            _activityTemplateRepository = activityTemplateRepository;
            _acquisitionFileService = acquisitionFileService;
            _researchFileService = researchFileService;
            _noteService = noteService;
            _documentService = documentService;
        }

        public PimsActivityInstance GetById(long id)
        {
            _logger.LogInformation("Getting activity {id}", id);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityView);

            var activityInstance = _activityRepository.GetById(id);

            return activityInstance;
        }

        public IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId)
        {
            _logger.LogInformation("Getting activities by research id {researchFileId}", researchFileId);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityView);
            this.User.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            var activityInstances = _activityRepository.GetAllByResearchFileId(researchFileId);

            return activityInstances;
        }

        public IList<PimsActivityInstance> GetAllByAcquisitionFileId(long acquisitionFileId)
        {
            _logger.LogInformation("Getting activities by acquisition id {acquisitionFileId}", acquisitionFileId);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityView);
            this.User.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            var activityInstances = _activityRepository.GetAllByAcquisitionFileId(acquisitionFileId);

            return activityInstances;
        }

        public IList<PimsActivityTemplate> GetAllActivityTemplates()
        {
            _logger.LogInformation("Getting activity templates");
            this.User.ThrowIfNotAuthorized(Permissions.ActivityView);

            var activtyTemplates = _activityTemplateRepository.GetAllActivityTemplates();
            return activtyTemplates;
        }

        public PimsActivityInstance Add(PimsActivityInstance instance)
        {
            _logger.LogInformation("Adding activity instance ...");
            this.User.ThrowIfNotAuthorized(Permissions.ActivityAdd);

            var newActivityInstance = _activityRepository.Add(instance);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }

        public PimsActivityInstance AddResearchActivity(PimsActivityInstance instance, long researchFileId)
        {
            _logger.LogInformation("Adding research activity instance ...");
            instance.PimsResearchActivityInstances.Add(new PimsResearchActivityInstance() { ResearchFileId = researchFileId, ActivityInstance = instance });

            return AddFileActivity(Permissions.ResearchFileEdit, instance);
        }

        public PimsActivityInstance AddAcquisitionActivity(PimsActivityInstance instance, long acquisitionFileId)
        {
            _logger.LogInformation("Adding acquisition activity instance ...");
            instance.PimsAcquisitionActivityInstances.Add(new PimsAcquisitionActivityInstance() { AcquisitionFileId = acquisitionFileId, ActivityInstance = instance });

            return AddFileActivity(Permissions.AcquisitionFileEdit, instance);
        }

        public PimsActivityInstance Update(PimsActivityInstance model)
        {
            _logger.LogInformation("Updating activity instance ...", model);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityEdit);
            ValidateVersion(model.ActivityInstanceId, model.ConcurrencyControlNumber);

            var newActivityInstance = _activityRepository.Update(model);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }

        public void Delete(long activityId)
        {
            _logger.LogInformation("Deleting activity instance ...", activityId);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityDelete);

            var activityDocuments = _documentService.GetActivityDocuments(activityId);
            foreach (PimsActivityInstanceDocument activityInstanceDocument in activityDocuments)
            {
                _documentService.DeleteActivityDocumentAsync(activityInstanceDocument);
            }

            var notes = _noteService.GetNotes(Constants.NoteType.Activity, activityId);
            foreach (PimsNote note in notes)
            {
                _noteService.DeleteNote(Constants.NoteType.Activity, note.Id);
            }

            _activityRepository.Delete(activityId);
            _activityRepository.CommitTransaction();
        }

        private PimsActivityInstance AddFileActivity(Permissions fileClaim, PimsActivityInstance instance)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ActivityAdd);
            this.User.ThrowIfNotAuthorized(fileClaim);

            var newActivityInstance = _activityRepository.Add(instance);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }

        private void ValidateVersion(long activityId, long activityVersion)
        {
            long currentRowVersion = _activityRepository.GetRowVersion(activityId);
            if (currentRowVersion != activityVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this activity, please refresh the application and retry.");
            }
        }
    }
}
