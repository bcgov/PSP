using System;
using System.Collections.Generic;

namespace Pims.Api.Areas.Admin.Models.User
{
    /// <summary>
    /// UserModel class, provides a model that represents a user.
    /// </summary>
    public class UserModel : Pims.Api.Models.BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the user.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The user's unique identifier.
        /// </summary>
        public Guid KeycloakUserId { get; set; }

        /// <summary>
        /// get/set - The user's unique identity.
        /// </summary>
        public string BusinessIdentifier { get; set; }

        /// <summary>
        /// get/set - The user's position or job title.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// get/set - A note corresponding to this user.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// get/set - The user's given name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The user's middlename.
        /// </summary>
        public string MiddleNames { get; set; }

        /// <summary>
        /// get/set - The user's surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The user's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - Whether the user is disabled.
        /// </summary>
        /// <value></value>
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
