using Mapster;
using Pims.Core.Extensions;
using Pims.Dal.Helpers.Extensions;
using System;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Reports.Models.Lease;

namespace Pims.Api.Areas.Reports.Mapping.Lease
{
    public class LeaseMap : IRegister
    {
        private static void MapLease((Entity.PimsLeaseTerm term, Entity.PimsLease lease, Entity.PimsPropertyLease property, Entity.PimsLeaseTenant tenant) src, Model.LeaseModel dest)
        {
            dest.LFileNo = src.lease.LFileNo;
            dest.MotiRegion = src.lease.RegionCodeNavigation?.RegionName;
            dest.StartDate = src.lease.OrigStartDate.FilterSqlMinDate();
            dest.EndDate = src.lease.OrigExpiryDate?.FilterSqlMinDate();
            dest.CurrentTermStartDate = src.lease.GetCurrentTermStartDate()?.FilterSqlMinDate();
            dest.CurrentTermEndDate = src.lease.GetCurrentTermEndDate()?.FilterSqlMinDate();
            dest.ProgramName = src.lease.LeaseProgramTypeCodeNavigation?.Description;
            dest.PurposeType = src.lease.LeasePurposeTypeCodeNavigation?.Description;
            dest.StatusType = src.lease.LeaseStatusTypeCodeNavigation?.Description;
            dest.LeaseTypeName = src.lease.LeaseLicenseTypeCodeNavigation?.Description;
            dest.PsFileNo = src.lease.PsFileNo;
            dest.LeaseAmount = src.lease.LeaseAmount;
            dest.InspectionNotes = src.lease.InspectionNotes;
            dest.InspectionDate = src.lease.InspectionDate?.FilterSqlMinDate();
            dest.LeaseNotes = src.lease.LeaseNotes;
            dest.IsExpired = src.lease.GetExpiryDate() < DateTime.Now;
            dest.TermStartDate = src.term?.TermStartDate.FilterSqlMinDate();
            dest.TermExpiryDate = src.term?.TermExpiryDate?.FilterSqlMinDate();
            dest.TermRenewalDate = src.term?.TermRenewalDate?.FilterSqlMinDate();
            dest.LeasePaymentFrequencyType = src.term?.LeasePmtFreqTypeCodeNavigation?.Description;
            dest.CivicAddress = src.property?.Property?.Address?.FormatAddress(true);
            dest.Pid = src.property?.Property?.Pid;
            dest.Pin = src.property?.Property?.Pin;
            dest.TenantName = src.tenant?.GetTenantName();
        }
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Entity.PimsLeaseTerm term, Entity.PimsLease lease, Entity.PimsPropertyLease property, Entity.PimsLeaseTenant tenant), Model.LeaseModel>()
                .AfterMapping((src, dest) =>
                {
                    MapLease(src, dest);
                });
        }
    }
}
