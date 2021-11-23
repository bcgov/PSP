using System;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// UserRole class, provides an entity for the datamodel to manage user organizations.
    /// </summary>
    public partial class PimsUserRole : IBaseAppEntity
    {
        #region Constructors
        public PimsUserRole() { }

        /// <summary>
        /// Create a new instance of a UserRole class.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        public PimsUserRole(long userId, long roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }

        /// <summary>
        /// Create a new instance of a UserRole class.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        public PimsUserRole(PimsUser user, PimsRole role)
        {
            this.User = user;
            this.UserId = user?.Id ??
                throw new ArgumentNullException(nameof(user));
            this.Role = role;
            this.RoleId = role?.RoleId ??
                throw new ArgumentNullException(nameof(role));
        }
        #endregion
    }
}
