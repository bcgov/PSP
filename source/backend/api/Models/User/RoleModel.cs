using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.User
{
    public class RoleModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify the role.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the role.
        /// </summary>
        public Guid KeycloakGroupId { get; set; }

        /// <summary>
        /// get/set - The role description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether the role is publically available.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// get/set - The item's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - Whether the item is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The item's sort order.
        /// </summary>
        public int DisplayOrder { get; set; }

        public ICollection<UserModel> Users { get; } = new List<UserModel>();
        #endregion
    }
}
