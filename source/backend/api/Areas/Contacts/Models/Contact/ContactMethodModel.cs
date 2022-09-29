using Pims.Api.Models;

namespace Pims.Api.Areas.Contact.Models.Contact
{
    /// <summary>
    /// Provides a contact-oriented contact method model.
    /// </summary>
    public class ContactMethodModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the contact method.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - The contact method type.
        /// </summary>
        public TypeModel<string> ContactMethodType { get; set; }

        /// <summary>
        /// get/set - The contact method value.
        /// </summary>
        public string Value { get; set; }
        #endregion
    }
}
