using System;

namespace Pims.Api.Areas.Keycloak.Models.User
{
    /// <summary>
    /// AccessRequestUserModel class, provides a model that represents a user attached to an access request.
    /// </summary>
    public class AccessRequestUserModel : Api.Models.BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the claim.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The user's display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// get/set - The user's given name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The user's middlename.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// get/set - The user's surname.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// get/set - The user's email.
        /// </summary>
        public string Email { get; set; }
        #endregion
    }
}
