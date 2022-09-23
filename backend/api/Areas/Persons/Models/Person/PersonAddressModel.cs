namespace Pims.Api.Areas.Persons.Models.Person
{
    /// <summary>
    /// Provides a contact-oriented address model.
    /// </summary>
    public class PersonAddressModel : Pims.Api.Models.Contact.AddressModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the person-address relationship.
        /// </summary>
        public long PersonAddressId { get; set; }

        /// <summary>
        /// get/set - The concurrency row version of the person-address relationship.
        /// </summary>
        public long PersonAddressRowVersion { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the person that owns the target address.
        /// </summary>
        public long PersonId { get; set; }
        #endregion
    }
}
