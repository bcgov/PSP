using System;

namespace Pims.Api.Models
{
    public abstract class BaseAppModel : BaseModel
    {
        #region Properties
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string UpdatedByName { get; set; }

        public string UpdatedByEmail { get; set; }
        #endregion
    }
}
