using System;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// RoleClaim class, provides an entity for the datamodel to manage role organizations.
    /// </summary>
    public partial class PimsRoleClaim : IDisableBaseAppEntity
    {
        #region Constructors
        public PimsRoleClaim()
        {
        }

        /// <summary>
        /// Create a new instance of a RoleClaim class.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="claimId"></param>
        public PimsRoleClaim(int roleId, int claimId)
        {
            this.RoleId = roleId;
            this.ClaimId = claimId;
        }

        /// <summary>
        /// Create a new instance of a RoleClaim class.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        public PimsRoleClaim(PimsRole role, PimsClaim claim)
        {
            this.Role = role;
            this.RoleId = role?.RoleId ??
                throw new ArgumentNullException(nameof(role));
            this.Claim = claim;
            this.ClaimId = claim?.ClaimId ??
                throw new ArgumentNullException(nameof(claim));
        }
        #endregion
    }
}
