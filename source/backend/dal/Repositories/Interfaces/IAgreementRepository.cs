using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IAgreementRepository : IRepository
    {
        List<PimsAgreement> GetAgreementsByAquisitionFile(long acquisitionFileId);

        List<PimsAgreement> SearchAgreements(AcquisitionReportFilterModel filter);

        PimsAgreement Update(PimsAgreement agreement);

        List<PimsAgreement> UpdateAllForAcquisition(long acquisitionFileId, List<PimsAgreement> agreements);
    }
}
