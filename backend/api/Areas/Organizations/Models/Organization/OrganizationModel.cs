using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Pims.Api.Models.Contact;

namespace Pims.Api.Areas.Organizations.Models.Organization
{
    /// <summary>
    /// Provides a contact-oriented organization model.
    /// </summary>
    public class OrganizationModel
    {
        #region Properties
        /// <summary>
        /// get/set - The organization's id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        /// <value></value>
        public long RowVersion { get; set; }

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
        /// get/set - The organization's addresses.
        /// </summary>
        public IList<AddressModel> Addresses { get; set; }

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
