namespace Pims.Api.Models
{
    /// <summary>
    /// CodeModel class, provides a model that represents a code item.
    /// </summary>
    public class CodeModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The item's unique code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - The parent id of this item.
        /// </summary>
        public long? ParentId { get; set; } // TODO: PSP-4421 this isn't ideal as it will only currently be used by organization.

        /// <summary>
        /// get/set - The name of the code.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - Whether this code is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The sort order of the lookup item.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// get/set - The item's type.
        /// </summary>
        public string Type { get; set; }
        #endregion
    }
}
