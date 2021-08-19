using System.Collections.Generic;

namespace Pims.Api.Areas.Admin.Models.AccessRequest
{
    public class AccessRequestModel : Api.Models.BaseAppModel
    {
        #region Properties
        public long Id { get; set; }
        public UserModel User { get; set; }
        public RoleModel Role { get; set; }
        public IEnumerable<OrganizationModel> Organizations { get; set; }
        #endregion
    }
}
