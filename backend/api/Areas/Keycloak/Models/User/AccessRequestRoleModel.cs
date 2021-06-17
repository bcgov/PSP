namespace Pims.Api.Areas.Keycloak.Models.User
{
    /// <summary>
    /// AccessRequestRoleModel class, provides a model that represents a role attached to an access request.
    /// </summary>
    public class AccessRequestRoleModel : Pims.Api.Models.LookupModel
    {
        #region Properties
        public long Id { get; set; }
        public string Description { get; set; }
        #endregion
    }
}
