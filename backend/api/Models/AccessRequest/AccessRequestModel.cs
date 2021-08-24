using System.Collections.Generic;

namespace Pims.Api.Models.AccessRequest
{
    public class AccessRequestModel : BaseAppModel
    {
        #region Properties
        public long Id { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public UserModel User { get; set; }
        public RoleModel Role { get; set; }
        public AccessRequestOrganizationModel Organization { get; set; }
        #endregion
    }
}
