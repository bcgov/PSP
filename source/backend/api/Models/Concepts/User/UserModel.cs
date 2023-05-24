using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class UserModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify the user.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The user's unique identifier.
        /// </summary>
        public Guid GuidIdentifierValue { get; set; }

        /// <summary>
        /// get/set - The user's unique identity.
        /// </summary>
        public string BusinessIdentifierValue { get; set; }

        /// <summary>
        /// get/set - Primary key to identify the user that approved this access request.
        /// </summary>
        public long ApprovedById { get; set; }

        /// <summary>
        /// get/set - The user's position or job title.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// get/set - The User type code.
        /// </summary>
        public TypeModel<string> UserTypeCode { get; set; }

        /// <summary>
        /// get/set - A note corresponding to this user.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// get/set - Whether the user is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The date this user last logged into PSP.
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// get/set - The date that this user was given access to PIMS.
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// get/set - An array of roles the user is a member of.
        /// </summary>
        public IEnumerable<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();

        /// <summary>
        /// get/set - A person entity that corresponds to this user.
        /// </summary>
        public PersonModel Person { get; set; }

        public IEnumerable<RegionUserModel> UserRegions { get; set; }
        #endregion
    }
}
