using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using static Pims.Dal.Entities.PimsLeaseStatusType;

namespace Pims.Api.Services
{
    public class LeaseReportsService : ILeaseReportsService
    {
        private readonly ILeaseRepository _leaseRepository;
        private readonly IUserRepository _userRepository;
        private readonly ClaimsPrincipal _user;

        public LeaseReportsService(ILeaseRepository leaseRepository, IUserRepository userRepository, ClaimsPrincipal user)
        {
            _leaseRepository = leaseRepository;
            _userRepository = userRepository;
            _user = user;
        }

        public IEnumerable<PimsLease> GetAggregatedLeaseReport(int fiscalYearStart)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            DateTime fiscalYearStartDate = fiscalYearStart.ToFiscalYearDate();
            var user = _userRepository.GetByKeycloakUserId(this._user.GetUserKey());

            // fiscal defined as April 01 to March 31 of following year
            return _leaseRepository.GetAllByFilter(
                new Dal.Entities.Models.LeaseFilter()
                {
                    ExpiryAfterDate = fiscalYearStartDate,
                    StartBeforeDate = fiscalYearStartDate.AddYears(1).AddDays(-1),
                    NotInStatus = new List<string>() { PimsLeaseStatusTypes.DRAFT, PimsLeaseStatusTypes.DISCARD },
                    IsReceivable = true,
                }, user.PimsRegionUsers.Select(u => u.RegionCode).ToHashSet(),
                true);
        }
    }
}
