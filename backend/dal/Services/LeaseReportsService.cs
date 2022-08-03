using Pims.Dal.Repositories;
using Pims.Dal.Entities;
using System.Collections.Generic;
using System;
using Pims.Core.Extensions;
using Pims.Dal.Helpers.Extensions;
using System.Security.Claims;
using Pims.Dal.Security;
using static Pims.Dal.Entities.PimsLeaseStatusType;

namespace Pims.Dal.Services
{
    public class LeaseReportsService : ILeaseReportsService
    {
        private readonly ILeaseRepository _leaseRepository;
        private readonly ClaimsPrincipal _user;

        public LeaseReportsService(ILeaseRepository leaseRepository, ClaimsPrincipal user)
        {
            _leaseRepository = leaseRepository;
            _user = user;
        }

        public IEnumerable<PimsLease> GetAggregatedLeaseReport(int fiscalYearStart)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            DateTime fiscalYearStartDate = fiscalYearStart.ToFiscalYearDate();
            //fiscal defined as April 01 to March 31 of following year
            return _leaseRepository.Get(new Entities.Models.LeaseFilter() {
                ExpiryAfterDate = fiscalYearStartDate,
                StartBeforeDate = fiscalYearStartDate.AddYears(1).AddDays(-1),
                NotInStatus = new List<string>() { PimsLeaseStatusTypes.DRAFT, PimsLeaseStatusTypes.DISCARD },
                IsReceivable = true
            }, true);
        }
    }
}
