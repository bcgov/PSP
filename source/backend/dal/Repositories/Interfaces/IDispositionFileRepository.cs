using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IDispositionFileRepository : IRepository
    {
        Paged<PimsDispositionFile> GetPageDeep(DispositionFilter filter);

        PimsDispositionFile GetById(long id);

        PimsDispositionFile Add(PimsDispositionFile disposition);

        LastUpdatedByModel GetLastUpdateBy(long id);

        List<PimsDispositionFileTeam> GetTeamMembers();

        List<PimsDispositionOffer> GetDispositionOffers(long dispositionId);

        PimsDispositionOffer GetDispositionOfferById(long dispositionId, long dispositionOfferId);

        PimsDispositionOffer AddDispositionOffer(PimsDispositionOffer dispositionOffer);

        PimsDispositionOffer UpdateDispositionOffer(PimsDispositionOffer dispositionOffer);

        bool TryDeleteDispositionOffer(long dispositionId, long dispositionOfferId);

        PimsDispositionSale GetDispositionFileSale(long dispositionId);

        PimsDispositionAppraisal GetDispositionFileAppraisal(long dispositionId);

        PimsDispositionAppraisal AddDispositionFileAppraisal(PimsDispositionAppraisal dispositionAppraisal);

        PimsDispositionAppraisal UpdateDispositionFileAppraisal(long id, PimsDispositionAppraisal dispositionAppraisal);

        long GetRowVersion(long id);

        List<PimsDispositionFile> GetDispositionFileExportDeep(DispositionFilter filter);
    }
}
