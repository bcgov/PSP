using System;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a DocumentQueue.
        /// </summary>
        /// <param name="id">the document queue id.</param>
        /// <param name="status">the status of the queued document.</param>
        /// <param name="dataSourceTypeCd">the source of the queued document.</param>
        /// <returns>the filled-out test entity.</returns>
        public static Entity.PimsDocumentQueue CreateDocumentQueue(long id = 1, string status = "Pending", string dataSourceTypeCd = "PIMS")
        {
            return new Entity.PimsDocumentQueue()
            {
                DocumentQueueId = id,
                DocumentQueueStatusTypeCode = status,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppLastUpdateTimestamp = DateTime.Now,
                AppLastUpdateUserid = "admin",
                DocumentId = id,
                DocumentNavigation = CreateDocument(id: id),
                Document = new byte[] { 1, 2, 3 },
                DataSourceTypeCode = "PIMS",
                DataSourceTypeCodeNavigation = new Entity.PimsDataSourceType() { Id = dataSourceTypeCd ?? $"PIMS-{id}", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" },
                DocumentQueueStatusTypeCodeNavigation = new Entity.PimsDocumentQueueStatusType() { Id = status ?? $"PENDING-{id}", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" },
            };
        }
    }
}
