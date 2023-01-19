using System.Collections.Generic;

namespace Pims.Keycloak.Models
{
    /// <summary>
    /// UserModel class, provides a way to manage users within keycloak.
    /// </summary>
    public class UserModel
    {
        #region Properties

        /// <summary>
        /// get/set - The unique user name for this user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// get/set - The user's given name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The user's surname.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// get/set - The user's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - A dictionary of user attributes.
        /// </summary>
        public Dictionary<string, string[]> Attributes { get; set; }
        #endregion
    }
}
