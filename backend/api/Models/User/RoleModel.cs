using System;
using System.Collections.Generic;

namespace Pims.Api.Models.User
{
    public class RoleModel : LookupModel
    {
        #region Properties
        public long Id { get; set; }
        public Guid Key { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public ICollection<UserModel> Users { get; } = new List<UserModel>();
        #endregion
    }
}
