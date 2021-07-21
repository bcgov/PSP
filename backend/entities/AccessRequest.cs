using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AccessRequest class, provides an entity for the datamodel to manage submitted access request forms for unauthorized users.
    /// </summary>
    [MotiTable("PIMS_ACCESS_REQUEST", "ACRQST")]
    public class AccessRequest : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        [Column("ACCESS_REQUEST_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to User
        /// </summary>
        [Column("USER_ID")]
        public long UserId { get; set; }

        /// <summary>
        /// get/set - the user originating this request
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to role.
        /// </summary>
        [Column("ROLE_ID")]
        public long RoleId { get; set; }

        /// <summary>
        /// get/set - the role the user is requesting.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// get - Foreign key to the status.
        /// </summary>
        [Column("ACCESS_REQUEST_STATUS_TYPE_CODE")]
        public string StatusId { get; set; }

        /// <summary>
        /// get - The access request status.
        /// </summary>
        public AccessRequestStatusType Status { get; set; }

        /// <summary>
        /// get - the list of organizations that the user is requesting to be added to.
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to organizations.
        /// </summary>
        public ICollection<AccessRequestOrganization> OrganizationsManyToMany { get; } = new List<AccessRequestOrganization>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a AccessRequest class.
        /// </summary>
        public AccessRequest() { }

        /// <summary>
        /// Create a new instance of a AccessRequest class.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <param name="status"></param>
        public AccessRequest(User user, Role role, AccessRequestStatusType status)
        {
            this.User = user ?? throw new ArgumentNullException(nameof(user));
            this.UserId = user.Id;
            this.Role = role ?? throw new ArgumentNullException(nameof(role));
            this.RoleId = role.Id;
            this.Status = status ?? throw new ArgumentNullException(nameof(status));
            this.StatusId = status.Id;
        }
        #endregion
    }
}
