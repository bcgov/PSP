using Pims.Api.Models.Base;

namespace Pims.Api.Areas.Keycloak.Models.AccessRequest
{
    public class AccessRequestModel : BaseAuditModel
    {
        #region Properties
        public long Id { get; set; }

        public string Status { get; set; }

        public string Note { get; set; }

        public UserModel User { get; set; }

        public RoleModel Role { get; set; }

        public OrganizationModel Organization { get; set; }
        #endregion
    }
}
