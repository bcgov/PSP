namespace Pims.Api.Models.User
{
    public class AccessRequestAgencyModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the agency.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The agency name.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
