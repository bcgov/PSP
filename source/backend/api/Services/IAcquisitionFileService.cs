using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface IAcquisitionFileService
    {
        Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter);

        PimsAcquisitionFile GetById(long id);

        LastUpdatedByModel GetLastUpdateInformation(long acquisitionFileId);

        PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile, IEnumerable<UserOverrideCode> userOverrides);

        PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile, IEnumerable<UserOverrideCode> userOverrides);

        PimsAcquisitionFile UpdateProperties(PimsAcquisitionFile acquisitionFile, IEnumerable<UserOverrideCode> userOverrides);

        IEnumerable<PimsPropertyAcquisitionFile> GetProperties(long id);

        IEnumerable<PimsAcquisitionOwner> GetOwners(long id);

        IEnumerable<PimsAcquisitionFileTeam> GetTeamMembers();

        IEnumerable<PimsAcquisitionChecklistItem> GetChecklistItems(long id);

        PimsAcquisitionFile UpdateChecklistItems(IList<PimsAcquisitionChecklistItem> checklistItems);

        IEnumerable<PimsAgreement> GetAgreements(long id);

        PimsAgreement AddAgreement(long acquisitionFileId, PimsAgreement agreement);

        PimsAgreement GetAgreementById(long acquisitionFileId, long agreementId);

        IEnumerable<PimsAgreement> SearchAgreements(AcquisitionReportFilterModel filter);

        PimsAgreement UpdateAgreement(long acquisitionFileId, PimsAgreement agreement);

        bool DeleteAgreement(long acquisitionFileId, long agreementId);

        IEnumerable<PimsInterestHolder> GetInterestHolders(long id);

        IEnumerable<PimsInterestHolder> UpdateInterestHolders(long acquisitionFileId, List<PimsInterestHolder> interestHolders);

        PimsExpropriationPayment AddExpropriationPayment(long acquisitionFileId, PimsExpropriationPayment expPayment);

        IList<PimsExpropriationPayment> GetAcquisitionExpropriationPayments(long acquisitionFileId);

        List<AcquisitionFileExportModel> GetAcquisitionFileExport(AcquisitionFilter filter);
    }
}
