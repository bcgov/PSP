using System;

namespace Pims.Tools.Keycloak.Sync.Models.Pims
{
    /// <summary>
    /// AgencyModel class, provides a model that represents the agency.
    /// </summary>
    public class AgencyModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - The item's unique identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify the agency.
        /// </summary>
        public Guid Key { get; set; }

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
        public int SortOrder { get; set; }

        /// <summary>
        /// get/set - The item's unique code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - The agency description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The parent agency.
        /// </summary>
        public long? ParentId { get; set; }
        #endregion
    }
}
