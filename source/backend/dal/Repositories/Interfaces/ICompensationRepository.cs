using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ICompensationRepository : IRepository<PimsCompensationRequisition>
    {
        IList<PimsCompensationRequisition> GetAllByAcquisitionFileId(long acquisitionFileId);

        bool TryDelete(long compensationId);
    }
}
