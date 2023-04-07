using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IAgreementRepository : IRepository
    {
        List<PimsAgreement> GetAgreementsByAquisitionFile(long acquisitionFileId);

        PimsAgreement Update(PimsAgreement agreement);

        List<PimsAgreement> UpdateAllForAcquisition(long acquisitionFileId, List<PimsAgreement> agreements);
    }
}
