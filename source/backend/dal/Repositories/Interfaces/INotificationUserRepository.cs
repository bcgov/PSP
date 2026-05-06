using System.Collections.Generic;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface INotificationUserRepository : IRepository
    {
        public IEnumerable<PimsNotificationUserOutput> GetAllByFilter(NotificationUserSearchFilterModel filter);

        public PimsNotificationUserOutput GetById(long notificationUserOutputId);

        public PimsNotificationUserOutput Update(PimsNotificationUserOutput userNotification);
    }
}
