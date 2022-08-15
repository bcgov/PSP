using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Dal.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ILogger _logger;
        private readonly IActivityRepository _activityRepository;
        private readonly IActivityTemplateRepository _activityTemplateRepository;

        public ActivityService(
            ILogger<ActivityService> logger,
            IActivityRepository activityRepository,
            IActivityTemplateRepository activityTemplateRepository,
            IPropertyRepository propertyRepository,
        {
            _logger = logger;
            _activityRepository = activityRepository;
            _activityTemplateRepository = activityTemplateRepository;
        }

        public PimsActivityInstance GetById(long id)
        {
            _logger.LogInformation("Getting activity {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.ActivityView);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            var activityInstance = _activityRepository.GetById(id);

            return activityInstance;
        }

        public IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId)
        {
            _logger.LogInformation("Getting activities by research id {researchFileId}", researchFileId);
            _user.ThrowIfNotAuthorized(Permissions.ActivityView);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            var activityInstances = _activityRepository.GetAllByResearchFileId(researchFileId);

            return activityInstances;
        }

        public IList<PimsActivityTemplate> GetAllActivityTemplates()
        {
            _logger.LogInformation("Getting activity templates");
            _user.ThrowIfNotAuthorized(Permissions.ActivityView);
            var activtyTemplates = _activityTemplateRepository.GetAllActivityTemplates();
            return activtyTemplates;
        }

        public PimsActivityInstance Add(PimsActivityInstance instance)
        {
            _logger.LogInformation("Adding activity instance ...");
            _user.ThrowIfNotAuthorized(Permissions.ActivityAdd);

            // _user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);
            var newActivityInstance = _activityRepository.Add(instance);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }
    }
}
