using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

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

        IEnumerable<PimsAcquisitionOwner> GetOwners(long id);

        IEnumerable<PimsAcquisitionChecklistItem> GetChecklistItems(long id);

        PimsAcquisitionFile UpdateChecklistItems(PimsAcquisitionFile acquisitionFile);

        IEnumerable<PimsAgreement> GetAgreements(long id);

        IEnumerable<PimsAgreement> UpdateAgreements(long acquisitionFileId, List<PimsAgreement> agreements);
    }
}
