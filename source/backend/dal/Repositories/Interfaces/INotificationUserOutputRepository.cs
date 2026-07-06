using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface INotificationUserOutputRepository : IRepository
    {
        public IEnumerable<PimsNotificationUserOutput> GetAllByFilter(NotificationUserSearchFilterModel filter);

        public PimsNotificationUserOutput GetById(long notificationUserOutputId);

        public Task<PimsNotificationUserOutput> UpdateAsync(PimsNotificationUserOutput userNotification);
    }
}
