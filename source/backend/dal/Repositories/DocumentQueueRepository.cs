using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// DocumentQueueRepository class, provides a repository to interact with queued documents within the datasource.
    /// </summary>
    public class DocumentQueueRepository : BaseRepository<PimsDocument>, IDocumentQueueRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentQueueRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DocumentQueueRepository(
            PimsContext dbContext,
            ClaimsPrincipal user,
            ILogger<DocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Updates the queued document in the database.
        /// </summary>
        /// <param name="queuedDocument"></param>
        /// <returns></returns>
        public PimsDocumentQueue Update(PimsDocumentQueue queuedDocument)
        {
            queuedDocument.ThrowIfNull(nameof(queuedDocument));

            queuedDocument = Context.Update(queuedDocument).Entity;
            return queuedDocument;
        }

        /// <summary>
        /// Deletes the passed queued document from the database. Note, removing a queued document does not delete the imported document.
        /// </summary>
        /// <param name="queuedDocument"></param>
        /// <returns></returns>
        public bool Delete(PimsDocumentQueue queuedDocument)
        {
            queuedDocument.ThrowIfNull(nameof(queuedDocument));

            Context.Remove(queuedDocument);
            return true;
        }

        /// <summary>
        /// Return a list of documents, filtered by the specified arguments.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<PimsDocumentQueue> GetAllByFilter(DocumentQueueFilter filter)
        {
            var query = Context.PimsDocumentQueues.Where(q => true);

            if (filter.DataSourceTypeCode != null)
            {
                query.Where(d => d.DataSourceTypeCode == filter.DataSourceTypeCode);
            }
            if (filter.DocumentQueueStatusTypeCode != null)
            {
                query.Where(d => d.DocumentQueueStatusTypeCode == filter.DocumentQueueStatusTypeCode);
            }
            if (filter.DocProcessStartDate != null)
            {
                query.Where(d => d.DocProcessStartDt >= filter.DocProcessStartDate);
            }
            if (filter.DocProcessEndDate != null)
            {
                query.Where(d => d.DocProcessEndDt <= filter.DocProcessEndDate);
            }
            return query.ToList();
        }

        public int DocumentQueueCount(PimsDocumentQueueStatusType pimsDocumentQueueStatusType)
        {
            if (pimsDocumentQueueStatusType == null)
            {
                Context.PimsDocumentQueues.Count();
            }
            return Context.PimsDocumentQueues.Count(d => d.DocumentQueueStatusTypeCode == pimsDocumentQueueStatusType.DocumentQueueStatusTypeCode);
        }

        #endregion
    }
}
