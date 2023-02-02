using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// DocumentRepository class, provides a repository to interact with documents within the datasource.
    /// </summary>
    public class DocumentRepository : BaseRepository<PimsDocument>, IDocumentRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentActivityRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DocumentRepository(
            PimsContext dbContext,
            ClaimsPrincipal user,
            ILogger<DocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        public int GetTotalRelationCount(long documentId)
        {
            var document = this.Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsActivityInstanceDocuments)
                .Where(d => d.DocumentId == documentId)
                .AsNoTracking()
                .FirstOrDefault();

            // Add all document relationships
            return document.PimsActivityInstanceDocuments.Count;
        }

        /// <summary>
        /// Get the document from the database based on document id.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public PimsDocument TryGet(long documentId)
        {
            return this.Context.PimsDocuments.FirstOrDefault(x => x.DocumentId == documentId);
        }

        /// <summary>
        /// Adds the passed document to the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public PimsDocument Add(PimsDocument document)
        {
            document.ThrowIfNull(nameof(document));

            var newEntry = this.Context.PimsDocuments.Add(document);
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
        /// Updates the passed document in the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public PimsDocument Update(PimsDocument document, bool commitTransaction = true)
        {
            document.ThrowIfNull(nameof(document));

            this.User.ThrowIfNotAuthorized(Permissions.DocumentEdit);
            document = Context.Update(document).Entity;
            return document;
        }

        /// <summary>
        /// Deletes the passed document from the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool Delete(PimsDocument document)
        {
            document.ThrowIfNull(nameof(document));

            // Need to load required related entities otherwise the below foreach may fail.
            var documentToDelete = this.Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsActivityInstanceDocuments)
                .Include(d => d.PimsActivityTemplateDocuments)
                .Include(d => d.PimsResearchFileDocuments)
                .Include(d => d.PimsAcquisitionFileDocuments)
                .Where(d => d.DocumentId == document.Id)
                .AsNoTracking()
                .FirstOrDefault();

            foreach (var pimsResearchFileDocument in documentToDelete.PimsResearchFileDocuments)
            {
                this.Context.PimsResearchFileDocuments.Remove(new PimsResearchFileDocument() { Id = pimsResearchFileDocument.Id });
            }

            foreach (var pimsAcquisitionFileDocument in documentToDelete.PimsAcquisitionFileDocuments)
            {
                this.Context.PimsAcquisitionFileDocuments.Remove(new PimsAcquisitionFileDocument() { Id = pimsAcquisitionFileDocument.Id });
            }

            foreach (var pimsActivityInstanceDocument in documentToDelete.PimsActivityInstanceDocuments)
            {
                this.Context.PimsActivityInstanceDocuments.Remove(new PimsActivityInstanceDocument() { Id = pimsActivityInstanceDocument.Id });
            }

            foreach (var pimsTemplateDocument in documentToDelete.PimsActivityTemplateDocuments)
            {
                this.Context.PimsActivityTemplateDocuments.Remove(new PimsActivityTemplateDocument() { Id = pimsTemplateDocument.Id });
            }

            this.Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

            this.Context.PimsDocuments.Remove(new PimsDocument() { Id = document.Id });
            return true;
        }

        #endregion
    }
}
