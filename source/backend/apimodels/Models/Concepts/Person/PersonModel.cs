using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.ContactMethod;

namespace Pims.Api.Models.Concepts.Person
{
    public class PersonModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The person's id.
        /// </summary>
        public long Id { get; set; }

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
        /// get/set - The person's name sufix.
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// get/set - The person's preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// get/set - The person's birth date.
        /// </summary>
        public DateOnly? BirthDate { get; set; }

        /// <summary>
        /// get/set - The person's Comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// get/set - The person's address comment.
        /// </summary>
        public string AddressComment { get; set; }

        /// <summary>
        /// get/set - Flag to note to use the organization address.
        /// </summary>
        public bool? UseOrganizationAddress { get; set; }

        /// <summary>
        /// get/set - The person's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The person's property activity id.
        /// </summary>
        public long? ManagementActivityId { get; set; }

        /// <summary>
        /// get/set - The person's contact methods.
        /// </summary>
        public IList<ContactMethodModel> ContactMethods { get; set; }

        /// <summary>
        /// get/set - The person's addresses.
        /// </summary>
        public IList<PersonAddressModel> PersonAddresses { get; set; }

        /// <summary>
        /// get/set - The person's organizations.
        /// </summary>
        public IList<PersonOrganizationModel> PersonOrganizations { get; set; }

        #endregion
    }
}
