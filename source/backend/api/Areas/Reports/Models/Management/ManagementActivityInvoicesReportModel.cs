using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Api.Areas.Reports.Models.Management
{
    public class ManagementActivityInvoicesReportModel
    {

        [DisplayName("Invoice Number")]
        [CsvHelper.Configuration.Attributes.Name("Invoice Number")]
        public string InvoiceNumber { get; set; }

        [DisplayName("Management File Name")]
        [CsvHelper.Configuration.Attributes.Name("Management File Name")]
        public string ManagementFileName { get; set; }

        [DisplayName("Historical File Number")]
        [CsvHelper.Configuration.Attributes.Name("Historical File Number")]
        public string LegacyFileNum { get; set; }

        [DisplayName("Funding")]
        [CsvHelper.Configuration.Attributes.Name("Funding")]
        public string Funding { get; set; }

        [DisplayName("Purpose")]
        [CsvHelper.Configuration.Attributes.Name("Purpose")]
        public string Purpose { get; set; }

        [DisplayName("Created By")]
        [CsvHelper.Configuration.Attributes.Name("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Property Contacts")]
        [CsvHelper.Configuration.Attributes.Name("Property Contacts")]
        public string PropertyContacts { get; set; }

        [DisplayName("Management File Status")]
        [CsvHelper.Configuration.Attributes.Name("Management File Status")]
        public string ManagementFileStatus { get; set; }

        [DisplayName("Activity Type")]
        [CsvHelper.Configuration.Attributes.Name("Activity Type")]
        public string ActivityType { get; set; }

        [DisplayName("Activity Sub-Types")]
        [CsvHelper.Configuration.Attributes.Name("Activity Sub-Types")]
        public string ActivitySubTypes { get; set; }

        [DisplayName("Completion Date")]
        [CsvHelper.Configuration.Attributes.Name("Completion Date")]
        public string CompletionDate { get; set; }

        [DisplayName("Services Provider")]
        [CsvHelper.Configuration.Attributes.Name("Services Provider")]
        public string ServicesProvider { get; set; }

        [DisplayName("Regions")]
        [CsvHelper.Configuration.Attributes.Name("Regions")]
        public string Regions { get; set; }

        [DisplayName("Properties")]
        [CsvHelper.Configuration.Attributes.Name("Properties")]
        public string Properties { get; set; }

        [DisplayName("Total (before tax) [invoice]")]
        [CsvHelper.Configuration.Attributes.Name("Total (before tax) [invoice]")]
        public decimal InvoicePreTaxTotal { get; set; }

        [DisplayName("GST Amount [invoice]")]
        [CsvHelper.Configuration.Attributes.Name("GST Amount [invoice]")]
        public decimal InvoiceGstAmount { get; set; }

        [DisplayName("PST Amount [invoice]")]
        [CsvHelper.Configuration.Attributes.Name("PST Amount [invoice]")]
        public decimal InvoicePstAmount { get; set; }

        [DisplayName("Total amount [invoice]")]
        [CsvHelper.Configuration.Attributes.Name("Total amount [invoice]")]
        public decimal InvoiceTotal { get; set; }

        [DisplayName("Payment Approved")]
        [CsvHelper.Configuration.Attributes.Name("Payment Approved")]
        public string IsPaymentApproved { get; set; }

        [DisplayName("Payment Forwarded")]
        [CsvHelper.Configuration.Attributes.Name("Payment Forwarded")]
        public string IsPaymentForwarded { get; set; }

        [DisplayName("Approved Invoices Total (before tax) [activity]")]
        [CsvHelper.Configuration.Attributes.Name("Approved Invoices Total (before tax) [activity]")]
        public decimal ApprovedPreTaxTotal { get; set; }

        [DisplayName("Approved Invoices GST amount [activity]")]
        [CsvHelper.Configuration.Attributes.Name("Approved Invoices GST amount [activity]")]
        public decimal ApprovedGstAmount { get; set; }

        [DisplayName("Approved Invoices PST amount [activity]")]
        [CsvHelper.Configuration.Attributes.Name("Approved Invoices PST amount [activity]")]
        public decimal ApprovedPstAmount { get; set; }

        [DisplayName("Approved Invoices Total amount [activity]")]
        [CsvHelper.Configuration.Attributes.Name("Approved Invoices Total amount [activity]")]
        public decimal ApprovedTotal { get; set; }

        [DisplayName("All Invoices Total (before tax) [activity]")]
        [CsvHelper.Configuration.Attributes.Name("All Invoices Total (before tax) [activity]")]
        public decimal AllPreTaxTotal { get; set; }

        [DisplayName("All Invoices GST amount [activity]")]
        [CsvHelper.Configuration.Attributes.Name("All Invoices GST amount [activity]")]
        public decimal AllGstAmount { get; set; }

        [DisplayName("All Invoices PST amount [activity]")]
        [CsvHelper.Configuration.Attributes.Name("All Invoices PST amount [activity]")]
        public decimal AllPstAmount { get; set; }

        [DisplayName("All Invoices Total amount [activity]")]
        [CsvHelper.Configuration.Attributes.Name("All Invoices Total amount [activity]")]
        public decimal AllTotal { get; set; }

        public ManagementActivityInvoicesReportModel(PimsManagementActivityInvoice invoice)
        {
            ArgumentNullException.ThrowIfNull(invoice, nameof(invoice));

            InvoiceNumber = GetNullableString(invoice.InvoiceNum);
            ManagementFileName = GetNullableString(invoice.ManagementActivity?.ManagementFile?.FileName);
            LegacyFileNum = GetNullableString(invoice.ManagementActivity?.ManagementFile?.LegacyFileNum);
            Funding = GetNullableString(invoice.ManagementActivity?.ManagementFile?.AcquisitionFundingTypeCodeNavigation?.Description);
            Purpose = GetNullableString(invoice.ManagementActivity?.ManagementFile?.ManagementFilePurposeTypeCodeNavigation?.Description);
            CreatedBy = GetNullableString(invoice.AppCreateUserid);
            PropertyContacts = GetPropertyContactsAsString(invoice.ManagementActivity?.ManagementFile?.PimsManagementFileContacts);
            ManagementFileStatus = GetNullableString(invoice.ManagementActivity?.ManagementFile?.ManagementFileStatusTypeCodeNavigation?.Description);
            ActivityType = GetNullableString(invoice.ManagementActivity?.MgmtActivityTypeCodeNavigation?.Description);
            ActivitySubTypes = GetActivitySubTypesAsString(invoice.ManagementActivity?.PimsMgmtActivityActivitySubtyps);
            CompletionDate = GetNullableDate(invoice.ManagementActivity?.CompletionDt);
            ServicesProvider = invoice.ManagementActivity?.ServiceProviderPerson?.GetFullName() ?? GetNullableString(invoice.ManagementActivity?.ServiceProviderOrg?.Name);
            Regions = GetRegionsAsString(invoice.ManagementActivity);
            Properties = GetPropertiesAsString(invoice.ManagementActivity);
            InvoicePreTaxTotal = invoice.PretaxAmt;
            InvoiceGstAmount = invoice.GstAmt ?? 0;
            InvoicePstAmount = invoice.PstAmt ?? 0;
            InvoiceTotal = invoice.TotalAmt ?? 0;
            IsPaymentApproved = invoice.IsPaymentApproved.BoolToYesNo();
            IsPaymentForwarded = invoice.IsPaymentForwarded.BoolToYesNo();
            ApprovedPreTaxTotal = invoice.ManagementActivity?.PimsManagementActivityInvoices?
                .Where(i => i.IsPaymentApproved)
                .Sum(i => i.PretaxAmt) ?? 0;
            ApprovedGstAmount = invoice.ManagementActivity?.PimsManagementActivityInvoices?
                .Where(i => i.IsPaymentApproved)
                .Sum(i => i.GstAmt ?? 0) ?? 0;
            ApprovedPstAmount = invoice.ManagementActivity?.PimsManagementActivityInvoices?
                .Where(i => i.IsPaymentApproved)
                .Sum(i => i.PstAmt ?? 0) ?? 0;
            ApprovedTotal = invoice.ManagementActivity?.PimsManagementActivityInvoices?
                .Where(i => i.IsPaymentApproved)
                .Sum(i => i.TotalAmt ?? 0) ?? 0;
            AllPreTaxTotal = invoice.ManagementActivity?.PimsManagementActivityInvoices?.Sum(i => i.PretaxAmt) ?? 0;
            AllGstAmount = invoice.ManagementActivity?.PimsManagementActivityInvoices?.Sum(i => i.GstAmt ?? 0) ?? 0;
            AllPstAmount = invoice.ManagementActivity?.PimsManagementActivityInvoices?.Sum(i => i.PstAmt ?? 0) ?? 0;
            AllTotal = invoice.ManagementActivity?.PimsManagementActivityInvoices?.Sum(i => i.TotalAmt ?? 0) ?? 0;
        }

        private static string GetNullableString(string value)
        {
            return value ?? string.Empty;
        }

        private static string GetNullableDate(DateOnly? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;
        }

        private static string GetPropertiesAsString(PimsManagementActivity activity)
        {
            if (activity?.PimsManagementActivityProperties is not null)
            {
                return string.Join("|", activity.PimsManagementActivityProperties
                        .Where(fp => fp?.Property != null)
                        .Select(fp => fp.Property.GetPropertyName())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct());
            }

            return string.Empty;
        }

        private static string GetPropertyContactsAsString(ICollection<PimsManagementFileContact> contacts)
        {
            if (contacts is not null)
            {
                return string.Join("|", contacts
                        .Select(c => c?.Person?.GetFullName() ?? GetNullableString(c?.Organization?.Name))
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct());
            }
            return string.Empty;
        }

        private static string GetActivitySubTypesAsString(ICollection<PimsMgmtActivityActivitySubtyp> subTypes)
        {
            if (subTypes is not null)
            {
                return string.Join("|", subTypes
                        .Select(st => st?.MgmtActivitySubtypeCodeNavigation?.Description)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct());
            }
            return string.Empty;
        }

        private static string GetRegionsAsString(PimsManagementActivity activity)
        {
            if (activity is not null)
            {
                var activityRegions = activity.PimsManagementActivityProperties
                        .Select(map => map?.Property?.RegionCodeNavigation?.Description)
                        .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                if (activity.ManagementFile?.RegionCode != null)
                {
                    activityRegions.Add(activity.ManagementFile.RegionCodeNavigation?.Description);
                }
                return string.Join("|", activityRegions.Distinct());
            }
            return string.Empty;
        }
    }
}
