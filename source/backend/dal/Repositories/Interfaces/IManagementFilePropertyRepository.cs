using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IManagementFilePropertyRepository : IRepository
    {
        List<PimsManagementFileProperty> GetPropertiesByManagementFileId(long managementFileId);

        int GetManagementFilePropertyRelatedCount(long propertyId);

        PimsManagementFileProperty Add(PimsManagementFileProperty propertyManagementFile);

        PimsManagementFileProperty Update(PimsManagementFileProperty propertyManagementFile);

        void Delete(PimsManagementFileProperty propertyManagementFile);
    }
}
