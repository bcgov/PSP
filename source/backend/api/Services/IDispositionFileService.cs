using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface IDispositionFileService
    {
        Paged<PimsDispositionFile> GetPage(DispositionFilter filter);

        PimsDispositionFile GetById(long id);

        PimsDispositionFile Add(PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides);

        PimsDispositionFile Update(long id, PimsDispositionFile dispositionFile, IEnumerable<UserOverrideCode> userOverrides);

        LastUpdatedByModel GetLastUpdateInformation(long dispositionFileId);

        IEnumerable<PimsDispositionFileProperty> GetProperties(long id);

        IEnumerable<PimsDispositionFileTeam> GetTeamMembers();

        IEnumerable<PimsDispositionOffer> GetOffers(long dispositionFileId);

        PimsDispositionOffer GetDispositionOfferById(long dispositionFileId, long dispositionOfferId);

        PimsDispositionOffer AddDispositionFileOffer(long dispositionFileId, PimsDispositionOffer dispositionOffer);

        PimsDispositionOffer UpdateDispositionFileOffer(long dispositionFileId, long offerId, PimsDispositionOffer dispositionOffer);

        bool DeleteDispositionFileOffer(long dispositionFileId, long offerId);

        PimsDispositionSale GetDispositionFileSale(long dispositionFileId);

        PimsDispositionSale AddDispositionFileSale(PimsDispositionSale dispositionSale);

        PimsDispositionSale UpdateDispositionFileSale(PimsDispositionSale dispositionSale);

        PimsDispositionAppraisal GetDispositionFileAppraisal(long dispositionFileId);

        PimsDispositionAppraisal AddDispositionFileAppraisal(long dispositionFileId, PimsDispositionAppraisal dispositionAppraisal);

        PimsDispositionAppraisal UpdateDispositionFileAppraisal(long dispositionFileId, long appraisalId, PimsDispositionAppraisal dispositionAppraisal);

        IEnumerable<PimsDispositionChecklistItem> GetChecklistItems(long id);

        List<DispositionFileExportModel> GetDispositionFileExport(DispositionFilter filter);

        PimsDispositionFile UpdateChecklistItems(IList<PimsDispositionChecklistItem> checklistItems);
    }
}
