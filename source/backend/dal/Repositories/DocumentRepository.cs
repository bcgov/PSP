using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                .Include(d => d.PimsProjectDocuments)
                .Where(d => d.DocumentId == document.Internal_Id)
                .AsNoTracking()
                .FirstOrDefault();

            foreach (var pimsResearchFileDocument in documentToDelete.PimsResearchFileDocuments)
            {
                this.Context.PimsResearchFileDocuments.Remove(new PimsResearchFileDocument() { Internal_Id = pimsResearchFileDocument.Internal_Id });
            }

            foreach (var pimsAcquisitionFileDocument in documentToDelete.PimsAcquisitionFileDocuments)
            {
                this.Context.PimsAcquisitionFileDocuments.Remove(new PimsAcquisitionFileDocument() { Internal_Id = pimsAcquisitionFileDocument.Internal_Id });
            }

            foreach (var pimsProjectDocument in documentToDelete.PimsProjectDocuments)
            {
                this.Context.PimsProjectDocuments.Remove(new PimsProjectDocument() { Internal_Id = pimsProjectDocument.Internal_Id });
            }

            foreach (var pimsActivityInstanceDocument in documentToDelete.PimsActivityInstanceDocuments)
            {
                this.Context.PimsActivityInstanceDocuments.Remove(new PimsActivityInstanceDocument() { Internal_Id = pimsActivityInstanceDocument.Internal_Id });
            }

            foreach (var pimsTemplateDocument in documentToDelete.PimsActivityTemplateDocuments)
            {
                this.Context.PimsActivityTemplateDocuments.Remove(new PimsActivityTemplateDocument() { Internal_Id = pimsTemplateDocument.Internal_Id });
            }

            this.Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

            this.Context.PimsDocuments.Remove(new PimsDocument() { Internal_Id = document.Internal_Id });
            return true;
        }

        public List<PimsDocument> GetAllByDocumentType(string documentType)
        {
            return this.Context.PimsDocuments
                .Include(d => d.DocumentType)
                .Where(d => d.DocumentType.DocumentType == documentType)
                .AsNoTracking()
                .ToList();
        }

        public int DocumentRelationshipCount(long documentId)
        {
            var documentRelationships = this.Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsActivityInstanceDocuments)
                .Include(d => d.PimsActivityTemplateDocuments)
                .Include(d => d.PimsResearchFileDocuments)
                .Include(d => d.PimsAcquisitionFileDocuments)
                .Include(d => d.PimsProjectDocuments)
                .Where(d => d.DocumentId == documentId)
                .AsNoTracking()
                .FirstOrDefault();

            return documentRelationships.PimsResearchFileDocuments.Count +
                    documentRelationships.PimsAcquisitionFileDocuments.Count +
                    documentRelationships.PimsProjectDocuments.Count +
                    documentRelationships.PimsActivityInstanceDocuments.Count +
                    documentRelationships.PimsActivityTemplateDocuments.Count;

        }

        #endregion
    }
}
