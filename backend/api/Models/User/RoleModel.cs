using System;
using System.Collections.Generic;

namespace Pims.Api.Models.User
{
    public class RoleModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the role.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the role.
        /// </summary>
        public Guid Key { get; set; }

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
        /// get/set - Whether this item is visible.
        /// </summary>
        public bool? IsVisible { get; set; }

        /// <summary>
        /// get/set - The item's sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// get/set - The item's type.
        /// </summary>
        public string Type { get; set; }

        public ICollection<UserModel> Users { get; } = new List<UserModel>();
        #endregion
    }
}
