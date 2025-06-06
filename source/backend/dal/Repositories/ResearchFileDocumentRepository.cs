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
    /// ResearchFileDocumentRepository class, provides a service layer to interact with document research files within the datasource.
    /// </summary>
    public class ResearchFileDocumentRepository : BaseRepository<PimsResearchFileDocument>, IDocumentRelationshipRepository<PimsResearchFileDocument>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFileDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ResearchFileDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ResearchFileDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a given research file.
        /// </summary>
        /// <returns></returns>
        public IList<PimsResearchFileDocument> GetAllByParentId(long parentId)
        {
            return this.Context.PimsResearchFileDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Include(ad => ad.Document)
                    .ThenInclude(q => q.PimsDocumentQueues)
                        .ThenInclude(s => s.DocumentQueueStatusTypeCodeNavigation)
                .Where(ad => ad.ResearchFileId == parentId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed document research file to the database.
        /// </summary>
        /// <param name="researchDocument"></param>
        /// <returns></returns>
        public PimsResearchFileDocument AddDocument(PimsResearchFileDocument researchDocument)
        {
            researchDocument.ThrowIfNull(nameof(researchDocument));

            var newEntry = this.Context.PimsResearchFileDocuments.Add(researchDocument);
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
        /// Deletes the passed document research file in the database.
        /// </summary>
        /// <param name="researchDocument"></param>
        /// <returns></returns>
        public bool DeleteDocument(PimsResearchFileDocument researchDocument)
        {
            if (researchDocument == null)
            {
                throw new ArgumentNullException(nameof(researchDocument), "researchDocument cannot be null.");
            }

            this.Context.PimsResearchFileDocuments.Remove(new PimsResearchFileDocument() { ResearchFileDocumentId = researchDocument.ResearchFileDocumentId });
            return true;
        }

        #endregion
    }
}
