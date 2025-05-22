using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class ManagementActivityService : IManagementActivityService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IManagementActivityRepository _managementActivityRepository;

        public ManagementActivityService(ClaimsPrincipal user, ILogger<ManagementActivityService> logger, IManagementActivityRepository managementActivityRepository)
        {
            _user = user;
            _logger = logger;
            _managementActivityRepository = managementActivityRepository;
        }

        public Paged<PimsPropertyActivity> GetPage(ManagementActivityFilter filter)
        {
            _logger.LogInformation("Searching for management files...");
            _logger.LogDebug("Management file search with filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.ManagementView);

            return _managementActivityRepository.GetPageDeep(filter);
        }
    }
}
