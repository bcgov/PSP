using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyOrganization class, provides an entity for the datamodel to manage property organizations.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_ORGANIZATION", "PRPORG")]
    public class PropertyOrganization : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the user organization.
        /// </summary>
        [Column("PROPERTY_ORGANIZATION_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the property.
        /// </summary>
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - The property.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// get/set - The foreign key to the organization.
        /// </summary>
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// get/set - Whether this user organization is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyOrganization class.
        /// </summary>
        public PropertyOrganization() { }

        /// <summary>
        /// Create a new instance of a PropertyOrganization class.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="organization"></param>
        public PropertyOrganization(Property property, Organization organization)
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.PropertyId = property.Id;
            this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            this.OrganizationId = organization.Id;
        }
        #endregion
    }
}
