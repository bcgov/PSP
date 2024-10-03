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
            dest.MotiRegion = src.lease.RegionCodeNavigation?.RegionName;
            dest.AgreementCommencementDate = src.lease.OrigStartDate?.FilterSqlMinDate().ToNullableDateOnly();
            dest.AgreementExpiryDate = src.lease.OrigExpiryDate?.FilterSqlMinDate().ToNullableDateOnly();
            dest.TerminationDate = src.lease.TerminationDate?.FilterSqlMinDate().ToNullableDateOnly();
            dest.CurrentRenewalCommencementDate = currentRenewal?.CommencementDt?.FilterSqlMinDate().ToNullableDateOnly();
            dest.CurrentRenewalExpiryDate = currentRenewal?.ExpiryDt?.FilterSqlMinDate().ToNullableDateOnly();
            dest.AdditionalRenewalOptionsCount = additionalRenewalsCount;
            dest.FinalRenewalExpiryDate = src.lease.PimsLeaseRenewals?.OrderBy(l => l.ExpiryDt).LastOrDefault()?.ExpiryDt?.FilterSqlMinDate().ToNullableDateOnly();
            dest.StartDate = src.lease.OrigStartDate?.FilterSqlMinDate().ToNullableDateOnly();
            dest.EndDate = leaseExpiryDate.ToNullableDateOnly();
            dest.CurrentPeriodStartDate = src.lease.GetCurrentPeriodStartDate()?.FilterSqlMinDate().ToNullableDateOnly();
            dest.CurrentTermEndDate = src.lease.GetCurrentPeriodEndDate()?.FilterSqlMinDate().ToNullableDateOnly();
            dest.ProgramName = src.lease.LeaseProgramTypeCodeNavigation?.GetTypeDescriptionOther(src.lease.OtherLeaseProgramType);
            dest.PurposeTypes = string.Join(",", src.lease.PimsLeaseLeasePurposes?.Select(lp => lp.LeasePurposeTypeCodeNavigation.GetTypeDescriptionOther(lp.LeasePurposeTypeCodeNavigation.Description)));
            dest.StatusType = src.lease.LeaseStatusTypeCodeNavigation?.Description;
            dest.LeaseTypeName = src.lease.LeaseLicenseTypeCodeNavigation?.GetTypeDescriptionOther(src.lease.OtherLeaseLicenseType);
            dest.LeaseArea = src.property?.LeaseArea;
            dest.AreaUnit = src.property?.AreaUnitTypeCodeNavigation?.Description;
            dest.LeaseNotes = src.lease.LeaseNotes;
            dest.IsExpired = (leaseExpiryDate < DateTime.Now).BoolToYesNo();
            dest.LeaseAmount = src.period?.PaymentAmount;
            dest.LeasePaymentFrequencyType = src.period?.LeasePmtFreqTypeCodeNavigation?.Description;
            dest.CivicAddress = src.property?.Property?.Address?.FormatAddress(true);
            dest.Pid = src.property?.Property?.Pid;
            dest.Pin = src.property?.Property?.Pin;
            dest.TenantName = src.stakeholder?.GetStakeholderName();
            dest.FinancialGain = src.lease.IsFinancialGain.BoolToYesNoUnknown();
            dest.PublicBenefit = src.lease.IsPublicBenefit.BoolToYesNoUnknown();
        }
    }
}
