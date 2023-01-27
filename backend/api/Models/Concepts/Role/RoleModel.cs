using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class RoleModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to role.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The keycloak group id.
        /// </summary>
        public Guid? RoleUid { get; set; }

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
        public int DisplayOrder { get; set; }

        /// <summary>
        /// get/set - The claims associated to this role.
        /// </summary>
        public IEnumerable<RoleClaimModel> RoleClaims { get; set; }
        #endregion
    }
}
