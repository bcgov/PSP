using System;
using System.ComponentModel;

namespace Pims.Api.Areas.Reports.Models.Management
{
    public class ManagementActivityReportModel
    {

        [DisplayName("Management File Name")]
        [CsvHelper.Configuration.Attributes.Name("Management File Name")]
        public string ManagementFileName { get; set; }

        [DisplayName("Management File Number")]
        [CsvHelper.Configuration.Attributes.Name("Management File Number")]
        public string LegacyFileNum { get; set; }

        [DisplayName("Type")]
        [CsvHelper.Configuration.Attributes.Name("Type")]
        public string ActivityType { get; set; }

        [DisplayName("Sub-Types")]
        [CsvHelper.Configuration.Attributes.Name("Sub-Types")]
        public string ActivitySubTypes { get; set; }

        [DisplayName("Status")]
        [CsvHelper.Configuration.Attributes.Name("Status")]
        public string ActivityStatusType { get; set; }

        [DisplayName("Request Added Date")]
        [CsvHelper.Configuration.Attributes.Name("Request Added Date")]
        public DateOnly RequestAddedDateOnly { get; set; }

        [DisplayName("Completion Date")]
        [CsvHelper.Configuration.Attributes.Name("Completion Date")]
        public DateOnly? CompletionDateOnly { get; set; }

        [DisplayName("Description")]
        [CsvHelper.Configuration.Attributes.Name("Description")]
        public string Description { get; set; }
    }
}
