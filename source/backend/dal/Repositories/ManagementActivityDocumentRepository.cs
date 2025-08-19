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
    /// ManagementActivityDocumentRepository class, provides a service layer to interact with document activity files within the datasource.
    /// </summary>
    public class ManagementActivityDocumentRepository : BaseRepository<PimsMgmtActivityDocument>, IDocumentRelationshipRepository<PimsMgmtActivityDocument>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ManagementActivityDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ManagementActivityDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ManagementActivityDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a a given property activity.
        /// </summary>
        /// <returns></returns>
        public IList<PimsMgmtActivityDocument> GetAllByParentId(long parentId)
        {
            return Context.PimsMgmtActivityDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Include(x => x.Document)
                    .ThenInclude(q => q.PimsDocumentQueues)
                        .ThenInclude(s => s.DocumentQueueStatusTypeCodeNavigation)
                .Where(ad => ad.ManagementActivityId == parentId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed property document activity file to the database.
        /// </summary>
        /// <param name="managementActivityDocument"></param>
        /// <returns></returns>
        public PimsMgmtActivityDocument AddDocument(PimsMgmtActivityDocument managementActivityDocument)
        {
            managementActivityDocument.ThrowIfNull(nameof(managementActivityDocument));

            var newEntry = Context.PimsMgmtActivityDocuments.Add(managementActivityDocument);
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
        /// Deletes the passed property document activity file in the database.
        /// </summary>
        /// <param name="managementActivityDocument"></param>
        /// <returns></returns>
        public bool DeleteDocument(PimsMgmtActivityDocument managementActivityDocument)
        {
            if (managementActivityDocument == null)
            {
                throw new ArgumentNullException(nameof(managementActivityDocument), "managementActivityDocument cannot be null.");
            }

            Context.PimsMgmtActivityDocuments.Remove(new PimsMgmtActivityDocument() { MgmtActivityDocumentId = managementActivityDocument.ManagementActivityId });

            return true;
        }

        #endregion
    }
}
