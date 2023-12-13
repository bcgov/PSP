using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Role;
using Pims.Api.Models.Concepts.User;

namespace Pims.Api.Models.Concepts.AccessRequest
{
    /// <summary>
    /// Provides a model for user access requests to the system.
    /// </summary>
    public class AccessRequestModel : BaseAuditModel
    {
        #region Properties
        public long Id { get; set; }

        public TypeModel<string> AccessRequestStatusTypeCode { get; set; }

        public TypeModel<short> RegionCode { get; set; }

        public string Note { get; set; }

        public UserModel User { get; set; }

        public long? UserId { get; set; }

        public RoleModel Role { get; set; }

        public long? RoleId { get; set; }
        #endregion
    }
}
