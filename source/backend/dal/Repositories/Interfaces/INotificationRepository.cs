namespace Pims.Dal.Repositories
{
    using System.Collections.Generic;
    using Pims.Dal.Entities;

    public interface INotificationRepository : IRepository
    {
        IEnumerable<PimsNotification> GetByUser(long userId);

        PimsNotification GetById(long notificationId);

        PimsNotification Add(PimsNotification notification, long userId);

        PimsNotification Update(PimsNotification notification, long userId);

        bool Delete(long notificationId, long userId);

        long GetRowVersion(long id);
    }
}