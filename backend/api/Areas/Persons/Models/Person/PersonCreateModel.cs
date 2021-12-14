using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pims.Api.Areas.Persons.Models.Person
{
    public class PersonCreateModel
    {
        #region Properties

        /// <summary>
        /// get/set - The person's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The person's surname.
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The person's first name.
        /// </summary>
        [Required]
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
        /// get/set - The person's linked organization (optional).
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - The person's addresses.
        /// </summary>
        public IList<AddressCreateModel> Addresses { get; set; }

        /// <summary>
        /// get/set - The person's contact methods.
        /// </summary>
        public IList<Areas.Contact.Models.Contact.ContactMethodModel> ContactMethods { get; set; }

        /// <summary>
        /// get/set - The person's Comment.
        /// </summary>
        public string Comment { get; set; }
        #endregion
    }
}
