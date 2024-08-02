using System;
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
            dest.LFileNo = src.lease.LFileNo;
            dest.MotiRegion = src.lease.RegionCodeNavigation?.RegionName;
            dest.StartDate = src.lease.OrigStartDate.FilterSqlMinDate().ToNullableDateOnly();
            dest.EndDate = leaseExpiryDate.ToNullableDateOnly();
            dest.CurrentPeriodStartDate = src.lease.GetCurrentPeriodStartDate()?.FilterSqlMinDate().ToNullableDateOnly();
            dest.CurrentTermEndDate = src.lease.GetCurrentPeriodEndDate()?.FilterSqlMinDate().ToNullableDateOnly();
            dest.ProgramName = src.lease.LeaseProgramTypeCodeNavigation?.GetTypeDescriptionOther(src.lease.OtherLeaseProgramType);
            //dest.PurposeType = src.lease.LeasePurposeTypeCodeNavigation?.GetTypeDescriptionOther(src.lease.OtherLeasePurposeType);
            dest.StatusType = src.lease.LeaseStatusTypeCodeNavigation?.Description;
            dest.LeaseTypeName = src.lease.LeaseLicenseTypeCodeNavigation?.GetTypeDescriptionOther(src.lease.OtherLeaseLicenseType);
            dest.PsFileNo = src.lease.PsFileNo;
            dest.LeaseArea = src.property?.LeaseArea;
            dest.AreaUnit = src.property?.AreaUnitTypeCodeNavigation?.Description;
            dest.InspectionNotes = src.lease.InspectionNotes;
            dest.InspectionDate = src.lease.InspectionDate?.FilterSqlMinDate();
            dest.LeaseNotes = src.lease.LeaseNotes;
            dest.IsExpired = (leaseExpiryDate < DateTime.Now).BoolToYesNo();
            dest.LeaseAmount = src.period?.PaymentAmount;
            dest.PeriodStartDate = src.period?.PeriodStartDate.FilterSqlMinDate().ToNullableDateOnly();
            dest.PeriodExpiryDate = src.period?.PeriodExpiryDate?.FilterSqlMinDate().ToNullableDateOnly();
            dest.PeriodRenewalDate = src.period?.PeriodRenewalDate?.FilterSqlMinDate().ToNullableDateOnly();
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
