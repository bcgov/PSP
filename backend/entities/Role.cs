using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Role class, provides an entity for the datamodel to manage roles.
    /// </summary>
    [MotiTable("PIMS_ROLE", "ROLE")]
    public class Role : BaseAppEntity
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
        /// get/set - The name of the role.
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the type.
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
        /// get/set - Whether this code is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The sort order of the lookup item.
        /// </summary>
        [Column("SORT_ORDER")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// get - A collection of users that have this role.
        /// </summary>
        public ICollection<User> Users { get; } = new List<User>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to users.
        /// </summary>
        public ICollection<UserRole> UsersManyToMany { get; } = new List<UserRole>();

        /// <summary>
        /// get - A collection of user organizations.
        /// </summary>
        public ICollection<UserOrganization> OrganizationsManyToMany { get; } = new List<UserOrganization>();

        /// <summary>
        /// get - A collection of claims for this role.
        /// </summary>
        public ICollection<Claim> Claims { get; } = new List<Claim>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to claims.
        /// </summary>
        public ICollection<RoleClaim> ClaimsManyToMany { get; } = new List<RoleClaim>();

        /// <summary>
        /// get - A collection of access requests for this role.
        /// </summary>
        public ICollection<AccessRequest> AccessRequests { get; } = new List<AccessRequest>();
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
