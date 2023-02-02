using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System.Collections.Generic;

namespace Pims.Api.Services
{
    public interface IAcquisitionFileService
    {
        Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter);

        PimsAcquisitionFile GetById(long id);

        PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile);

        PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile, bool userOverride);

        PimsAcquisitionFile UpdateProperties(PimsAcquisitionFile acquisitionFile);

        IEnumerable<PimsPropertyAcquisitionFile> GetProperties(long id);
    }
}
