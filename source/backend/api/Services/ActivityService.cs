using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class ActivityService : BaseService, IActivityService
    {
        private readonly ILogger _logger;
        private readonly IActivityRepository _activityRepository;
        private readonly IActivityTemplateRepository _activityTemplateRepository;
        private readonly IDocumentActivityService _documentActivityService;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly INoteService _noteService;

        public ActivityService(
            ClaimsPrincipal user,
            ILogger<ActivityService> logger,
            IActivityRepository activityRepository,
            IActivityTemplateRepository activityTemplateRepository,
            IEntityNoteRepository entityNoteRepository,
            INoteService noteService,
            IDocumentActivityService documentActivityService)
            : base(user, logger)
        {
            _logger = logger;
            _activityRepository = activityRepository;
            _activityTemplateRepository = activityTemplateRepository;
            _noteService = noteService;
            _documentActivityService = documentActivityService;
            _entityNoteRepository = entityNoteRepository;
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

        public IList<PimsActivityInstance> GetAllByLeaseId(long leaseId)
        {
            _logger.LogInformation("Getting activities by lease id {leaseId}", leaseId);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityView);
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);

            var activityInstances = _activityRepository.GetAllByLeaseId(leaseId);

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

            var result = _activityRepository.Update(model);
            AddNoteIfActivityStatusChanged(model);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(result.ActivityInstanceId);
        }

        public PimsActivityInstance UpdateActivityResearchProperties(PimsActivityInstance model)
        {
            _logger.LogInformation("Updating activity instance research properties ...", model.PimsActInstPropRsrchFiles);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityEdit);
            this.User.ThrowIfNotAuthorized(Permissions.PropertyEdit);
            ValidateVersion(model.ActivityInstanceId, model.ConcurrencyControlNumber);

            var newActivityInstance = _activityRepository.UpdateActivityResearchProperties(model);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }

        public PimsActivityInstance UpdateActivityAcquisitionProperties(PimsActivityInstance model)
        {
            _logger.LogInformation("Updating activity instance acquisition properties ...", model.PimsActInstPropAcqFiles);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityEdit);
            this.User.ThrowIfNotAuthorized(Permissions.PropertyEdit);
            ValidateVersion(model.ActivityInstanceId, model.ConcurrencyControlNumber);

            var newActivityInstance = _activityRepository.UpdateActivityAcquisitionProperties(model);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }

        public async Task Delete(long activityId)
        {
            _logger.LogInformation("Deleting activity instance ...", activityId);
            this.User.ThrowIfNotAuthorized(Permissions.ActivityDelete);

            var activityDocuments = _documentActivityService.GetActivityDocuments(activityId);
            foreach (PimsActivityInstanceDocument activityInstanceDocument in activityDocuments)
            {
                var response = await _documentActivityService.DeleteActivityDocumentAsync(activityInstanceDocument);
                if (response.Status == ExternalResultStatus.Error)
                {
                    throw new DbUpdateException("Failed to delete one or more related documents, unable to remove activity at this time. If this error persists, contact support.");
                }
            }

            var notes = _noteService.GetNotes(Constants.NoteType.Activity, activityId);
            foreach (PimsNote note in notes)
            {
                _noteService.DeleteNote(Constants.NoteType.Activity, note.Internal_Id, false);
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

        private void AddNoteIfActivityStatusChanged(PimsActivityInstance instance)
        {
            var currentActivity = _activityRepository.GetById(instance.Internal_Id);
            if (currentActivity.ActivityInstanceStatusTypeCode != instance.ActivityInstanceStatusTypeCode)
            {
                PimsActivityInstanceNote note = new PimsActivityInstanceNote()
                {
                    ActivityInstanceId = currentActivity.ActivityInstanceId,
                    AppCreateTimestamp = System.DateTime.Now,
                    AppCreateUserid = instance.AppCreateUserid,
                    Note = new PimsNote()
                    {
                        IsSystemGenerated = true,
                        NoteTxt = $"Activity status changed from {currentActivity.ActivityInstanceStatusTypeCodeNavigation?.Description} to {instance.ActivityInstanceStatusTypeCodeNavigation?.Description}",
                        AppCreateTimestamp = System.DateTime.Now,
                        AppCreateUserid = this.User.GetUsername(),
                    },
                };
                _entityNoteRepository.Add(note);
            }
        }
    }
}
