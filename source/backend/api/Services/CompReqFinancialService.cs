using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class CompReqFinancialService : ICompReqFinancialService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ICompReqFinancialRepository _compReqFinancialRepository;
        private readonly IUserRepository _userRepository;

        public CompReqFinancialService(
            ClaimsPrincipal user,
            ILogger<CompReqFinancialService> logger,
            ICompReqFinancialRepository compReqFinancialRepository,
            IUserRepository userRepository)
        {
            _user = user;
            _logger = logger;
            _compReqFinancialRepository = compReqFinancialRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<PimsCompReqFinancial> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly)
        {
            _logger.LogInformation("Getting pims comp req financials by {acquisitionFileId}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _compReqFinancialRepository.GetAllByAcquisitionFileId(acquisitionFileId, finalOnly);
        }

        public IEnumerable<PimsCompReqFinancial> GetAllByLeaseFileId(long leaseFileId, bool? finalOnly)
        {
            _logger.LogInformation("Getting pims comp req financials by {leaseFileId}", leaseFileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);

            return _compReqFinancialRepository.GetAllByLeaseFileId(leaseFileId, finalOnly);
        }

        public IEnumerable<PimsCompReqFinancial> SearchCompensationRequisitionFinancials(AcquisitionReportFilterModel filter)
        {
            _logger.LogInformation("Searching all comp req financials matching the filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            // The following check enforces that the user has AT LEAST one of the following permissions (or both)
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView, Permissions.LeaseView);

            var includeAcquisitions = _user.HasPermission(Permissions.AcquisitionFileView);
            var includeLeases = _user.HasPermission(Permissions.LeaseView);

            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            long? contractorPersonId = pimsUser.IsContractor ? pimsUser.PersonId : null;
            var userRegions = pimsUser.PimsRegionUsers.Select(ur => ur.RegionCode).ToHashSet();

            var allMatchingFinancials = _compReqFinancialRepository.SearchCompensationRequisitionFinancials(filter, userRegions, contractorPersonId, includeAcquisitions, includeLeases);
            return allMatchingFinancials;
        }
    }
}
