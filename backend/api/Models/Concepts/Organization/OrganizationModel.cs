using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// Provides a contact-oriented organization model.
    /// </summary>
    public class OrganizationModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The organization's id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The organization's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The organization's alias name.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// get/set - The organization's incorporation number.
        /// </summary>
        public string IncorporationNumber { get; set; }

        /// <summary>
        /// get/set - The organization and person relationships.
        /// </summary>
        public IList<OrganizationPersonModel> OrganizationPersons { get; set; }

        /// <summary>
        /// get/set - The organization's addresses.
        /// </summary>
        public IList<OrganizationAddressModel> OrganizationAddresses { get; set; }

        /// <summary>
        /// get/set - The organization's contact methods.
        /// </summary>
        public IList<ContactMethodModel> ContactMethods { get; set; }

        /// <summary>
        /// get/set - The organization's Comment.
        /// </summary>
        public string Comment { get; set; }
        #endregion
    }
}
