using System;

namespace Pims.Dal.Entities
{
    public interface IBaseAppEntity : IBaseEntity
    {
        #region Properties
        DateTime AppCreateTimestamp { get; set; }

        string AppCreateUserid { get; set; }

        Guid? AppCreateUserGuid { get; set; }

        string AppCreateUserDirectory { get; set; }

        DateTime AppLastUpdateTimestamp { get; set; }

        string AppLastUpdateUserid { get; set; }

        Guid? AppLastUpdateUserGuid { get; set; }

        string AppLastUpdateUserDirectory { get; set; }

        DateTime DbCreateTimestamp { get; set; }

        string DbCreateUserid { get; set; }

        DateTime DbLastUpdateTimestamp { get; set; }

        string DbLastUpdateUserid { get; set; }
        #endregion
    }
}
