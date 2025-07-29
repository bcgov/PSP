using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IInterestHolderRepository : IRepository
    {
        List<PimsInterestHolder> GetInterestHoldersByAcquisitionFile(long acquisitionFileId);

        List<PimsInterestHolder> UpdateAllForAcquisition(long acquisitionFileId, List<PimsInterestHolder> interestHolders);

        bool DeleteInterestHoldersPropertyTypes(ICollection<PimsPropInthldrInterestTyp> inthldPropTypes);
        bool DeleteInterestHoldersProperties(ICollection<PimsInthldrPropInterest> interestHolderProperties);
        bool DeleteInterestHolders(ICollection<PimsInterestHolder> interestHolders);
    }
}
