using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class PropertyActivityModel : BaseAppModel
    {
        #region Properties

        public long Id { get; set; }

        public TypeModel<string> ActivityTypeCode { get; set; }

        public TypeModel<string> ActivitySubtypeCode { get; set; }

        public TypeModel<string> ActivityStatusTypeCode { get; set; }

        public DateTime RequestAddedDateTime { get; set; }

        public DateTime? CompletionDateTime { get; set; }

        public string Description { get; set; }

        public string RequestSource { get; set; }

        public decimal? PretaxAmt { get; set; }

        public decimal? GstAmt { get; set; }

        public decimal? PstAmt { get; set; }

        public decimal? TotalAmt { get; set; }

        public bool? IsDisabled { get; set; }

        public long? ServiceProviderOrgId { get; set; }

        public OrganizationModel ServiceProviderOrg { get; set; }

        public long? ServiceProviderPersonId { get; set; }

        public PersonModel ServiceProviderPerson { get; set; }

        public IList<PropertyActivityInvolvedPartyModel> InvolvedParties { get; set; }

        public IList<PropertyMinistryContactModel> MinistryContacts { get; set; }

        public IList<PropertyActivityPropertyModel> ActivityProperties { get; set; }

        public IList<PropertyActivityInvoiceModel> Invoices { get; set; }

        #endregion
    }
}
