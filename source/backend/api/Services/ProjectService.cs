using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

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

        public IList<PimsProject> SearchProjects(string filter, int maxResult)
        {
            _logger.LogInformation("Getting all projects");
            this.User.ThrowIfNotAuthorized(Permissions.ProjectView);

            var projects = _projectRepository.SearchProjects(filter, maxResult);

            return projects;
        }
    }
}
