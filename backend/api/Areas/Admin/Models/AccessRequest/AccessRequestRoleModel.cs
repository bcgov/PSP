namespace Pims.Api.Areas.Admin.Models.AccessRequest
{
    /// <summary>
    /// AccessRequestRoleModel class, provides a model that represents a role attached to an access request.
    /// </summary>
    public class AccessRequestRoleModel : Api.Models.BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify record.
        /// </summary>
        public long Id { get; set; }

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
        public string Description { get; set; }
        #endregion
    }
}
