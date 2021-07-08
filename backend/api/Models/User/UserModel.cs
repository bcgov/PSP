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
        /// get/set - Unique key to identify the claim.
        /// </summary>
        public Guid Key { get; set; }

        public bool IsDisabled { get; set; }

        public string Username { get; set; }

        public string Position { get; set; }

        public string DisplayName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Note { get; set; }

        public IEnumerable<AgencyModel> Agencies { get; set; }

        public IEnumerable<RoleModel> Roles { get; set; }
        #endregion
    }
}
