namespace Pims.Api.Models.AccessRequest
{
    public class AccessRequestModel : BaseAppModel
    {
        #region Properties
        public long Id { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public UserModel User { get; set; }
        public long? RoleId { get; set; }
        public long? OrganizationId { get; set; }
        #endregion
    }
}
