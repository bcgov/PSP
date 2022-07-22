using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IActivityRepository _activityRepository;
        private readonly IActivityTemplateRepository _activityTemplateRepository;



        public ActivityService(
            ClaimsPrincipal user,
            ILogger<ActivityService> logger,
            IActivityRepository activityRepository,
             IActivityTemplateRepository activityTemplateRepository,
        IPropertyRepository propertyRepository,
            ICoordinateTransformService coordinateService)
        {
            _user = user;
            _logger = logger;
            _activityRepository = activityRepository;
            _activityTemplateRepository = activityTemplateRepository;
        }

        public PimsActivityInstance GetById(long id)
        {
            _logger.LogInformation("Getting activity with id {id}", id);
            //_user.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            var activityInstance = _activityRepository.GetById(id);

            return activityInstance;
        }

       public IList<PimsActivityInstance> GetAllByResearchFileId(long researchFileId)
        {
            _logger.LogInformation("Getting activities by research id {researchFileId}", researchFileId);
            //_user.ThrowIfNotAuthorized(Permissions.ResearchFileView);

            var activityInstances = _activityRepository.GetAllByResearchFileId(researchFileId);

            return activityInstances;
        }

        public IList<PimsActivityTemplate> GetAllActivityTemplates()
        {
            _logger.LogInformation("Getting activity templates");
            //_user.ThrowIfNotAuthorized(Permissions.ResearchFileView);
            var activtyTemplates = _activityTemplateRepository.GetAllActivityTemplates();
            return activtyTemplates;
        }

        public PimsActivityInstance Add(PimsActivityInstance instance)
        {
            _logger.LogInformation("Adding activity instance ...");
            //_user.ThrowIfNotAuthorized(Permissions.ResearchFileAdd);

        

            var newActivityInstance = _activityRepository.Add(instance);
            _activityRepository.CommitTransaction();
            return _activityRepository.GetById(newActivityInstance.ActivityInstanceId);
        }
    }
}
