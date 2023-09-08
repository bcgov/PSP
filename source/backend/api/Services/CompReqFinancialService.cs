using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

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

        public IEnumerable<PimsCompReqFinancial> SearchCompensationRequisitionFinancials(AcquisitionReportFilterModel filter)
        {
            _logger.LogInformation("Searching all comp req financials matching the filter: {filter}", filter);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);
            _user.ThrowIfNotAuthorized(Permissions.ProjectView);

            var pimsUser = _userRepository.GetUserInfoByKeycloakUserId(_user.GetUserKey());
            var allMatchingFinancials = _compReqFinancialRepository.SearchCompensationRequisitionFinancials(filter);

            // When a contractor exports data, they should only see data for files they have been assigned to.
            if (pimsUser.IsContractor)
            {
                return allMatchingFinancials.Where(f => f.CompensationRequisition.AcquisitionFile.PimsAcquisitionFilePeople.Any(afp => afp.PersonId == pimsUser.PersonId));
            }

            // The system will only provide data that adheres to the user's "region limited data".
            return allMatchingFinancials.Where(f => pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == f.CompensationRequisition.AcquisitionFile.RegionCode));
        }
    }
}
