using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Role class, provides an entity for the datamodel to manage roles.
    /// </summary>
    [MotiTable("PIMS_ROLE", "ROLE")]
    public class Role : LookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify role.
        /// </summary>
        [Column("ROLE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique id to identify the role.
        /// </summary>
        [Column("ROLE_UID")]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The GUID that identifies this Group in Keycloak.
        /// </summary>
        [Column("KEYCLOAK_GROUP_ID")]
        public Guid? KeycloakGroupId { get; set; }

        /// <summary>
        /// get/set - The roles first name.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether the role is a public role.
        /// One which new users can request to join.
        /// </summary>
        [Column("IS_PUBLIC")]
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// get - A collection of users that have this role.
        /// </summary>
        public ICollection<UserRole> Users { get; } = new List<UserRole>();

        /// <summary>
        /// get - A collection of claims for this role.
        /// </summary>
        public ICollection<RoleClaim> Claims { get; } = new List<RoleClaim>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Role class.
        /// </summary>
        public Role() { }

        /// <summary>
        /// Create a new instance of a Role class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public Role(Guid key, string name)
        {
            this.Key = key;
            this.Name = name;
        }
        #endregion
    }
}
