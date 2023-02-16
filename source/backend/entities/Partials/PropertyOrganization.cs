using System;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyOrganization class, provides an entity for the datamodel to manage property organizations.
    /// </summary>
    public partial class PimsPropertyOrganization : IDisableBaseAppEntity
    {
        #region Constructors
        public PimsPropertyOrganization()
        {
        }

        /// <summary>
        /// Create a new instance of a PropertyOrganization class.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="organization"></param>
        public PimsPropertyOrganization(PimsProperty property, PimsOrganization organization)
            : this()
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.PropertyId = property.PropertyId;
            this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            this.OrganizationId = organization.Internal_Id;
        }
        #endregion
    }
}
