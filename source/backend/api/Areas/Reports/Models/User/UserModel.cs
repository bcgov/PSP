using System;

namespace Pims.Api.Areas.Reports.Models.User
{
    /// <summary>
    /// UserModel class, provides a model that represents a user.
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
        /// get/set - The user's unique identity.
        /// </summary>
        public string BusinessIdentifier { get; set; }

        /// <summary>
        /// get/set - The user's given name.
        /// </summary>
        public string FirstName { get; set; }

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
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - Who approved this user.
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// get/set - When this user was approved on.
        /// </summary>
        public DateTime? IssueOn { get; set; }

        /// <summary>
        /// get/set - The User type (e.g. internal staff, contractor, etc).
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// get/set - A comma-separated list of regions the user belongs to.
        /// </summary>
        public string Regions { get; set; }

        /// <summary>
        /// get/set - A comma-separated list of roles the user is a member of.
        /// </summary>
        public string Roles { get; set; }
        #endregion
    }
}
