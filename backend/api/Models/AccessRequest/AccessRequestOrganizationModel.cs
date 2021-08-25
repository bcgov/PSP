namespace Pims.Api.Models.AccessRequest
{
    public class AccessRequestOrganizationModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the organization.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization name.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
