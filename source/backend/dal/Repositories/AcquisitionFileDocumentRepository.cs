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
    /// AcquisitionFileDocumentRepository class, provides a service layer to interact with document acquisition files within the datasource.
    /// </summary>
    public class AcquisitionFileDocumentRepository : BaseRepository<PimsAcquisitionFileDocument>, IDocumentRelationshipRepository<PimsAcquisitionFileDocument>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFileDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AcquisitionFileDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AcquisitionFileDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a a given acquisition file.
        /// </summary>
        /// <returns></returns>
        public IList<PimsAcquisitionFileDocument> GetAllByParentId(long parentId)
        {
            return this.Context.PimsAcquisitionFileDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Include(ad => ad.Document)
                    .ThenInclude(q => q.PimsDocumentQueues)
                        .ThenInclude(s => s.DocumentQueueStatusTypeCodeNavigation)
                .Where(ad => ad.AcquisitionFileId == parentId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed document acquisition file to the database.
        /// </summary>
        /// <param name="acquisitionDocument"></param>
        /// <returns></returns>
        public PimsAcquisitionFileDocument AddDocument(PimsAcquisitionFileDocument acquisitionDocument)
        {
            acquisitionDocument.ThrowIfNull(nameof(acquisitionDocument));

            var newEntry = this.Context.PimsAcquisitionFileDocuments.Add(acquisitionDocument);
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
        /// Deletes the passed document acquisition file in the database.
        /// </summary>
        /// <param name="acquisitionDocument"></param>
        /// <returns></returns>
        public bool DeleteDocument(PimsAcquisitionFileDocument acquisitionDocument)
        {
            if (acquisitionDocument == null)
            {
                throw new ArgumentNullException(nameof(acquisitionDocument), "acquisitionDocument cannot be null.");
            }

            this.Context.PimsAcquisitionFileDocuments.Remove(new PimsAcquisitionFileDocument() { AcquisitionFileDocumentId = acquisitionDocument.AcquisitionFileDocumentId });
            return true;
        }
        #endregion
    }
}
