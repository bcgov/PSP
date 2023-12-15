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

        LastUpdatedByModel GetLastUpdateInformation(long dispositionFileId);

        IEnumerable<PimsDispositionFileProperty> GetProperties(long id);

        IEnumerable<PimsDispositionFileTeam> GetTeamMembers();

        IEnumerable<PimsDispositionOffer> GetOffers(long dispositionFileId);

        IEnumerable<PimsDispositionSale> GetSales(long dispositionFileId);
    }
}
