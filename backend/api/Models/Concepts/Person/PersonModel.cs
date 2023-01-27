using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class PersonModel : BaseModel
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
        /// get/set - The person's surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The person's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The person's middle names.
        /// </summary>
        public string MiddleNames { get; set; }

        /// <summary>
        /// get/set - The person's preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// get/set - The person's organizations.
        /// </summary>
        public IList<PersonOrganizationModel> PersonOrganizations { get; set; }

        /// <summary>
        /// get/set - The person's addresses.
        /// </summary>
        public IList<PersonAddressModel> PersonAddresses { get; set; }

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
