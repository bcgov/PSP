namespace Pims.Api.Areas.Admin.Models.AccessRequest
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
        #endregion
    }
}
