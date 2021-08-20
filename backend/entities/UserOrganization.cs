using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// UserOrganization class, provides an entity for the datamodel to manage user organizations.
    /// </summary>
    [MotiTable("PIMS_USER_ORGANIZATION", "USRORG")]
    public class UserOrganization : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the user organization.
        /// </summary>
        [Column("USER_ORGANIZATION_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the user - PRIMARY KEY.
        /// </summary>
        [Column("USER_ID")]
        public long UserId { get; set; }

        /// <summary>
        /// get/set - The user that belongs to this organization.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - The foreign key to the organization the user belongs to - PRIMARY KEY.
        /// </summary>
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization the user belongs to.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// get/set - The foreign key to the role.
        /// </summary>
        [Column("ROLE_ID")]
        public long RoleId { get; set; } = 5; // this is the default value for the undefined role.

        /// <summary>
        /// get/set - The role.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// get/set - Whether this user organization is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a UserOrganization class.
        /// </summary>
        public UserOrganization() { }

        /// <summary>
        /// Create a new instance of a UserOrganization class.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationId"></param>
        /// <param name="roleId"></param>
        public UserOrganization(long userId, long organizationId, long? roleId)
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
        public UserOrganization(User user, Organization organization, Role role)
        {
            this.User = user ?? throw new ArgumentNullException(nameof(user));
            this.UserId = user.Id;
            this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            this.OrganizationId = organization.Id;
            this.Role = role ?? throw new ArgumentNullException(nameof(role));
            this.RoleId = role.Id;
        }
        #endregion
    }
}
