using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IProjectNotificationService interface, provides functions to interact with project notifications within the datasource.
    /// </summary>
    public interface IProjectNotificationService : IService
    {
        ProjectStatusNotification Get(long id);
        IEnumerable<ProjectStatusNotification> GetFor(long? fromStatusId, long? toStatusId);
    }
}
