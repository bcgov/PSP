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
    /// ProjectDocumentRepository class, provides a service layer to interact with document project within the datasource.
    /// </summary>
    public class ProjectDocumentRepository : BaseRepository<PimsProjectDocument>, IDocumentRelationshipRepository<PimsProjectDocument>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ProjectDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ProjectDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ProjectDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a given project.
        /// </summary>
        /// <returns></returns>
        public IList<PimsProjectDocument> GetAllByParentId(long parentId)
        {
            return this.Context.PimsProjectDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Include(ad => ad.Document)
                    .ThenInclude(q => q.PimsDocumentQueues)
                        .ThenInclude(s => s.DocumentQueueStatusTypeCodeNavigation)
                .Where(ad => ad.ProjectId == parentId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed document project to the database.
        /// </summary>
        /// <param name="projectDocument"></param>
        /// <returns></returns>
        public PimsProjectDocument AddDocument(PimsProjectDocument projectDocument)
        {
            projectDocument.ThrowIfNull(nameof(projectDocument));

            var newEntry = this.Context.PimsProjectDocuments.Add(projectDocument);
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
        /// Deletes the passed document project in the database.
        /// </summary>
        /// <param name="projectDocument"></param>
        /// <returns></returns>
        public bool DeleteDocument(PimsProjectDocument projectDocument)
        {
            if (projectDocument == null)
            {
                throw new ArgumentNullException(nameof(projectDocument), "projectDocument cannot be null.");
            }

            this.Context.PimsProjectDocuments.Remove(new PimsProjectDocument() { ProjectDocumentId = projectDocument.ProjectDocumentId });
            return true;
        }

        #endregion
    }
}
