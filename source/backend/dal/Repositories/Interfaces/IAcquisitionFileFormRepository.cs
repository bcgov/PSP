using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionFileFormRepository : IRepository<PimsAcquisitionFileForm>
    {
        PimsAcquisitionFileForm GetById(long id);

        long GetRowVersion(long fileFormId);

        PimsAcquisitionFileForm Add(PimsAcquisitionFileForm fileForm);

        IEnumerable<PimsAcquisitionFileForm> GetAllByAcquisitionFileId(long acquisitionFileId);

        PimsAcquisitionFileForm GetByAcquisitionFileFormId(long acquisitionFileFormId);

        bool TryDelete(long acquisitionFileFormId);
    }
}
