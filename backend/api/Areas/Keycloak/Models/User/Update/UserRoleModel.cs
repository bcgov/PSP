using System;

namespace Pims.Api.Areas.Keycloak.Models.User.Update
{
    /// <summary>
    /// UserRoleModel class, provides a model that represents a user role model.
    /// </summary>
    public class UserRoleModel
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
        /// get/set - A unique name that identifies the user role.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
