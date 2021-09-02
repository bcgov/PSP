using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// RoleClaim class, provides an entity for the datamodel to manage role organizations.
    /// </summary>
    [MotiTable("PIMS_ROLE_CLAIM", "ROLCLM")]
    public class RoleClaim : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify this role claim.
        /// </summary>
        [Column("ROLE_CLAIM_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the role - PRIMARY KEY.
        /// </summary>
        [Column("ROLE_ID")]
        public long RoleId { get; set; }

        /// <summary>
        /// get/set - The role that belongs to this claim.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// get/set - The foreign key to the claim the role belongs to - PRIMARY KEY.
        /// </summary>
        [Column("CLAIM_ID")]
        public long ClaimId { get; set; }

        /// <summary>
        /// get/set - The claim the role belongs to.
        /// </summary>
        public Claim Claim { get; set; }

        /// <summary>
        /// get/set - Whether this role claim is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a RoleClaim class.
        /// </summary>
        public RoleClaim() { }

        /// <summary>
        /// Create a new instance of a RoleClaim class.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="claimId"></param>
        public RoleClaim(int roleId, int claimId)
        {
            this.RoleId = roleId;
            this.ClaimId = claimId;
        }

        /// <summary>
        /// Create a new instance of a RoleClaim class.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        public RoleClaim(Role role, Claim claim)
        {
            this.Role = role;
            this.RoleId = role?.Id ??
                throw new ArgumentNullException(nameof(role));
            this.Claim = claim;
            this.ClaimId = claim?.Id ??
                throw new ArgumentNullException(nameof(claim));
        }
        #endregion
    }
}
