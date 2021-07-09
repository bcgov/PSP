using System;
using System.Collections.Generic;

namespace Pims.Api.Models.User
{
    public class AgencyModel : CodeModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify agency.
        /// </summary>
        public long Id { get; set; }

        public string Description { get; set; }
        public AgencyModel Parent { get; set; }
        public ICollection<AgencyModel> Children { get; } = new List<AgencyModel>();
        public ICollection<UserModel> Users { get; } = new List<UserModel>();
        #endregion
    }
}
