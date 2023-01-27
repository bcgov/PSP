namespace Pims.Api.Areas.Contact.Models.Contact
{
    public class ContactModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the organization or person.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// get/set - Detailed information if the contact is a person. Null if the contact is an organization.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - Detailed information if the contact is an organization. Null if the contact is a person.
        /// </summary>
        public OrganizationModel Organization { get; set; }
        #endregion
    }
}
