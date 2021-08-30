using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Claim class, provides an entity for the datamodel to manage claims.
    /// </summary>
    [MotiTable("PIMS_CLAIM", "CLMTYP")]
    public class Claim : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        [Column("CLAIM_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this claim.
        /// </summary>
        [Column("CLAIM_UID")]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The claims display name.
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - The GUID that identifies this Role in Keycloak.
        /// </summary>
        [Column("KEYCLOAK_ROLE_ID")]
        public Guid? KeycloakRoleId { get; set; }

        /// <summary>
        /// get/set - The claims first name.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether the claim is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get - A collection of roles that have this claim.
        /// </summary>
        public ICollection<Role> Roles { get; } = new List<Role>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to roles.
        /// </summary>
        public ICollection<RoleClaim> RolesManyToMany { get; } = new List<RoleClaim>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Claim class.
        /// </summary>
        public Claim() { }

        /// <summary>
        /// Create a new instance of a Claim class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public Claim(Guid key, string name)
        {
            this.Key = key;
            this.Name = name;
        }
        #endregion
    }
}
