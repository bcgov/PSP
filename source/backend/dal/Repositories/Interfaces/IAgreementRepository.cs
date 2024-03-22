using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IAgreementRepository : IRepository
    {
        List<PimsAgreement> GetAgreementsByAcquisitionFile(long acquisitionFileId);

        PimsAgreement GetAgreementById(long agreementId);

        PimsAgreement AddAgreement(PimsAgreement agreement);

        List<PimsAgreement> SearchAgreements(AcquisitionReportFilterModel filter);

        PimsAgreement UpdateAgreement(PimsAgreement agreement);

        bool TryDeleteAgreement(long acquisitionFileId, long agreementId);
    }
}
