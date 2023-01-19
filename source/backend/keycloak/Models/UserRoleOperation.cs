using System.Text.Json.Serialization;

namespace Pims.Keycloak.Models
{
    /// <summary>
    /// UserRoleOperation class, provides a way to add or remove a role from a user in keycloak.
    /// </summary>
    public class UserRoleOperation
    {
        #region Properties

        /// <summary>
        /// get/set - The user to operate on.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// get/set - The role to operate on the user.
        /// </summary>
        [JsonPropertyName("roleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// get/set - The operation to perform on the user with the role.
        /// </summary>
        [JsonPropertyName("operation")]
        public string Operation { get; set; }
        #endregion
    }
}
