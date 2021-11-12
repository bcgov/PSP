namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// Provides a lease-oriented person model.
    /// </summary>
    public class PersonModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the person.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The person's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The person's middle name(s).
        /// </summary>
        public string MiddleNames { get; set; }

        /// <summary>
        /// get/set - The person's last name.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The person's concatenated full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// get/set - The person's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - The person's mobile phone number.
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// get/set - The person's landline phone number.
        /// </summary>
        public string Landline { get; set; }

        /// <summary>
        /// get/set - The person's address.
        /// </summary>
        public AddressModel Address { get; set; }
        #endregion
    }
}
