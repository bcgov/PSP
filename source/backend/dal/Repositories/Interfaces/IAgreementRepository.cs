using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IAgreementRepository : IRepository
    {
        List<PimsAgreement> GetAgreementsByAcquisitionFile(long acquisitionFileId);

        List<PimsAgreement> SearchAgreements(AcquisitionReportFilterModel filter);

        List<PimsAgreement> UpdateAllForAcquisition(long acquisitionFileId, List<PimsAgreement> agreements);
    }
}
