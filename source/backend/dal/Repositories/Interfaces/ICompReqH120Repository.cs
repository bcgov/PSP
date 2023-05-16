using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ICompReqH120Repository : IRepository<PimsCompReqH120>
    {
        public IEnumerable<PimsCompReqH120> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly);
    }
}
