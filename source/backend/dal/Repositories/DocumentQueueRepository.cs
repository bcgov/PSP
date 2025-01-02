using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
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
        /// Attempts to find a queued document via the documentQueueId. Returns null if not found.
        /// </summary>
        /// <param name="documentQueueId"></param>
        /// <returns></returns>
        public PimsDocumentQueue TryGetById(long documentQueueId)
        {

            return Context.PimsDocumentQueues
                .AsNoTracking()
                .FirstOrDefault(dq => dq.DocumentQueueId == documentQueueId);
        }

        /// <summary>
        /// Add Document to Queue.
        /// </summary>
        /// <param name="queuedDocument"></param>
        /// <returns></returns>
        public PimsDocumentQueue Add(PimsDocumentQueue queuedDocument)
        {
            queuedDocument.ThrowIfNull(nameof(queuedDocument));

            // Default values for new queue items.
            queuedDocument.DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PENDING.ToString();
            queuedDocument.DataSourceTypeCode = DataSourceTypes.PIMS.ToString();
            queuedDocument.MayanError = null;

            // Add
            Context.PimsDocumentQueues.Add(queuedDocument);

            return queuedDocument;
        }

        /// <summary>
        /// Find Queue Item for a Document.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public PimsDocumentQueue GetByDocumentId(long documentId)
        {
            return Context.PimsDocumentQueues.Where(x => x.DocumentId == documentId).FirstOrDefault();
        }

        /// <summary>
        /// Updates the queued document in the database.
        /// </summary>
        /// <param name="queuedDocument"></param>
        /// <returns></returns>
        public PimsDocumentQueue Update(PimsDocumentQueue queuedDocument, bool removeDocument = false)
        {
            queuedDocument.ThrowIfNull(nameof(queuedDocument));
            var existingQueuedDocument = TryGetById(queuedDocument.DocumentQueueId) ?? throw new KeyNotFoundException($"DocumentQueueId {queuedDocument.DocumentQueueId} not found.");
            if (existingQueuedDocument?.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.SUCCESS.ToString() && queuedDocument.DocumentQueueStatusTypeCode != DocumentQueueStatusTypes.SUCCESS.ToString())
            {
                throw new InvalidOperationException($"DocumentQueueId {queuedDocument.DocumentQueueId} is already completed.");
            }
            if (!removeDocument)
            {
                queuedDocument.Document = existingQueuedDocument.Document;
            }

            queuedDocument.MayanError = queuedDocument.MayanError?.Truncate(4000);
            queuedDocument.DataSourceTypeCode = existingQueuedDocument.DataSourceTypeCode; // Do not allow the data source to be updated.
            Context.Entry(existingQueuedDocument).CurrentValues.SetValues(queuedDocument);
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
        public IEnumerable<DocumentQueueSearchResult> GetAllByFilter(DocumentQueueFilter filter)
        {
            var query = Context.PimsDocumentQueues
                .Include(dq => dq.DocumentNavigation)
                .ThenInclude(d => d.DocumentType)
                .Include(dq => dq.DocumentQueueStatusTypeCodeNavigation)
                .Include(dq => dq.DataSourceTypeCodeNavigation)
                .Where(q => true).AsNoTracking();

            if (filter.DataSourceTypeCode != null)
            {
                query = query.Where(d => d.DataSourceTypeCode == filter.DataSourceTypeCode);
            }
            if (filter.DocumentQueueStatusTypeCodes != null && filter.DocumentQueueStatusTypeCodes.Length > 0)
            {
                query = query.Where(d => filter.DocumentQueueStatusTypeCodes.Any(filterStatus => d.DocumentQueueStatusTypeCode == filterStatus));
            }
            if (filter.DocProcessStartDate != null)
            {
                query = query.Where(d => d.DocProcessStartDt >= filter.DocProcessStartDate);
            }
            if (filter.DocProcessEndDate != null)
            {
                query = query.Where(d => d.DocProcessEndDt <= filter.DocProcessEndDate);
            }
            if (filter.MaxDocProcessRetries != null)
            {
                query = query.Where(d => d.DocProcessRetries == null || d.DocProcessRetries < filter.MaxDocProcessRetries);
            }

            // Return the PimsDocumentQueue search results without the file contents - to avoid memory issues.
            return query.Take(filter.Quantity).Select(dq => new DocumentQueueSearchResult()
            {
                DocumentQueueId = dq.DocumentQueueId,
                DocumentId = dq.DocumentId,
                DocumentQueueStatusTypeCode = dq.DocumentQueueStatusTypeCode,
                DocumentQueueStatusTypeCodeNavigation = dq.DocumentQueueStatusTypeCodeNavigation,
                DataSourceTypeCode = dq.DataSourceTypeCode,
                DataSourceTypeCodeNavigation = dq.DataSourceTypeCodeNavigation,
                DocumentExternalId = dq.DocumentExternalId,
                DocProcessStartDt = dq.DocProcessStartDt,
                DocProcessEndDt = dq.DocProcessEndDt,
                DocProcessRetries = dq.DocProcessRetries,
                MayanError = dq.MayanError,
                AppCreateTimestamp = dq.AppCreateTimestamp,
                AppCreateUserDirectory = dq.AppCreateUserDirectory,
                AppCreateUserGuid = dq.AppCreateUserGuid,
                AppCreateUserid = dq.AppCreateUserid,
                AppLastUpdateTimestamp = dq.AppLastUpdateTimestamp,
                AppLastUpdateUserDirectory = dq.AppLastUpdateUserDirectory,
                AppLastUpdateUserGuid = dq.AppLastUpdateUserGuid,
                AppLastUpdateUserid = dq.AppLastUpdateUserid,
                DbCreateTimestamp = dq.DbCreateTimestamp,
                DbCreateUserid = dq.DbCreateUserid,
                DbLastUpdateTimestamp = dq.DbLastUpdateTimestamp,
                DbLastUpdateUserid = dq.DbLastUpdateUserid,
                ConcurrencyControlNumber = dq.ConcurrencyControlNumber,
                DocumentSize = dq.Document != null ? dq.Document.Length : 0,
            }).ToList();
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
