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
    /// LeaseDocumentRepository class, provides a service layer to interact with document lease files within the datasource.
    /// </summary>
    public class LeaseDocumentRepository : BaseRepository<PimsLeaseDocument>, IDocumentRelationshipRepository<PimsLeaseDocument>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeaseDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a given lease file.
        /// </summary>
        /// <param name="parentId">The lease file ID.</param>
        /// <returns></returns>
        public IList<PimsLeaseDocument> GetAllByParentId(long parentId)
        {
            return Context.PimsLeaseDocuments
                .Include(fd => fd.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(fd => fd.Document)
                    .ThenInclude(d => d.DocumentType)
                .Include(x => x.Document)
                    .ThenInclude(q => q.PimsDocumentQueues)
                        .ThenInclude(s => s.DocumentQueueStatusTypeCodeNavigation)
                .Where(fd => fd.LeaseId == parentId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed property document lease file to the database.
        /// </summary>
        /// <param name="leaseDocument"></param>
        /// <returns></returns>
        public PimsLeaseDocument AddDocument(PimsLeaseDocument leaseDocument)
        {
            leaseDocument.ThrowIfNull(nameof(leaseDocument));

            var newEntry = Context.PimsLeaseDocuments.Add(leaseDocument);
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
        /// Deletes the passed property document lease file in the database.
        /// </summary>
        /// <param name="leaseDocument"></param>
        /// <returns></returns>
        public bool DeleteDocument(PimsLeaseDocument leaseDocument)
        {
            if (leaseDocument == null)
            {
                throw new ArgumentNullException(nameof(leaseDocument), "leaseDocument cannot be null.");
            }

            Context.PimsLeaseDocuments.Remove(new PimsLeaseDocument() { LeaseDocumentId = leaseDocument.LeaseDocumentId });
            return true;
        }

        #endregion
    }
}
