using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Pims.Api.Models;
using Pims.Api.Models.Contact;

namespace Pims.Api.Areas.Organizations.Models.Organization
{
    /// <summary>
    /// Provides a contact-oriented organization model.
    /// </summary>
    public class OrganizationModel : BaseAppModel
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
        [Required]
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
        /// get/set - The organization's persons. This collection is read-only
        /// Changing this collection will NOT modify the organization's persons.
        /// </summary>
        public IList<Pims.Api.Areas.Persons.Models.Person.PersonModel> Persons { get; set; }

        /// <summary>
        /// get/set - The organization's addresses.
        /// </summary>
        public IList<OrganizationAddressModel> Addresses { get; set; }

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
