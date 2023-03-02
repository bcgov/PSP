using System;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// UserOrganization class, provides an entity for the datamodel to manage user organizations.
    /// </summary>
    public partial class PimsUserOrganization : IDisableBaseAppEntity
    {
        public PimsUserOrganization()
        {
        }
        #region Constructors

        /// <summary>
        /// Create a new instance of a UserOrganization class.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationId"></param>
        /// <param name="roleId"></param>
        public PimsUserOrganization(long userId, long organizationId, long roleId)
        {
            this.UserId = userId;
            this.OrganizationId = organizationId;
            this.RoleId = roleId;
        }

        /// <summary>
        /// Create a new instance of a UserOrganization class.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="organization"></param>
        /// <param name="role"></param>
        public PimsUserOrganization(PimsUser user, PimsOrganization organization, PimsRole role)
        {
            this.User = user ?? throw new ArgumentNullException(nameof(user));
            this.UserId = user.Internal_Id;
            this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            this.OrganizationId = organization.Internal_Id;
            this.Role = role ?? throw new ArgumentNullException(nameof(role));
            this.RoleId = role.RoleId;
        }
        #endregion
    }
}
