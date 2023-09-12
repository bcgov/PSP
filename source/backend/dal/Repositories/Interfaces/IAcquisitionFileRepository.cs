using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionFileRepository : IRepository
    {
        Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter, HashSet<short> regions, long? contractorPersonId = null);

        PimsAcquisitionFile GetById(long id);

        List<PimsAcquisitionOwner> GetOwnersByAcquisitionFileId(long acquisitionFileId);

        List<PimsAcquisitionFilePerson> GetTeamMembers(HashSet<short> regions, long? contractorPersonId = null);

        PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile);

        PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile);

        long GetRowVersion(long id);

        short GetRegion(long id);

        List<PimsAcquisitionFile> GetByProductId(long productId);

        List<PimsAcquisitionFile> GetAcquisitionFileExport(AcquisitionFilter filter, HashSet<short> regions, long? contractorPersonId = null);
    }
}
