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
    /// DocumentActivityTemplateRepository class, provides a service layer to interact with activity template documents within the datasource.
    /// </summary>
    public class DocumentActivityTemplateRepository : BaseRepository<PimsActivityTemplateDocument>, IDocumentActivityTemplateRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentActivityTemplateRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DocumentActivityTemplateRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<DocumentActivityRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document relationships for a given document.
        /// </summary>
        /// <returns></returns>
        public IList<PimsActivityTemplateDocument> GetAllByDocument(long documentId)
        {
            return this.Context.PimsActivityTemplateDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Where(ad => ad.DocumentId == documentId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Get a list of all the document relationships for a given activity template.
        /// </summary>
        /// <returns></returns>
        public IList<PimsActivityTemplateDocument> GetAllByActivityTemplate(long activityTemplateId)
        {
            return this.Context.PimsActivityTemplateDocuments
                .Include(at => at.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(at => at.Document)
                    .ThenInclude(d => d.DocumentType)
                .Where(at => at.ActivityTemplateId == activityTemplateId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed document activity template to the database.
        /// </summary>
        /// <param name="activityTemplateDocument"></param>
        /// <returns></returns>
        public PimsActivityTemplateDocument Add(PimsActivityTemplateDocument activityTemplateDocument)
        {
            activityTemplateDocument.ThrowIfNull(nameof(activityTemplateDocument));

            var newEntry = this.Context.PimsActivityTemplateDocuments.Add(activityTemplateDocument);
            if (newEntry.State == EntityState.Added)
            {
                return newEntry.Entity;
            }
            else
            {
                throw new InvalidOperationException("Could not create activity template document");
            }
        }

        /// <summary>
        /// Deletes the passed document activity template to the database.
        /// </summary>
        /// <param name="activityTemplateDocument"></param>
        /// <returns></returns>
        public bool Delete(PimsActivityTemplateDocument activityTemplateDocument)
        {
            if (activityTemplateDocument == null)
            {
                throw new ArgumentNullException(nameof(activityTemplateDocument), "activityTemplateDocument cannot be null.");
            }

            this.Context.PimsActivityTemplateDocuments.Remove(new PimsActivityTemplateDocument()
            {
                ActivityTemplateId = activityTemplateDocument.ActivityTemplateId,
            });
            return true;
        }

        #endregion
    }
}
