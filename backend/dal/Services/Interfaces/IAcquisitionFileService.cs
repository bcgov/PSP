using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface IAcquisitionFileService
    {
        PimsAcquisitionFile GetById(long id);
        PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile);
        PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile);
    }
}
