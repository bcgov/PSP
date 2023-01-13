using System;
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
    public class ProjectService : IProjectService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IProjectRepository _projectRepository;

        public ProjectService(ClaimsPrincipal user, ILogger<ProjectService> logger, IProjectRepository projectRepository)
        {
            _user = user;
            _logger = logger;
            _projectRepository = projectRepository;
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

        private async Task<Paged<PimsProject>> GetPageAsync(ProjectFilter filter)
        {
            return await _projectRepository.GetPageAsync(filter);
        }
    }
}
