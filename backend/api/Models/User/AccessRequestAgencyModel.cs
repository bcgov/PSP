namespace Pims.Api.Models.User
{
    public class AccessRequestAgencyModel : CodeModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify agency.
        /// </summary>
        public long Id { get; set; }

        public string Description { get; set; }
        #endregion
    }
}
