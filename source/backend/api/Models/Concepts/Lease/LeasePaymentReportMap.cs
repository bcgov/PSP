using System.Linq;
using Mapster;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class LeasePaymentReportMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeasePayment, Model.LeasePaymentReportModel>()
                .Map(dest => dest.Region, src => src.LeaseTerm.Lease.RegionCodeNavigation != null ? src.LeaseTerm.Lease.RegionCodeNavigation.Description : string.Empty)
                .Map(dest => dest.LFileNumber, src => src.LeaseTerm.Lease.LFileNo)
                .Map(dest => dest.LisNumber, src => src.LeaseTerm.Lease.TfaFileNumber)
                .Map(dest => dest.PsFileNumber, src => src.LeaseTerm.Lease.PsFileNo)
                .Map(dest => dest.LeaseStatus, src => src.LeaseTerm.Lease.LeaseStatusTypeCodeNavigation.Description)
                .Map(dest => dest.PropertyList, src => string.Join(",", src.LeaseTerm.Lease.PimsPropertyLeases.Select(x => GetFallbackPropertyIdentifier(x))))
                .Map(dest => dest.TenantList, src => string.Join(",", src.LeaseTerm.Lease.PimsLeaseTenants.Where(t => t != null && t.TenantTypeCode == "TEN").Select(x => x != null && x.Person != null ? x.Person.GetFullName(false) : x != null && x.Organization != null ? x.Organization.OrganizationName : string.Empty)))
                .Map(dest => dest.PayableOrReceivable, src => src.LeaseTerm.Lease.LeasePayRvblTypeCodeNavigation.Description)
                .Map(dest => dest.Program, src => src.LeaseTerm.Lease.LeaseProgramTypeCode.ToUpper() == "OTHER" && !string.IsNullOrEmpty(src.LeaseTerm.Lease.OtherLeaseProgramType) ? $"{src.LeaseTerm.Lease.LeaseProgramTypeCodeNavigation.Description} - {src.LeaseTerm.Lease.OtherLeaseProgramType}" : src.LeaseTerm.Lease.LeaseProgramTypeCodeNavigation.Description)
                .Map(dest => dest.Purpose, src => src.LeaseTerm.Lease.LeasePurposeTypeCode.ToUpper() == "OTHER" && !string.IsNullOrEmpty(src.LeaseTerm.Lease.OtherLeasePurposeType) ? $"{src.LeaseTerm.Lease.LeasePurposeTypeCodeNavigation.Description} - {src.LeaseTerm.Lease.OtherLeasePurposeType}" : src.LeaseTerm.Lease.LeasePurposeTypeCodeNavigation.Description)
                .Map(dest => dest.TermStart, src => src.LeaseTerm.TermStartDate.ToString("MMMM dd, yyyy"))
                .Map(dest => dest.TermExpiry, src => src.LeaseTerm.TermExpiryDate.HasValue ? src.LeaseTerm.TermExpiryDate.Value.ToString("MMMM dd, yyyy") : string.Empty)
                .Map(dest => dest.IsTermExercised, src => src.LeaseTerm.LeaseTermStatusTypeCode == "EXER" ? "Yes" : "No")
                .Map(dest => dest.PaymentFrequency, src => src.LeaseTerm.LeasePmtFreqTypeCodeNavigation != null ? src.LeaseTerm.LeasePmtFreqTypeCodeNavigation.Description : string.Empty)
                .Map(dest => dest.PaymentDueString, src => src.LeaseTerm.PaymentDueDate)
                .Map(dest => dest.ExpectedPayment, src => src.LeaseTerm.PaymentAmount)
                .Map(dest => dest.PaymentTotal, src => src.PaymentAmountTotal)
                .Map(dest => dest.PaymentStatus, src => src.LeasePaymentStatusTypeCodeNavigation != null ? src.LeasePaymentStatusTypeCodeNavigation.Description : string.Empty)
                .Map(dest => dest.PaymentAmount, src => src.PaymentAmountPreTax)
                .Map(dest => dest.PaymentGst, src => src.PaymentAmountGst)
                .Map(dest => dest.PaymentReceivedDate, src => src.PaymentReceivedDate.ToString("MMMM dd, yyyy"))
                .Map(dest => dest.LatestPaymentDate, src => src.LeaseTerm.PimsLeasePayments.OrderByDescending(lp => lp.PaymentReceivedDate).FirstOrDefault() != null ?
                    src.LeaseTerm.PimsLeasePayments.OrderByDescending(lp => lp.PaymentReceivedDate).FirstOrDefault().PaymentReceivedDate.ToString("MMMM dd, yyyy") : string.Empty);
        }

        private static string GetFallbackPropertyIdentifier(PimsPropertyLease propertyLease)
        {
            PimsProperty property = propertyLease.Property;
            if (property?.Pid != null)
            {
                return property.Pid.ToString().ConvertPIDToDash();
            }
            else if (property?.Pin != null)
            {
                return property.Pin.ToString();
            }
            else if (property?.Address != null && !string.IsNullOrEmpty(property.Address.StreetAddress1))
            {
                string[] addressStrings = new string[2] { property.Address.StreetAddress1, property.Address.MunicipalityName };
                return $"({string.Join(" ", addressStrings.Where(s => s != null))})";
            }
            else if (!string.IsNullOrEmpty(propertyLease?.Name))
            {
                return $"({propertyLease.Name})";
            }
            else if (property?.Location != null)
            {
                return $"({property.Location.Coordinate.X}, {property.Location.Coordinate.Y})";
            }
            else
            {
                return "No Property Identifier";
            }
        }
    }
}
