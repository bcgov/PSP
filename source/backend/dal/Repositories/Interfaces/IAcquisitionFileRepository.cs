using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionFileRepository : IRepository
    {
        Paged<PimsAcquisitionFile> GetPageDeep(AcquisitionFilter filter, HashSet<short> regions, long? contractorPersonId = null);

        PimsAcquisitionFile GetById(long id);

        LastUpdatedByModel GetLastUpdateBy(long id);

        List<PimsAcquisitionOwner> GetOwnersByAcquisitionFileId(long acquisitionFileId);

        List<PimsAcquisitionFileTeam> GetTeamMembers(HashSet<short> regions, long? contractorPersonId = null);

        PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile);

        PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile);

        long GetRowVersion(long id);

        short GetRegion(long id);

        List<PimsAcquisitionFile> GetByProductId(long productId);

        PimsAcquisitionFile GetByAcquisitionFilePropertyId(long acquisitionFilePropertyId);

        PimsProperty GetProperty(long acquisitionFilePropertyId);

        List<PimsAcquisitionFile> GetAcquisitionFileExportDeep(AcquisitionFilter filter, HashSet<short> regions, long? contractorPersonId = null);
    }
}
