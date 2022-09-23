using System;
using System.Collections.Generic;

namespace Pims.Api.Areas.Keycloak.Models.User
{
    /// <summary>
    /// UserModel class, provides a model to represent a user.
    /// </summary>
    public class UserModel : Pims.Api.Models.BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the user within keycloak.
        /// </summary>
        public Guid KeycloakUserId { get; set; }

        /// <summary>
        /// get/set - A unique username to identify the user.
        /// </summary>
        public string BusinessIdentifier { get; set; }

        /// <summary>
        /// get/set - The user's given name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The user's middle names.
        /// </summary>
        public string MiddleNames { get; set; }

        /// <summary>
        /// get/set - The user's last name.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The user's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - Whether the user is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - An array of organizations the user belongs to.
        /// </summary>
        public IEnumerable<OrganizationModel> Organizations { get; set; } = new List<OrganizationModel>();

        /// <summary>
        /// get/set - An array of roles the user is a member of.
        /// </summary>
        public IEnumerable<RoleModel> Roles { get; set; } = new List<RoleModel>();
        #endregion
    }
}
