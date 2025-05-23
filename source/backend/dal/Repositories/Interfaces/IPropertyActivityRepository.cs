namespace Pims.Dal.Repositories
{
    using System.Collections.Generic;
    using Pims.Dal.Entities;

    /// <summary>
    /// IPropertyActivityRepository interface, provides functions to interact with property activities within the datasource.
    /// </summary>
    public interface IPropertyActivityRepository : IRepository<PimsPropertyActivity>
    {
        IList<PimsPropertyActivity> GetActivitiesByProperty(long propertyId);

        PimsPropertyActivity GetActivity(long activityId);

        IList<PimsPropertyActivity> GetActivitiesByManagementFile(long managementFileId);

        IList<PimsPropertyActivity> GetActivitiesByPropertyIds(IEnumerable<long> propertyIds);

        PimsPropertyActivity Create(PimsPropertyActivity propertyActivity);

        PimsPropertyActivity Update(PimsPropertyActivity propertyActivity);

        bool TryDelete(long activityId);

        bool TryDeleteByFile(long activityId, long managementFileId);
    }
}
