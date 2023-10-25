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

        PimsPropertyActivity Create(PimsPropertyActivity propertyActivity);

        PimsPropertyActivity Update(PimsPropertyActivity propertyActivity);

        bool TryDelete(long activityId);
    }
}
