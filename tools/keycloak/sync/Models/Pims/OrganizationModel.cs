using System;

namespace Pims.Tools.Keycloak.Sync.Models.Pims
{
    /// <summary>
    /// OrganizationModel class, provides a model that represents the organization.
    /// </summary>
    public class OrganizationModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - The item's unique identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify the organization.
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
        public int DisplayOrder { get; set; }

        /// <summary>
        /// get/set - The item's unique code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - The organization description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The parent organization.
        /// </summary>
        public long? ParentId { get; set; }
        #endregion
    }
}
