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

        PimsDispositionSale GetDispositionFileSale(long dispositionId);

        long GetRowVersion(long id);
    }
}
