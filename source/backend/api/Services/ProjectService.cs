using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly ILogger _logger;
        private readonly IProjectRepository _projectRepository;


        /// <summary>
        /// Creates a new instance of a ProjectService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="projectRepository"></param>
        public ProjectService(
            ClaimsPrincipal user,
            ILogger<ProjectService> logger,
            IProjectRepository projectRepository)
            : base(user, logger)
        {
            _logger = logger;
            _projectRepository = projectRepository;
        }

        public IList<PimsProject> GetProjectPredictions(string filter, int maxResult)
        {
            _logger.LogInformation("Getting all projects");
            /*this.User.ThrowIfNotAuthorized(Permissions.ActivityView);
            this.User.ThrowIfNotAuthorized(Permissions.ResearchFileView);*/

            var projects = _projectRepository.GetProjectPrediction(filter, maxResult);

            return projects;
        }
    }
}
