using System;

namespace Pims.Api.Models
{
    public abstract class BaseAppModel : BaseModel
    {
        #region Properties
        public DateTime AppCreateTimestamp { get; set; }

        public DateTime AppLastUpdateTimestamp { get; set; }

        public string AppLastUpdateUserid { get; set; }

        public string AppCreateUserid { get; set; }

        public Guid? AppLastUpdateUserGuid { get; set; }

        public Guid? AppCreateUserGuid { get; set; }
        #endregion
    }
}
