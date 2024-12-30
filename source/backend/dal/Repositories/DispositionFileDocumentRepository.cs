using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// DispositionFileDocumentRepository class, provides functions to interact with document disposition files within the datasource.
    /// </summary>
    public class DispositionFileDocumentRepository : BaseRepository<PimsDispositionFileDocument>, IDispositionFileDocumentRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionFileDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DispositionFileDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<DispositionFileDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a a given disposition file.
        /// </summary>
        /// <param name="fileId">The disposition file ID.</param>
        /// <returns></returns>
        public IList<PimsDispositionFileDocument> GetAllByDispositionFile(long fileId)
        {
            return Context.PimsDispositionFileDocuments
                .Include(fd => fd.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(fd => fd.Document)
                    .ThenInclude(d => d.DocumentType)
                .Include(x => x.Document)
                    .ThenInclude(q => q.PimsDocumentQueues)
                        .ThenInclude(s => s.DocumentQueueStatusTypeCodeNavigation)
                .Where(fd => fd.DispositionFileId == fileId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed document disposition file to the database.
        /// </summary>
        /// <param name="dispositionDocument"></param>
        /// <returns></returns>
        public PimsDispositionFileDocument AddDispositionDocument(PimsDispositionFileDocument dispositionDocument)
        {
            dispositionDocument.ThrowIfNull(nameof(dispositionDocument));

            var newEntry = Context.PimsDispositionFileDocuments.Add(dispositionDocument);
            if (newEntry.State == EntityState.Added)
            {
                return newEntry.Entity;
            }
            else
            {
                throw new InvalidOperationException("Could not create document");
            }
        }

        /// <summary>
        /// Deletes the passed document disposition file in the database.
        /// </summary>
        /// <param name="dispositionDocument"></param>
        /// <returns></returns>
        public bool DeleteDispositionDocument(PimsDispositionFileDocument dispositionDocument)
        {
            if (dispositionDocument == null)
            {
                throw new ArgumentNullException(nameof(dispositionDocument), "dispositionDocument cannot be null.");
            }

            Context.PimsDispositionFileDocuments.Remove(new PimsDispositionFileDocument() { DispositionFileDocumentId = dispositionDocument.DispositionFileDocumentId });
            return true;
        }

        #endregion
    }
}
