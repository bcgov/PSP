namespace Pims.Api.Areas.Organizations.Models.Organization
{
    /// <summary>
    /// Provides a contact-oriented address model.
    /// </summary>
    public class OrganizationAddressModel : Pims.Api.Models.Contact.AddressModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the Organization-address relationship.
        /// </summary>
        public long OrganizationAddressId { get; set; }

        /// <summary>
        /// get/set - The concurrency row version of the Organization-address relationship.
        /// </summary>
        public long OrganizationAddressRowVersion { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the Organization that owns the target address.
        /// </summary>
        public long OrganizationId { get; set; }
        #endregion
    }
}
