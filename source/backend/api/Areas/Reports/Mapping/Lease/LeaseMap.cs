using System;
using System.Linq;
using Mapster;
using Pims.Core.Extensions;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Reports.Models.Lease;

namespace Pims.Api.Areas.Reports.Mapping.Lease
{
    public class LeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Entity.PimsLeasePeriod period, Entity.PimsLease lease, Entity.PimsPropertyLease property, Entity.PimsLeaseStakeholder stakeholder), Model.LeaseModel>()
                .AfterMapping((src, dest) =>
                {
                    MapLease(src, dest);
                });
        }

        private static void MapLease((Entity.PimsLeasePeriod period, Entity.PimsLease lease, Entity.PimsPropertyLease property, Entity.PimsLeaseStakeholder stakeholder) src, Model.LeaseModel dest)
        {
            var leaseExpiryDate = src.lease.GetExpiryDate();
            var historicalString = src.property?.Property?.GetHistoricalNumbersAsString();
            var currentRenewal = src.lease.PimsLeaseRenewals?.FirstOrDefault(renewal => renewal != null && renewal.IsExercised == true && DateTime.Now > renewal.CommencementDt && DateTime.Now <= renewal.ExpiryDt);

            var additionalRenewalsCount = src.lease.PimsLeaseRenewals?.FirstOrDefault(r => r.LeaseRenewalId == currentRenewal?.LeaseRenewalId) == null ? src.lease.PimsLeaseRenewals?.Count ?? 0 : src.lease.PimsLeaseRenewals.Count - 1;

            dest.LFileNo = src.lease.LFileNo;
            dest.HistoricalFileNo = historicalString;
            dest.AgreementCommencementDate = src.lease.OrigStartDate?.FilterSqlMinDate().ToNullableDateOnly();
            dest.AgreementExpiryDate = src.lease.OrigExpiryDate?.FilterSqlMinDate().ToNullableDateOnly();
            dest.LeaseAmount = src.period?.PaymentAmount;
            dest.Pid = src.property?.Property?.PidFormatted ?? string.Empty;
            dest.Pin = src.property?.Property?.Pin.ToString() ?? string.Empty;
            dest.TenantName = src.stakeholder?.GetStakeholderName();
        }
    }
}
