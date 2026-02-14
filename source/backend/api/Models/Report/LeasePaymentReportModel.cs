using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Pims.Api.Services;
using Pims.Core.Helpers;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Api.Models.Report.Lease
{
    /// <summary>
    /// Model that represents a single row for the lease payment report.
    /// </summary>
    public class LeasePaymentReportModel
    {
        #region Properties

        /// <summary>
        /// get/set - Lease region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// get/set - PIMS L File Number.
        /// </summary>
        [DisplayName("L-File Number")]
        [CsvHelper.Configuration.Attributes.Name("L-File Number")]
        public string LFileNumber { get; set; }

        /// <summary>
        /// get/set - The set of historical files numbers as a contatenated string.
        /// </summary>
        [DisplayName("Historical files")]
        [CsvHelper.Configuration.Attributes.Name("Historical files")]
        public string HistoricalFiles { get; set; }

        /// <summary>
        /// get/set - Lease status description.
        /// </summary>
        [DisplayName("Lease status")]
        [CsvHelper.Configuration.Attributes.Name("Lease status")]
        public string LeaseStatus { get; set; }

        /// <summary>
        /// get/set - The value of the program name.
        /// </summary>
        [DisplayName("Properties")]
        [CsvHelper.Configuration.Attributes.Name("Properties")]
        public string PropertyList { get; set; }

        /// <summary>
        /// get/set - Tenant list, separated by commas. Only display if type is "tenant".
        /// </summary>
        [DisplayName("Tenant(s)")]
        [CsvHelper.Configuration.Attributes.Name("Tenant(s)")]
        public string TenantList { get; set; }

        /// <summary>
        /// get/set - Either Payable or Receivable.
        /// </summary>
        [DisplayName("Payable/Receivable")]
        [CsvHelper.Configuration.Attributes.Name("Payable/Receivable")]
        public string PayableOrReceivable { get; set; }

        /// <summary>
        /// get/set - Program, including other description.
        /// </summary>
        public string Program { get; set; }

        /// <summary>
        /// get/set - Purpose, including other description.
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// get/set - Start date of the period parent for this lease.
        /// </summary>
        [DisplayName("Period start")]
        [CsvHelper.Configuration.Attributes.Name("Period start")]
        public string PeriodStart { get; set; }

        /// <summary>
        /// get/set - Expiry date of the period parent for this lease.
        /// </summary>
        [DisplayName("Period expiry")]
        [CsvHelper.Configuration.Attributes.Name("Period expiry")]
        public string PeriodExpiry { get; set; }

        /// <summary>
        /// get/set - Whether or not this period is exercised (Yes/No).
        /// </summary>
        [DisplayName("Period exercised")]
        [CsvHelper.Configuration.Attributes.Name("Period exercised")]
        public string IsPeriodExercised { get; set; }

        /// <summary>
        /// get/set - How often this payment occurs within the period.
        /// </summary>
        [DisplayName("Payment frequency")]
        [CsvHelper.Configuration.Attributes.Name("Payment frequency")]
        public string PaymentFrequency { get; set; }

        /// <summary>
        /// get/set - A text description of when the payment is due.
        /// </summary>
        [DisplayName("Payment due")]
        [CsvHelper.Configuration.Attributes.Name("Payment due")]
        public string PaymentDueString { get; set; }

        /// <summary>
        /// get/set - The payment type.
        /// </summary>
        [DisplayName("Payment type")]
        [CsvHelper.Configuration.Attributes.Name("Payment type")]
        public string PaymentType { get; set; }

        /// <summary>
        /// get/set - The rent category.
        /// </summary>
        [DisplayName("Rent category")]
        [CsvHelper.Configuration.Attributes.Name("Rent category")]
        public string RentCategory { get; set; }

        /// <summary>
        /// get/set - The expected payment total, including GST.
        /// </summary>
        [DisplayName("Expected payment")]
        [CsvHelper.Configuration.Attributes.Name("Expected payment")]
        public decimal ExpectedPayment { get; set; }

        /// <summary>
        /// get/set - The actual payment total, including GST.
        /// </summary>
        [DisplayName("Payment total")]
        [CsvHelper.Configuration.Attributes.Name("Payment total")]
        public decimal PaymentTotal { get; set; }

        /// <summary>
        /// get/set - For this payment, if the payment is partial, complete, etc.
        /// </summary>
        [DisplayName("Payment status")]
        [CsvHelper.Configuration.Attributes.Name("Payment status")]
        public string PaymentStatus { get; set; }

        /// <summary>
        /// get/set - The payment amount, not including GST.
        /// </summary>
        [DisplayName("Payment amount (pretax)")]
        [CsvHelper.Configuration.Attributes.Name("Payment amount (pretax)")]
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// get/set - The GST portion of the payment.
        /// </summary>
        [DisplayName("Payment GST")]
        [CsvHelper.Configuration.Attributes.Name("Payment GST")]
        public decimal PaymentGst { get; set; }

        /// <summary>
        /// get/set - The date the payment was received.
        /// </summary>
        [DisplayName("Payment received date")]
        [CsvHelper.Configuration.Attributes.Name("Payment received date")]
        public string PaymentReceivedDate { get; set; }

        /// <summary>
        /// get/set - Most recent payment made for the file.
        /// </summary>
        [DisplayName("Latest payment date")]
        [CsvHelper.Configuration.Attributes.Name("Latest payment date")]
        public string LatestPaymentDate { get; set; }

        /// <summary>
        /// get/set - Notes regarding this payment.
        /// </summary>
        [DisplayName("Payment notes")]
        [CsvHelper.Configuration.Attributes.Name("Payment notes")]
        public string PaymentNote { get; set; }
        #endregion

        public static LeasePaymentReportModel MapFrom(PimsLeasePayment src)
        {
            var dest = new LeasePaymentReportModel();
            var leaseProperties = src.LeasePeriod.Lease?.PimsPropertyLeases ?? new List<PimsPropertyLease>();
            var leaseStakeholders = src.LeasePeriod.Lease?.PimsLeaseStakeholders ?? new List<PimsLeaseStakeholder>();
            var leaseTenants = leaseStakeholders.Where(s => s is not null && string.Equals(s.LeaseStakeholderTypeCode, "TEN", StringComparison.OrdinalIgnoreCase)).ToList();

            dest.Region = src.LeasePeriod.Lease?.RegionCodeNavigation?.Description ?? string.Empty;
            dest.LFileNumber = src.LeasePeriod.Lease?.LFileNo ?? string.Empty;
            dest.HistoricalFiles = GetHistoricalFileNumbers(src.LeasePeriod.Lease);
            dest.LeaseStatus = src.LeasePeriod.Lease?.LeaseStatusTypeCodeNavigation?.Description ?? string.Empty;
            dest.PropertyList = string.Join(",", leaseProperties.Select(lp => GetFallbackPropertyIdentifier(lp)));
            dest.TenantList = string.Join(",", leaseTenants.Select(t => GetStakeholderName(t)));
            dest.PayableOrReceivable = src.LeasePeriod.Lease.LeasePayRvblTypeCodeNavigation?.Description ?? string.Empty;
            dest.Program = GetLeaseProgramName(src.LeasePeriod.Lease);
            dest.PeriodStart = src.LeasePeriod.PeriodStartDate.ToString("MMMM dd, yyyy");
            dest.PeriodExpiry = src.LeasePeriod.PeriodExpiryDate.HasValue ? src.LeasePeriod.PeriodExpiryDate.Value.ToString("MMMM dd, yyyy") : string.Empty;
            dest.IsPeriodExercised = string.Equals(src.LeasePeriod.LeasePeriodStatusTypeCode, "EXER", StringComparison.OrdinalIgnoreCase) ? "Yes" : "No";
            dest.PaymentFrequency = src.LeasePeriod.LeasePmtFreqTypeCodeNavigation?.Description ?? string.Empty;
            dest.PaymentDueString = src.LeasePeriod.PaymentDueDate ?? string.Empty;
            dest.PaymentType = src.LeasePeriod.IsVariablePayment ? "Variable" : "Predeterminded";
            dest.RentCategory = src.LeasePaymentCategoryTypeCodeNavigation?.Description ?? string.Empty;
            dest.ExpectedPayment = src.LeasePeriod.PaymentAmount ?? 0;
            dest.PaymentTotal = src.PaymentAmountTotal;
            dest.PaymentStatus = src.LeasePaymentStatusTypeCodeNavigation != null ? src.LeasePaymentStatusTypeCodeNavigation.Description : LeasePaymentService.GetPaymentStatus(src, src.LeasePeriod);
            dest.PaymentAmount = src.PaymentAmountPreTax;
            dest.PaymentGst = src.PaymentAmountGst ?? 0;
            dest.PaymentReceivedDate = src.PaymentReceivedDate.ToString("MMMM dd, yyyy");
            dest.PaymentNote = src.Note ?? string.Empty;
            dest.LatestPaymentDate = GetLastPaymentDate(src.LeasePeriod.Lease);

            return dest;
        }

        private static string GetLastPaymentDate(PimsLease lease)
        {
            var lastPayment = lease.PimsLeasePeriods?.SelectMany(lp => lp.PimsLeasePayments ?? new List<PimsLeasePayment>())?.OrderByDescending(p => p.PaymentReceivedDate)?.FirstOrDefault();
            return lastPayment != null ? lastPayment.PaymentReceivedDate.ToString("MMMM dd, yyyy") : string.Empty;
        }

        private static string GetStakeholderName(PimsLeaseStakeholder stakeholder)
        {
            if (stakeholder is null)
            {
                return string.Empty;
            }

            if (stakeholder.Person is not null)
            {
                return stakeholder.Person.GetFullName(false);
            }
            else if (stakeholder.Organization is not null)
            {
                return stakeholder.Organization.OrganizationName;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetLeaseProgramName(PimsLease lease)
        {
            if (lease is null)
            {
                return string.Empty;
            }

            if (string.Equals(lease.LeaseProgramTypeCode, "OTHER", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(lease.OtherLeaseProgramType))
            {
                string[] programStrings = new string[2] { lease.LeaseProgramTypeCodeNavigation?.Description, lease.OtherLeaseProgramType };
                return string.Join(" - ", programStrings.Where(ps => ps != null));
            }
            else
            {
                return lease.LeaseProgramTypeCodeNavigation?.Description ?? string.Empty;
            }
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
