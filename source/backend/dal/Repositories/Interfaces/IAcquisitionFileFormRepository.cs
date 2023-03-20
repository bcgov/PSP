using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionFileFormRepository : IRepository<PimsAcquisitionFileForm>
    {
        PimsAcquisitionFileForm GetById(long id);

        long GetRowVersion(long fileFormId);

        PimsAcquisitionFileForm Add(PimsAcquisitionFileForm fileForm);
    }
}
