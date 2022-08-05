using System;
using System.Collections.Generic;

namespace Pims.Api.Models.User
{
    public class UserModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the user.
        /// </summary>
        public Guid KeycloakUserId { get; set; }

        public bool IsDisabled { get; set; }

        public string BusinessIdentifier { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Position { get; set; }

        public IEnumerable<OrganizationModel> Organizations { get; set; }

        public IEnumerable<RoleModel> Roles { get; set; }
        #endregion
    }
}
