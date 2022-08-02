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
    /// DocumentActivityRepository class, provides a service layer to interact with document types within the datasource.
    /// </summary>
    public class DocumentActivityRepository : BaseRepository<PimsActivityInstanceDocument>, IDocumentActivityRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a DocumentActivityRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DocumentActivityRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<DocumentActivityRepository> logger)
            : base(dbContext, user, logger) { }
        #endregion

        #region Methods


        /// <summary>
        /// Get a list of all the document relationships for a given document.
        /// </summary>
        /// <returns></returns>
        public IList<PimsActivityInstanceDocument> GetAllByDocument(long documentId)
        {
            return this.Context.PimsActivityInstanceDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Where(ad => ad.DocumentId == documentId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Get a list of all the document relationships for a given activity.
        /// </summary>
        /// <returns></returns>
        public IList<PimsActivityInstanceDocument> GetAllByActivity(long activityId)
        {
            return this.Context.PimsActivityInstanceDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Where(ad => ad.ActivityInstanceId == activityId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed document activity to the database.
        /// </summary>
        /// <param name="activityDocument"></param>
        /// <returns></returns>
        public PimsActivityInstanceDocument Add(PimsActivityInstanceDocument activityDocument)
        {
            activityDocument.ThrowIfNull(nameof(activityDocument));

            var newEntry = this.Context.PimsActivityInstanceDocuments.Add(activityDocument);
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
        /// Deletes the passed document activity to the database.
        /// </summary>
        /// <param name="activityDocument"></param>
        /// <returns></returns>
        public bool Delete(PimsActivityInstanceDocument activityDocument)
        {
            if (activityDocument == null)
            {
                throw new ArgumentNullException(nameof(activityDocument), "activityDocument cannot be null.");
            }

            this.Context.PimsActivityInstanceDocuments.Remove(activityDocument);
            return true;
        }


        #endregion
    }
}