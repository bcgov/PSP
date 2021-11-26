using System;

namespace Pims.Api.Models
{
    public abstract class BaseAppModel : BaseModel
    {
        #region Properties
        public DateTime AppCreateTimestamp { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string UpdatedByName { get; set; }
        #endregion
    }
}
