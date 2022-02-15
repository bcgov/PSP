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
        private static void MapLease(Entity.PimsLease src, Model.LeaseModel dest)
        {
            dest.LFileNo = src.LFileNo;
            dest.StartDate = src.OrigStartDate.FilterSqlMinDate();
            dest.EndDate = src.OrigExpiryDate.FilterSqlMinDate();
            dest.CurrentTermStartDate = src.GetCurrentTermStartDate().FilterSqlMinDate();
            dest.CurrentTermEndDate = src.GetCurrentTermEndDate();
            dest.ProgramName = src?.LeaseProgramTypeCodeNavigation?.Description;
            dest.PurposeType = src?.LeasePurposeTypeCodeNavigation?.Description;
            dest.StatusType = src?.LeaseStatusTypeCodeNavigation?.Description;
            dest.PsFileNo = src.PsFileNo;
            dest.InspectionNotes = src.InspectionNotes;
            dest.InspectionDate = src.InspectionDate;
            dest.LeaseNotes = src.LeaseNotes;
            dest.IsExpired = src.GetExpiryDate() < DateTime.Now;
        }
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Entity.PimsLease, Entity.PimsLeaseTenant), Model.LeaseModel>()
                .Map(dest => dest, src => src.Item1)
                .Map(dest => dest.TenantName, src => src.Item2.GetTenantName())
                .AfterMapping((src, dest) =>
                {
                    MapLease(src.Item1, dest);
                });

            config.NewConfig<(Entity.PimsLease, Entity.PimsPropertyLease), Model.LeaseModel>()
                .Map(dest => dest, src => src.Item1)
                .AfterMapping((src, dest) =>
                {
                    var property = src.Item2?.Property;
                    dest.Pin = property?.Pin.ToString();
                    dest.Pid = property?.Pid.ToString();
                    dest.CivicAddress = property?.Address?.StreetAddress1;
                    dest.Unit = property?.PropertyAreaUnitTypeCodeNavigation?.Description;

                    MapLease(src.Item1, dest);
                });

            config.NewConfig<(Entity.PimsLease, Entity.PimsLeaseTerm), Model.LeaseModel>()
                .Map(dest => dest, src => src.Item1)
                .AfterMapping((src, dest) =>
                {
                    var term = src.Item2;
                    dest.TermStartDate = term?.TermStartDate.FilterSqlMinDate();
                    dest.TermExpiryDate = term?.TermExpiryDate.FilterSqlMinDate();
                    dest.TermRenewalDate = term?.TermRenewalDate.FilterSqlMinDate();

                    MapLease(src.Item1, dest);
                });
        }
    }
}
