using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface IAcquisitionFileService
    {
        Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter);

        PimsAcquisitionFile GetById(long id);

        PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile);

        PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile);

        PimsAcquisitionFile UpdateProperties(PimsAcquisitionFile acquisitionFile);
    }
}
