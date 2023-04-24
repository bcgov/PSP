using System;

namespace Pims.Api.Areas.Keycloak.Models.AccessRequest
{
    /// <summary>
    /// UserModel class, provides a model that represents a user attached to an access request.
    /// </summary>
    public class UserModel : Api.Models.BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the claim.
        /// </summary>
        public Guid KeycloakUserId { get; set; }

        /// <summary>
        /// get/set - The user's given name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The user's surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The user's Type.
        /// </summary>
        public string UserTypeCode { get; set; }

        /// <summary>
        /// get/set - The user's email.
        /// </summary>
        public string Email { get; set; }
        #endregion
    }
}
