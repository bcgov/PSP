using System;

namespace Pims.Api.Areas.Admin.Models.User
{
    /// <summary>
    /// RoleModel class, provides a model that represents a role.
    /// </summary>
    public class RoleModel : Api.Models.BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to role.
        /// </summary>
        public long Id { get; set; }

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
        #endregion
    }
}
