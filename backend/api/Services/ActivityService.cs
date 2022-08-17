using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IDocumentService _documentService;
        private readonly INoteService _noteService;

        public ActivityService(
            ClaimsPrincipal user,
            ILogger<ActivityService> logger,
            IActivityRepository activityRepository,
            IActivityTemplateRepository activityTemplateRepository,
            INoteService noteService,
            IDocumentService documentService)
            : base(user, logger)
        {
            _logger = logger;
            _activityRepository = activityRepository;
            _activityTemplateRepository = activityTemplateRepository;
            _noteService = noteService;
            _documentService = documentService;
        }

        public PimsActivityInstance GetById(long id)
        {
            _logger.LogInformation("Getting activity {id}", id);
            this.User.ThrowIfNotAuthorized(Permissions.NoteView);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            var activityInstance = _activityRepository.GetById(id);

            return activityInstance;
        }

        public IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId)
        {
            _logger.LogInformation("Getting activities by research id {researchFileId}", researchFileId);
            this.User.ThrowIfNotAuthorized(Permissions.NoteView);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            var activityInstances = _activityRepository.GetAllByResearchFileId(researchFileId);

            return activityInstances;
        }

        public IList<PimsActivityTemplate> GetAllActivityTemplates()
        {
            _logger.LogInformation("Getting activity templates");
            this.User.ThrowIfNotAuthorized(Permissions.NoteView);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            var activtyTemplates = _activityTemplateRepository.GetAllActivityTemplates();
            return activtyTemplates;
        }

        public PimsActivityInstance Add(PimsActivityInstance instance)
        {
            _logger.LogInformation("Adding activity instance ...");
            this.User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            var newActivityInstance = _activityRepository.Add(instance);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }

        public PimsActivityInstance Update(PimsActivityInstance model)
        {
            _logger.LogInformation("Updating activity instance ...", model);
            this.User.ThrowIfNotAuthorized(Permissions.NoteEdit);
            ValidateVersion(model.ActivityInstanceId, model.ConcurrencyControlNumber);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            var newActivityInstance = _activityRepository.Update(model);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }

        public void Delete(long activityId)
        {
            _logger.LogInformation("Deleting activity instance ...", activityId);
            this.User.ThrowIfNotAuthorized(Permissions.NoteDelete);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
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
