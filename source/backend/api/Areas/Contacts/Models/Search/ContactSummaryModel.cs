using Pims.Api.Areas.Contact.Models.Contact;

namespace Pims.Api.Areas.Contact.Models.Search
{
    public class ContactSummaryModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the organization or person.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the person.
        /// </summary>
        public long? PersonId { get; set; }

        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the organization.
        /// </summary>
        public long? OrganizationId { get; set; }

        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - Either the person name or the organization name.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// get/set - The person's surname (optional).
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The person's first name (optional).
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The person's middle names (optional).
        /// </summary>
        public string MiddleNames { get; set; }

        /// <summary>
        /// get/set - The organization.
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// get/set - The contact's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - The mailing address, if one exists, of the person or organization.
        /// </summary>
        public string MailingAddress { get; set; }

        /// <summary>
        /// get/set - The municipality(city) of this contact.
        /// </summary>
        public string MunicipalityName { get; set; }

        /// <summary>
        /// get/set - The province or state, of the person or organization.
        /// </summary>
        public string ProvinceState { get; set; }

        /// <summary>
        /// get/set - If this contact is disabled and should be hidden by default.
        /// </summary>
        public bool IsDisabled { get; set; }
        #endregion
    }
}
