using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Role class, provides an entity for the datamodel to manage roles.
    /// </summary>
    public partial class PimsRole : IDisableBaseAppEntity
    {
        #region Properties
        public ICollection<PimsClaim> GetClaims() => PimsRoleClaims?.Select(c => c.Claim).ToArray();

        public ICollection<PimsUser> GetUsers() => PimsUserRoles?.Select(u => u.User).ToArray();

        [NotMapped]
        public long Id { get => RoleId; set => RoleId = value; }
        #endregion
        #region Constructors

        /// <summary>
        /// Create a new instance of a Role class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public PimsRole(Guid key, string name)
            : this()
        {
            this.RoleUid = key;
            this.Name = name;
            this.IsDisabled = false;
        }
        #endregion
    }
}
