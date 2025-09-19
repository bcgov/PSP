using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Api.Areas.Reports.Models.Management
{
    public class ManagementActivityOverviewReportModel
    {

        [DisplayName("Management File Name")]
        [CsvHelper.Configuration.Attributes.Name("Management File Name")]
        public string ManagementFileName { get; set; }

        [DisplayName("Historical File Number")]
        [CsvHelper.Configuration.Attributes.Name("Historical File Number")]
        public string LegacyFileNum { get; set; }

        [DisplayName("Properties")]
        [CsvHelper.Configuration.Attributes.Name("Properties")]
        public string Properties { get; set; }

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

        [DisplayName("Activity Status")]
        [CsvHelper.Configuration.Attributes.Name("Activity Status")]
        public string ActivityStatus { get; set; }

        [DisplayName("Activity Type")]
        [CsvHelper.Configuration.Attributes.Name("Activity Type")]
        public string ActivityType { get; set; }

        [DisplayName("Activity Sub-Types")]
        [CsvHelper.Configuration.Attributes.Name("Activity Sub-Types")]
        public string ActivitySubTypes { get; set; }

        [DisplayName("Services Provider")]
        [CsvHelper.Configuration.Attributes.Name("Services Provider")]
        public string ServicesProvider { get; set; }

        [DisplayName("Ministry Contacts")]
        [CsvHelper.Configuration.Attributes.Name("Ministry Contacts")]
        public string MinistryContacts { get; set; }

        [DisplayName("Request Added Date")]
        [CsvHelper.Configuration.Attributes.Name("Request Added Date")]
        public string RequestAddedDate { get; set; }

        [DisplayName("Completion Date")]
        [CsvHelper.Configuration.Attributes.Name("Completion Date")]
        public string CompletionDate { get; set; }

        [DisplayName("Description")]
        [CsvHelper.Configuration.Attributes.Name("Description")]
        public string Description { get; set; }

        [DisplayName("External Contacts")]
        [CsvHelper.Configuration.Attributes.Name("External Contacts")]
        public string ExternalContacts { get; set; }

        [DisplayName("Invoices total (before tax)")]
        [CsvHelper.Configuration.Attributes.Name("Invoices total (before tax)")]
        public decimal InvoicesPreTaxTotal { get; set; }

        [DisplayName("Invoices total")]
        [CsvHelper.Configuration.Attributes.Name("Invoices total")]
        public decimal InvoicesTotal { get; set; }

        public ManagementActivityOverviewReportModel(PimsManagementActivity activity)
        {
            ArgumentNullException.ThrowIfNull(activity, nameof(activity));

            ManagementFileName = GetNullableString(activity.ManagementFile?.FileName);
            LegacyFileNum = GetNullableString(activity.ManagementFile?.LegacyFileNum);
            Properties = GetPropertiesAsString(activity);
            Funding = GetNullableString(activity.ManagementFile?.AcquisitionFundingTypeCodeNavigation?.Description);
            Purpose = GetNullableString(activity.ManagementFile?.ManagementFilePurposeTypeCodeNavigation?.Description);
            CreatedBy = GetNullableString(activity.ManagementFile?.AppCreateUserid);
            PropertyContacts = GetPropertyContactsAsString(activity.ManagementFile?.PimsManagementFileContacts);
            ManagementFileStatus = GetNullableString(activity.ManagementFile?.ManagementFileStatusTypeCodeNavigation?.Description);
            ActivityStatus = GetNullableString(activity.MgmtActivityStatusTypeCodeNavigation?.Description);
            ActivityType = GetNullableString(activity.MgmtActivityTypeCodeNavigation?.Description);
            ActivitySubTypes = string.Join("|", activity.PimsMgmtActivityActivitySubtyps.Select(st => st.MgmtActivitySubtypeCodeNavigation.Description));
            ServicesProvider = activity.ServiceProviderPerson?.GetFullName() ?? GetNullableString(activity.ServiceProviderOrg?.Name);
            MinistryContacts = GetMinistyContactsAsString(activity.PimsMgmtActMinContacts);
            RequestAddedDate = GetNullableDate(activity.RequestAddedDt);
            CompletionDate = GetNullableDate(activity.CompletionDt);
            Description = GetNullableString(activity.Description);
            ExternalContacts = GetExternalContactsAsString(activity.PimsMgmtActInvolvedParties);
            InvoicesPreTaxTotal = activity.PimsManagementActivityInvoices?.Sum(i => i.PretaxAmt) ?? 0;
            InvoicesTotal = activity.PimsManagementActivityInvoices?.Sum(i => i.TotalAmt ?? 0) ?? 0;
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
            if (activity.ManagementFile is not null && activity.ManagementFile.PimsManagementFileProperties is not null)
            {
                return string.Join("|", activity.ManagementFile.PimsManagementFileProperties
                        .Where(fp => fp?.Property != null)
                        .Select(fp => fp.Property.GetPropertyName())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct());
            }
            else if (activity.PimsManagementActivityProperties is not null)
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

        private static string GetMinistyContactsAsString(ICollection<PimsMgmtActMinContact> ministryContacts)
        {
            if (ministryContacts is not null)
            {
                return string.Join("|", ministryContacts
                        .Select(c => GetNullableString(c?.Person?.GetFullName()))
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct());
            }
            return string.Empty;
        }

        private static string GetExternalContactsAsString(ICollection<PimsMgmtActInvolvedParty> externalContacts)
        {
            if (externalContacts is not null)
            {
                return string.Join("|", externalContacts
                        .Select(c => c?.Person?.GetFullName() ?? GetNullableString(c?.Organization?.Name))
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct());
            }
            return string.Empty;
        }
    }
}
