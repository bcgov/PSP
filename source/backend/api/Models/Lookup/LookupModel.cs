namespace Pims.Api.Models.Lookup
{
    /// <summary>
    /// LookupModel class, provides a model that represents a code lookup item.
    /// </summary>
    /// <typeparam name="T">The underlying type of this LookupModel.</typeparam>
    public class LookupModel<T> : LookupModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify record.
        /// </summary>
        public new T Id { get; set; }

        /// <summary>
        /// get/set - Optional parent of this lookup.
        /// Allows to model parent/child relationships in lookup codes (ex Country -> Province).
        /// </summary>
        public new T ParentId { get; set; }
        #endregion
    }

    public class LookupModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify record.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// get/set - Optional parent of this lookup.
        /// Allows to model parent/child relationships in lookup codes (ex Country -> Province).
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// get/set - Code value of this lookup.
        /// </summary>
        public string Code { get; set; }

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
        public int DisplayOrder { get; set; }

        /// <summary>
        /// get/set - The item's type.
        /// </summary>
        public string Type { get; set; }
        #endregion
    }
}
