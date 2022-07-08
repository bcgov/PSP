namespace Pims.Api.Models.Contact
{
    /// <summary>
    /// Provides a contact-oriented contact method model.
    /// </summary>
    public class ContactMethodModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the contact method.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The optional key to the parent person of this contact method.
        /// </summary>
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The optional key to the parent organization of this contact method.
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - Foreign key to the contact method type.
        /// </summary>
        public TypeModel<string> ContactMethodTypeCode { get; set; }

        /// <summary>
        /// get/set - The contact method value.
        /// </summary>
        public string Value { get; set; }
        #endregion
    }
}
