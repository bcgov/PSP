using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class PersonModel : BaseAppModel
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
        public IList<OrganizationModel> Organizations { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the person-organization link (optional).
        /// </summary>
        public long? PersonOrganizationId { get; set; }

        /// <summary>
        /// get/set - The concurrency row version for the person-organization link (optional).
        /// </summary>
        /// <value></value>
        public long? PersonOrganizationRowVersion { get; set; }

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
