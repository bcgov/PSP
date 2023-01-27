namespace Pims.Api.Models.Tenant
{
    /// <summary>
    /// TenantModel class, provides a model to represent a tenant.
    /// </summary>
    public class TenantModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The tenant unique code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - The tenant name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The tenant settings configuration.
        /// </summary>
        public TenantSettingsModel Settings { get; set; }
        #endregion
    }
}
