namespace Pims.Api.Models.Concepts
{
    public class ClaimModel : Api.Models.BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify the claim.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - The claims display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The GUID that identifies this Role in Keycloak.
        /// </summary>
        public string KeycloakRoleId { get; set; }

        /// <summary>
        /// get/set - The claims first name.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether the user is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }
        #endregion
    }
}
