using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.ContactMethod;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.Organization
{
    /// <summary>
    /// Provides a contact-oriented organization model.
    /// </summary>
    public class OrganizationModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The organization's id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization's parent organization id.
        /// </summary>
        public long? ParentOrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization's reion code.
        /// </summary>
        public short? RegionCode { get; set; }

        /// <summary>
        /// get/set - The organization's district code.
        /// </summary>
        public short? DistrictCode { get; set; }

        /// <summary>
        /// get/set - The organization's type code.
        /// </summary>
        public string OrganizationTypeCode { get; set; }

        /// <summary>
        /// get/set - The organization's identifier code.
        /// </summary>
        public string IdentifierTypeCode { get; set; }

        /// <summary>
        /// get/set - The organization's identifier.
        /// </summary>
        public string OrganizationIdentifier { get; set; }

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
        /// get/set - The organization's website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// get/set - The organization's Comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// get/set - The organization's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The organization's contact methods.
        /// </summary>
        public IList<ContactMethodModel> ContactMethods { get; set; }

        /// <summary>
        /// get/set - The organization's addresses.
        /// </summary>
        public IList<OrganizationAddressModel> OrganizationAddresses { get; set; }

        /// <summary>
        /// get/set - The organization and person relationships.
        /// </summary>
        public IList<PersonOrganizationModel> OrganizationPersons { get; set; }

        /// <summary>
        /// get/set - The organization's parent organization.
        /// </summary>
        public OrganizationModel ParentOrganization { get; set; }

        #endregion
    }
}
