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
            config.NewConfig<(Entity.PimsLeaseTerm term, Entity.PimsLease lease, Entity.PimsPropertyLease property, Entity.PimsLeaseTenant tenant), Model.LeaseModel>()
                .AfterMapping((src, dest) =>
                {
                    MapLease(src, dest);
                });
        }

        private static void MapLease((Entity.PimsLeaseTerm term, Entity.PimsLease lease, Entity.PimsPropertyLease property, Entity.PimsLeaseTenant tenant) src, Model.LeaseModel dest)
        {
            dest.LFileNo = src.lease.LFileNo;
            dest.MotiRegion = src.lease.RegionCodeNavigation?.RegionName;
            dest.StartDate = src.lease.OrigStartDate.FilterSqlMinDate();
            dest.EndDate = src.lease.OrigExpiryDate?.FilterSqlMinDate();
            dest.CurrentTermStartDate = src.lease.GetCurrentTermStartDate()?.FilterSqlMinDate();
            dest.CurrentTermEndDate = src.lease.GetCurrentTermEndDate()?.FilterSqlMinDate();
            dest.ProgramName = src.lease.LeaseProgramTypeCodeNavigation?.GetTypeDescriptionOther(src.lease.OtherLeaseProgramType);
            dest.PurposeType = src.lease.LeasePurposeTypeCodeNavigation?.GetTypeDescriptionOther(src.lease.OtherLeasePurposeType);
            dest.StatusType = src.lease.LeaseStatusTypeCodeNavigation?.Description;
            dest.LeaseTypeName = src.lease.LeaseLicenseTypeCodeNavigation?.GetTypeDescriptionOther(src.lease.OtherLeaseLicenseType);
            dest.PsFileNo = src.lease.PsFileNo;
            dest.LeaseArea = src.property?.LeaseArea;
            dest.AreaUnit = src.property?.AreaUnitTypeCodeNavigation?.Description;
            dest.InspectionNotes = src.lease.InspectionNotes;
            dest.InspectionDate = src.lease.InspectionDate?.FilterSqlMinDate();
            dest.LeaseNotes = src.lease.LeaseNotes;
            dest.IsExpired = (src.lease.GetExpiryDate() < DateTime.Now).BoolToYesNo();
            dest.LeaseAmount = src.term?.PaymentAmount;
            dest.TermStartDate = src.term?.TermStartDate.FilterSqlMinDate();
            dest.TermExpiryDate = src.term?.TermExpiryDate?.FilterSqlMinDate();
            dest.TermRenewalDate = src.term?.TermRenewalDate?.FilterSqlMinDate();
            dest.LeasePaymentFrequencyType = src.term?.LeasePmtFreqTypeCodeNavigation?.Description;
            dest.CivicAddress = src.property?.Property?.Address?.FormatAddress(true);
            dest.Pid = src.property?.Property?.Pid;
            dest.Pin = src.property?.Property?.Pin;
            dest.TenantName = src.tenant?.GetTenantName();
        }
    }
}
