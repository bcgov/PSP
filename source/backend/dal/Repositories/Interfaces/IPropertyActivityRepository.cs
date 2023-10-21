namespace Pims.Dal.Repositories
{
    using Pims.Dal.Entities;

    /// <summary>
    /// IPropertyActivityRepository interface, provides functions to interact with property activities within the datasource.
    /// </summary>
    public interface IPropertyActivityRepository : IRepository<PimsPropertyActivity>
    {
        PimsPropertyActivity GetActivity(long activityId);

        PimsPropertyActivity Create(PimsPropertyActivity propertyActivity);

        PimsPropertyActivity Update(PimsPropertyActivity propertyActivity);

        void Delete(long activityId);
    }
}
