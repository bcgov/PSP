using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IForm8Repository : IRepository<PimsForm8>
    {
        PimsForm8 Add(PimsForm8 form8);

        IList<PimsForm8> GetAllByAcquisitionFileId(long acquisitionFileId);

        PimsForm8 GetById(long form8Id);
    }
}
