using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.ManagementActivity;
using Pims.Api.Models.Concepts.ManagementFile;
using Pims.Api.Models.Concepts.Organization;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.Property
{
    public class ManagementActivityModel : BaseAuditModel
    {
        #region Properties

        public long Id { get; set; }

        public long? ManagementFileId { get; set; }

        public ManagementFileModel ManagementFile { get; set; }

        public CodeTypeModel<string> ActivityTypeCode { get; set; }

        public IList<ManagementActivitySubTypeModel> ActivitySubTypeCodes { get; set; }

        public CodeTypeModel<string> ActivityStatusTypeCode { get; set; }

        public DateOnly RequestAddedDateOnly { get; set; }

        public DateOnly? CompletionDateOnly { get; set; }

        public string Description { get; set; }

        public string RequestSource { get; set; }

        public bool? IsDisabled { get; set; }

        public long? ServiceProviderOrgId { get; set; }

        public OrganizationModel ServiceProviderOrg { get; set; }

        public long? ServiceProviderPersonId { get; set; }

        public PersonModel ServiceProviderPerson { get; set; }

        public IList<ManagementActivityInvolvedPartyModel> InvolvedParties { get; set; }

        public IList<PropertyMinistryContactModel> MinistryContacts { get; set; }

        public IList<ManagementActivityPropertyModel> ActivityProperties { get; set; }

        public IList<ManagementActivityInvoiceModel> Invoices { get; set; }

        #endregion
    }
}
