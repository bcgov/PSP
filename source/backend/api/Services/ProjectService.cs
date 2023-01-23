using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly ILogger _logger;
        private readonly IProjectRepository _projectRepository;
        private readonly ClaimsPrincipal _user;

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
            _user = user;
        }

        public IList<PimsProject> SearchProjects(string filter, int maxResult)
        {
            _logger.LogInformation("Getting all projects");
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            var projects = _projectRepository.SearchProjects(filter, maxResult);

            return projects;
        }

        public Task<Paged<PimsProject>> GetPage(ProjectFilter filter)
        {
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            _logger.LogInformation("Searching for projects ...");
            _logger.LogDebug("Project search with filter", filter);

            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            return GetPageAsync(filter);
        }

        public Task<PimsProject> Add(PimsProject project)
        {
            _user.ThrowIfNotAuthorized(Permissions.ProjectAdd);

            _logger.LogInformation("Adding new project...");
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            return AddInternalAsync(project);
        }

        private async Task<PimsProject> AddInternalAsync(PimsProject project)
        {
            return await _projectRepository.Add(project);
        }

        private async Task<Paged<PimsProject>> GetPageAsync(ProjectFilter filter)
        {
            return await _projectRepository.GetPageAsync(filter);
        }
    }
}
