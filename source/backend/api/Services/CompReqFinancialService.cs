using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Core.Security;

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
            var allMatchingFinancials = _compReqFinancialRepository.SearchCompensationRequisitionFinancials(filter, includeAcquisitions, includeLeases);

            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());

            // For Leases without region, export them regardless of the users' region. Otherwise filter lease data by the user’s region.
            var leasePredicate = PredicateBuilder.New<PimsCompReqFinancial>(p => false);
            leasePredicate.Or(f => f.CompensationRequisition != null && f.CompensationRequisition.Lease != null && f.CompensationRequisition.Lease.RegionCode == null);
            leasePredicate.Or(f => f.CompensationRequisition != null && f.CompensationRequisition.Lease != null && pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == f.CompensationRequisition.Lease.RegionCode));

            var acquisitionPredicate = PredicateBuilder.New<PimsCompReqFinancial>(p => false);

            // For acquisition file financials - when a contractor exports data, they should only see data for files they have been assigned to.
            if (pimsUser.IsContractor)
            {
                acquisitionPredicate.Or(f => f.CompensationRequisition != null && f.CompensationRequisition.AcquisitionFile != null && f.CompensationRequisition.AcquisitionFile.PimsAcquisitionFileTeams.Any(afp => afp.PersonId == pimsUser.PersonId));
            }
            else
            {
                // user region matches acquisition file region
                acquisitionPredicate.Or(f => f.CompensationRequisition != null && f.CompensationRequisition.AcquisitionFile != null && pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == f.CompensationRequisition.AcquisitionFile.RegionCode));
            }

            // The system will only provide data that adheres to the user's "region limited data".
            var predicate = leasePredicate.Or(acquisitionPredicate).Compile();
            return allMatchingFinancials.Where(predicate);
        }
    }
}
