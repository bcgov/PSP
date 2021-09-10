using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Organization class, provides an entity for the datamodel to manage property organizations.
    /// </summary>
    [MotiTable("PIMS_ORGANIZATION", "ORG")]
    public class Organization : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify organization.
        /// </summary>
        [Column("ORGANIZATION_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the parent organization.
        /// </summary>
        [Column("PRNT_ORGANIZATION_ID")]
        public long? ParentId { get; set; }

        /// <summary>
        /// get/set - The parent organization this organization belongs to.
        /// </summary>
        public Organization Parent { get; set; }

        /// <summary>
        /// get/set - Foreign key to the address of the organization.
        /// </summary>
        [Column("ADDRESS_ID")]
        public long AddressId { get; set; }

        /// <summary>
        /// get/set - The organization address.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// get/set - Foreign key to the region.
        /// </summary>
        [Column("REGION_CODE")]
        public int? RegionId { get; set; }

        /// <summary>
        /// get/set - The region.
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// get/set - Foreign key to the district.
        /// </summary>
        [Column("DISTRICT_CODE")]
        public int? DistrictId { get; set; }

        /// <summary>
        /// get/set - The district.
        /// </summary>
        public District District { get; set; }

        /// <summary>
        /// get/set - Foreign key to the organization type.
        /// </summary>
        [Column("ORGANIZATION_TYPE_CODE")]
        public string OrganizationTypeId { get; set; }

        /// <summary>
        /// get/set - The organization type.
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the organization identifier type.
        /// </summary>
        [Column("ORG_IDENTIFIER_TYPE_CODE")]
        public string OrganizationIdentifierTypeId { get; set; }

        /// <summary>
        /// get/set - The organization identifier type.
        /// </summary>
        public OrganizationIdentifierType OrganizationIdentifierType { get; set; }

        /// <summary>
        /// get/set - The organization identifier.
        /// </summary>
        [Column("ORGANIZATION_IDENTIFIER")]
        public string Identifier { get; set; }

        /// <summary>
        /// get/set - The organization name.
        /// </summary>
        [Column("ORGANIZATION_NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - The organization website.
        /// </summary>
        [Column("WEBSITE")]
        public string Website { get; set; }

        /// <summary>
        /// get/set - Whether this organization is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get - A collection of child organizations.
        /// </summary>
        public ICollection<Organization> Children { get; } = new List<Organization>();

        /// <summary>
        /// get - A collection of persons that belong to this organization.
        /// </summary>
        public ICollection<Person> Persons { get; } = new List<Person>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to users.
        /// </summary>
        public ICollection<PersonOrganization> PersonsManyToMany { get; } = new List<PersonOrganization>();

        /// <summary>
        /// get - A collection of contact methods associated with this organization.
        /// </summary>
        public ICollection<ContactMethod> ContactMethods { get; } = new List<ContactMethod>();

        /// <summary>
        /// get - A collection of users that belong to this organization.
        /// </summary>
        public ICollection<User> Users { get; } = new List<User>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to users.
        /// </summary>
        public ICollection<UserOrganization> UsersManyToMany { get; } = new List<UserOrganization>();

        /// <summary>
        /// get - A collection of access requests that belong to this organization.
        /// </summary>
        public ICollection<AccessRequest> AccessRequests { get; } = new List<AccessRequest>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to access requests.
        /// </summary>
        public ICollection<AccessRequestOrganization> AccessRequestsManyToMany { get; } = new List<AccessRequestOrganization>();

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();

        /// <summary>
        /// get - Collection of many-to-many properties.
        /// </summary>
        public ICollection<PropertyOrganization> PropertiesManyToMany { get; } = new List<PropertyOrganization>();

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Organization class.
        /// </summary>
        public Organization() { }

        /// <summary>
        /// Create a new instance of a Organization class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="identifierType"></param>
        /// <param name="address"></param>
        public Organization(string name, OrganizationType type, OrganizationIdentifierType identifierType, Address address)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"Argument '{nameof(name)}' is required.", nameof(name));

            this.Name = name;
            this.OrganizationType = type ?? throw new ArgumentNullException(nameof(type));
            this.OrganizationTypeId = type.Id;
            this.OrganizationIdentifierType = identifierType ?? throw new ArgumentNullException(nameof(identifierType));
            this.OrganizationIdentifierTypeId = identifierType.Id;
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
            this.AddressId = address.Id;
            this.Region = address.Region;
            this.RegionId = address.RegionId;
            this.District = address.District;
            this.DistrictId = address.DistrictId;
        }
        #endregion
    }
}
