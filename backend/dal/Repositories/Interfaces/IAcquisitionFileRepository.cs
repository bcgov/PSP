using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionFileRepository : IRepository
    {
        PimsAcquisitionFile GetById(long id);

        PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile);

        PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile);

        long GetRowVersion(long id);
    }
}
