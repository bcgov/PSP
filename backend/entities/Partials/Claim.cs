using System;
using System.Collections.Generic;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Claim class, provides an entity for the datamodel to manage claims.
    /// </summary>
    public partial class PimsClaim : IDisableBaseAppEntity
    {
        #region Properties
        public ICollection<PimsRole> GetRoles() => PimsRoleClaims?.Select(c => c.Role).ToArray();
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a Claim class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public PimsClaim(Guid key, string name)
            : this()
        {
            this.ClaimUid = key;
            this.Name = name;
        }
        #endregion
    }
}
