using System.Collections.Generic;

namespace Pims.Api.Areas.Contact.Models.Contact
{
    public class PersonModel
    {
        #region Properties

        /// <summary>
        /// get/set - The person's id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The person's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The person's concatenated full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// get/set - The person's preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// get/set - The person's organizations.
        /// </summary>
        public IList<OrganizationModel> Organizations { get; set; }

        /// <summary>
        /// get/set - The person's addresses.
        /// </summary>
        public IList<AddressModel> Addresses { get; set; }

        /// <summary>
        /// get/set - The person's contact methods.
        /// </summary>
        public IList<ContactMethodModel> ContactMethods { get; set; }

        /// <summary>
        /// get/set - The person's Comment.
        /// </summary>
        public string Comment { get; set; }
        #endregion
    }
}
