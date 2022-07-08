using System.Collections.Generic;

namespace Pims.Api.Models.User
{
    public class OrganizationModel : CodeModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify organization.
        /// </summary>
        public long Id { get; set; }

        public string Description { get; set; }

        public OrganizationModel Parent { get; set; }

        public ICollection<OrganizationModel> Children { get; } = new List<OrganizationModel>();

        public ICollection<UserModel> Users { get; } = new List<UserModel>();
        #endregion
    }
}
