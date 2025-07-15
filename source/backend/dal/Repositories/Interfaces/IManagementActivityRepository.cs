namespace Pims.Dal.Repositories
{
    using System.Collections.Generic;
    using Pims.Dal.Entities;
    using Pims.Dal.Entities.Models;

    /// <summary>
    /// IManagementActivityPropertyRepository interface, provides functions to interact with management activitiy properties within the datasource.
    /// </summary>
    public interface IManagementActivityRepository : IRepository<PimsManagementActivity>
    {
        int Count();

        Paged<PimsManagementActivity> GetPageDeep(ManagementActivityFilter filter);

        IList<PimsManagementActivity> GetActivitiesByProperty(long propertyId);

        PimsManagementActivity GetActivity(long activityId);

        IList<PimsManagementActivity> GetActivitiesByManagementFile(long managementFileId);

        IList<PimsManagementActivity> GetActivitiesByPropertyIds(IEnumerable<long> propertyIds);

        PimsManagementActivity Create(PimsManagementActivity propertyActivity);

        PimsManagementActivity Update(PimsManagementActivity propertyActivity);

        bool TryDelete(long activityId);

        bool TryDeleteByFile(long activityId, long managementFileId);
    }
}
