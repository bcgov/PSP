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
    /// ManagementFileDocumentRepository class, provides a service layer to interact with document management files within the datasource.
    /// </summary>
    public class ManagementFileDocumentRepository : BaseRepository<PimsManagementFileDocument>, IManagementFileDocumentRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ManagementFileDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ManagementFileDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ManagementFileDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a given management file.
        /// </summary>
        /// <param name="fileId">The management file ID.</param>
        /// <returns></returns>
        public IList<PimsManagementFileDocument> GetAllByManagementFile(long fileId)
        {
            return Context.PimsManagementFileDocuments
                .Include(fd => fd.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(fd => fd.Document)
                    .ThenInclude(d => d.DocumentType)
                .Include(x => x.Document)
                    .ThenInclude(q => q.PimsDocumentQueues)
                        .ThenInclude(s => s.DocumentQueueStatusTypeCodeNavigation)
                .Where(fd => fd.ManagementFileId == fileId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed property document management file to the database.
        /// </summary>
        /// <param name="managementDocument"></param>
        /// <returns></returns>
        public PimsManagementFileDocument AddManagementFileDocument(PimsManagementFileDocument managementDocument)
        {
            managementDocument.ThrowIfNull(nameof(managementDocument));

            var newEntry = Context.PimsManagementFileDocuments.Add(managementDocument);
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
        /// Deletes the passed property document management file in the database.
        /// </summary>
        /// <param name="managementDocument"></param>
        /// <returns></returns>
        public bool DeleteManagementFileDocument(PimsManagementFileDocument managementDocument)
        {
            if (managementDocument == null)
            {
                throw new ArgumentNullException(nameof(managementDocument), "managementDocument cannot be null.");
            }

            Context.PimsManagementFileDocuments.Remove(new PimsManagementFileDocument() { ManagementFileDocumentId = managementDocument.ManagementFileDocumentId });
            return true;
        }

        #endregion
    }
}
