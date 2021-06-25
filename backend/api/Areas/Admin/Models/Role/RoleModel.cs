using System;
using System.Collections.Generic;

namespace Pims.Api.Areas.Admin.Models.Role
{
    /// <summary>
    /// RoleModel class, provides a model that represents a role.
    /// </summary>
    public class RoleModel : Api.Models.BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify role.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the role.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The keycloak group id.
        /// </summary>
        public Guid? KeycloakGroupId { get; set; }

        /// <summary>
        /// get/set - The item's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The role description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether the role is public.
        /// One which users can request to join.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// get/set - Whether the item is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The item's sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// get/set - An array of claims associated to this role.
        /// </summary>
        public IEnumerable<ClaimModel> Claims { get; set; }
        #endregion
    }
}
