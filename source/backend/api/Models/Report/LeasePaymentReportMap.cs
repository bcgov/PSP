using System.Collections.Generic;
using System.Linq;
using Mapster;
using Pims.Api.Services;
using Pims.Core.Helpers;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Report.Lease
{
    public class LeasePaymentReportMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeasePayment, LeasePaymentReportModel>()
                .Map(dest => dest.Region, src => src.LeasePeriod.Lease.RegionCodeNavigation != null ? src.LeasePeriod.Lease.RegionCodeNavigation.Description : string.Empty)
                .Map(dest => dest.LFileNumber, src => src.LeasePeriod.Lease.LFileNo)
                .Map(dest => dest.HistoricalFiles, src => GetHistoricalFileNumbers(src.LeasePeriod.Lease))
                .Map(dest => dest.LeaseStatus, src => src.LeasePeriod.Lease.LeaseStatusTypeCodeNavigation.Description)
                .Map(dest => dest.PropertyList, src => string.Join(",", src.LeasePeriod.Lease.PimsPropertyLeases.Select(x => GetFallbackPropertyIdentifier(x))))
                .Map(dest => dest.TenantList, src => string.Join(",", src.LeasePeriod.Lease.PimsLeaseStakeholders.Where(t => t != null && t.LeaseStakeholderTypeCode == "TEN").Select(x => x != null && x.Person != null ? x.Person.GetFullName(false) : x != null && x.Organization != null ? x.Organization.OrganizationName : string.Empty)))
                .Map(dest => dest.PayableOrReceivable, src => src.LeasePeriod.Lease.LeasePayRvblTypeCodeNavigation.Description)
                .Map(dest => dest.Program, src => src.LeasePeriod.Lease.LeaseProgramTypeCode.ToUpper() == "OTHER" && !string.IsNullOrEmpty(src.LeasePeriod.Lease.OtherLeaseProgramType) ? $"{src.LeasePeriod.Lease.LeaseProgramTypeCodeNavigation.Description} - {src.LeasePeriod.Lease.OtherLeaseProgramType}" : src.LeasePeriod.Lease.LeaseProgramTypeCodeNavigation.Description)
                .Map(dest => dest.PeriodStart, src => src.LeasePeriod.PeriodStartDate.ToString("MMMM dd, yyyy"))
                .Map(dest => dest.PeriodExpiry, src => src.LeasePeriod.PeriodExpiryDate.HasValue ? src.LeasePeriod.PeriodExpiryDate.Value.ToString("MMMM dd, yyyy") : string.Empty)
                .Map(dest => dest.IsPeriodExercised, src => src.LeasePeriod.LeasePeriodStatusTypeCode == "EXER" ? "Yes" : "No")
                .Map(dest => dest.PaymentFrequency, src => src.LeasePeriod.LeasePmtFreqTypeCodeNavigation != null ? src.LeasePeriod.LeasePmtFreqTypeCodeNavigation.Description : string.Empty)
                .Map(dest => dest.PaymentDueString, src => src.LeasePeriod.PaymentDueDate)
                .Map(dest => dest.PaymentType, src => src.LeasePeriod.IsVariablePayment ? "Variable" : "Predeterminded")
                .Map(dest => dest.RentCategory, src => src.LeasePaymentCategoryTypeCodeNavigation.Description)
                .Map(dest => dest.ExpectedPayment, src => src.LeasePeriod.PaymentAmount)
                .Map(dest => dest.PaymentTotal, src => src.PaymentAmountTotal)
                .Map(dest => dest.PaymentStatus, src => src.LeasePaymentStatusTypeCodeNavigation != null ? src.LeasePaymentStatusTypeCodeNavigation.Description : LeasePaymentService.GetPaymentStatus(src, src.LeasePeriod))
                .Map(dest => dest.PaymentAmount, src => src.PaymentAmountPreTax)
                .Map(dest => dest.PaymentGst, src => src.PaymentAmountGst)
                .Map(dest => dest.PaymentReceivedDate, src => src.PaymentReceivedDate.ToString("MMMM dd, yyyy"))
                .Map(dest => dest.LatestPaymentDate, src => src.LeasePeriod.Lease.PimsLeasePeriods.SelectMany(lp => lp.PimsLeasePayments).OrderByDescending(lp => lp.PaymentReceivedDate).FirstOrDefault() != null ?
                    src.LeasePeriod.Lease.PimsLeasePeriods.SelectMany(lp => lp.PimsLeasePayments).OrderByDescending(lp => lp.PaymentReceivedDate).FirstOrDefault().PaymentReceivedDate.ToString("MMMM dd, yyyy") : string.Empty);
        }

        private static string GetFallbackPropertyIdentifier(PimsPropertyLease propertyLease)
        {
            PimsProperty property = propertyLease.Property;
            if (property?.Pid != null)
            {
                return PidTranslator.ConvertPIDToDash(property.Pid.ToString());
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

        private static string GetHistoricalFileNumbers(PimsLease lease)
        {
            var properties = lease.PimsPropertyLeases.Select(pl => pl.Property).Where(p => p != null);
            var historicalDictionary = new Dictionary<string, PimsHistoricalFileNumberType>();
            foreach (var property in properties)
            {
                foreach (var historical in property.PimsHistoricalFileNumbers)
                {
                    var historicalType = historical.HistoricalFileNumberTypeCodeNavigation.Description;
                    if (historical.HistoricalFileNumberTypeCodeNavigation.HistoricalFileNumberTypeCode == "OTHER")
                    {
                        historicalType = historical.OtherHistFileNumberTypeCode;
                    }

                    var identifier = $"{historicalType}: {historical.HistoricalFileNumber}";
                    historicalDictionary[identifier] = historical.HistoricalFileNumberTypeCodeNavigation;
                }
            }

            var historicalList = historicalDictionary.ToList();
            historicalList.Sort((a, b) => a.Value.DisplayOrder.GetValueOrDefault() - b.Value.DisplayOrder.GetValueOrDefault());
            return string.Join("; ", historicalList.Select(a => a.Key));
        }
    }
}
