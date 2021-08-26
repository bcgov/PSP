namespace Pims.Api.Areas.Admin.Models.User
{
    /// <summary>
    /// OrganizationModel class, provides a model that represents the organization.
    /// </summary>
    public class OrganizationModel : Api.Models.BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify organization.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The organization description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Foreign key to the parent organization.
        /// </summary>
        public long? ParentId { get; set; }
        #endregion
    }
}
