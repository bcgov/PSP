using Pims.Api.Models.Concepts;
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
        /// get/set - The person's surname (optional)
        /// </summary>
        /// <value></value>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The person's first name (optional)
        /// </summary>
        /// <value></value>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The person's middle names (optional)
        /// </summary>
        /// <value></value>
        public string MiddleNames { get; set; }

        /// <summary>
        /// get/set - The organization
        /// </summary>
        /// <value></value>
        public string OrganizationName { get; set; }

        /// <summary>
        /// get/set - The contact's email
        /// </summary>
        /// <value></value>
        public string Email { get; set; }

        /// <summary>
        /// get/set - The mailing address, if one exists, of the person or organization.
        /// </summary>
        /// <value></value>
        public string MailingAddress { get; set; }

        /// <summary>
        /// get/set - The municipality(city) of this contact.
        /// </summary>
        /// <value></value>
        public string MunicipalityName { get; set; }

        /// <summary>
        /// get/set - The province or state, of the person or organization.
        /// </summary>
        /// <value></value>
        public string ProvinceState { get; set; }

        /// <summary>
        /// get/set - If this contact is disabled and should be hidden by default.
        /// </summary>
        /// <value></value>
        public bool IsDisabled { get; set; }
        #endregion
    }
}
