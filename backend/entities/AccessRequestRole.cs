using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AccessRequestRole class, provides an entity for the datamodel to manage AccessRequest Roles.
    /// </summary>
    [MotiTable("PIMS_ACCESS_REQUEST_ROLE", "ACCRQR")]
    public class AccessRequestRole : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the access request role.
        /// </summary>
        [Column("ACCESS_REQUEST_ROLE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the AccessRequest - PRIMARY KEY.
        /// </summary>
        [Column("ACCESS_REQUEST_ID")]
        public long AccessRequestId { get; set; }

        /// <summary>
        /// get/set - The access request that belongs to an Role.
        /// </summary>
        public AccessRequest AccessRequest { get; set; }

        /// <summary>
        /// get/set - The foreign key to the role the Role belongs to - PRIMARY KEY.
        /// </summary>
        [Column("ROLE_ID")]
        public long RoleId { get; set; }

        /// <summary>
        /// get/set - The Role the AccessRequest belongs to.
        /// </summary>
        public Role Role { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a AccessRequestRole class.
        /// </summary>
        public AccessRequestRole() { }

        /// <summary>
        /// Create a new instance of a AccessRequestRole class.
        /// </summary>
        /// <param name="accessRequestId"></param>
        /// <param name="roleId"></param>
        public AccessRequestRole(long accessRequestId, long roleId)
        {
            this.AccessRequestId = accessRequestId;
            this.RoleId = roleId;
        }

        /// <summary>
        /// Create a new instance of a AccessRequestRole class.
        /// </summary>
        /// <param name="accessRequest"></param>
        /// <param name="role"></param>
        public AccessRequestRole(AccessRequest accessRequest, Role role)
        {
            this.AccessRequest = accessRequest;
            this.AccessRequestId = accessRequest?.Id ??
                throw new ArgumentNullException(nameof(accessRequest));
            this.Role = role;
            this.RoleId = role?.Id ??
                throw new ArgumentNullException(nameof(role));
        }
        #endregion
    }
}
