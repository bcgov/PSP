using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class H120CategoryService : IH120CategoryService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IH120CategoryRepository _h120CategoryRepository;

        public H120CategoryService(ClaimsPrincipal user, ILogger<CompensationRequisitionService> logger, IH120CategoryRepository h120CategoryRepository)
        {
            _user = user;
            _logger = logger;
            _h120CategoryRepository = h120CategoryRepository;
        }

        public IEnumerable<PimsH120Category> GetAll()
        {
            _logger.LogInformation($"Getting all H120 categories");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _h120CategoryRepository.GetAll();
        }
    }
}
