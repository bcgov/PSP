using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IInterestHolderRepository : IRepository
    {
        List<PimsInterestHolder> GetInterestHoldersByAcquisitionFile(long acquisitionFileId);

        List<PimsInterestHolder> UpdateAllForAcquisition(long acquisitionFileId, List<PimsInterestHolder> interestHolders);
    }
}
