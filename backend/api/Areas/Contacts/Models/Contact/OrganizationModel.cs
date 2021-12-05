namespace Pims.Api.Areas.Contact.Models.Contact
{
    /// <summary>
    /// Provides a contact-oriented organization model.
    /// </summary>
    public class OrganizationModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the organization.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization's company name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The organization's primary contact name.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// get/set - The organizations's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - The organizations's mobile phone number.
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// get/set - The organizations's landline phone number.
        /// </summary>
        public string Landline { get; set; }

        /// <summary>
        /// get/set - The person's address.
        /// </summary>
        public AddressModel Address { get; set; }
        #endregion
    }
}
