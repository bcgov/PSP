using System;
using System.Collections.Generic;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AccessRequest class, provides an entity for the datamodel to manage submitted access request forms for unauthorized users.
    /// </summary>
    public partial class PimsAccessRequest : IBaseAppEntity
    {
        #region Properties
        public ICollection<PimsOrganization> GetOrganizations() => PimsAccessRequestOrganizations?.Select(o => o.Organization).ToArray();
        #endregion
        #region Constructors

        /// <summary>
        /// Create a new instance of a AccessRequest class.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <param name="status"></param>
        public PimsAccessRequest(PimsUser user, PimsRole role, PimsAccessRequestStatusType status)
            : this()
        {
            this.User = user ?? throw new ArgumentNullException(nameof(user));
            this.UserId = user.Internal_Id;
            this.Role = role ?? throw new ArgumentNullException(nameof(role));
            this.RoleId = role.RoleId;
            this.AccessRequestStatusTypeCodeNavigation = status ?? throw new ArgumentNullException(nameof(status));
            this.AccessRequestStatusTypeCode = status.Id;
        }
        #endregion
    }
}
