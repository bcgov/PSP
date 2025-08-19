using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;

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

        /// <summary>
        /// Get the document from the database based on document id.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public PimsDocument TryGet(long documentId)
        {
            return this.Context.PimsDocuments.AsNoTracking().FirstOrDefault(x => x.DocumentId == documentId);
        }

        public PimsDocument TryGetDocumentRelationships(long documentId)
        {
            var documentRelationships = Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsResearchFileDocuments)
                .Include(d => d.PimsAcquisitionFileDocuments)
                .Include(d => d.PimsProjectDocuments)
                .Include(d => d.PimsFormTypes)
                .Include(d => d.PimsLeaseDocuments)
                .Include(d => d.PimsManagementFileDocuments)
                .Include(d => d.PimsMgmtActivityDocuments)
                .Include(d => d.PimsDispositionFileDocuments)
                .Where(d => d.DocumentId == documentId)
                .AsNoTracking()
                .FirstOrDefault();

            return documentRelationships;
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
            User.ThrowIfNotAuthorized(Permissions.DocumentEdit);

            document = Context.Update(document).Entity;

            return document;
        }

        /// <summary>
        /// Deletes the passed document from the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool Delete(PimsDocument document, bool commitTransaction = true)
        {
            document.ThrowIfNull(nameof(document));

            // Need to load required related entities otherwise the below foreach may fail.
            var documentToDelete = Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsResearchFileDocuments)
                .Include(d => d.PimsAcquisitionFileDocuments)
                .Include(d => d.PimsProjectDocuments)
                .Include(d => d.PimsFormTypes)
                .Include(d => d.PimsLeaseDocuments)
                .Include(d => d.PimsManagementFileDocuments)
                .Include(d => d.PimsMgmtActivityDocuments)
                .Include(d => d.PimsDispositionFileDocuments)
                .Where(d => d.DocumentId == document.Internal_Id)
                .FirstOrDefault();

            if(documentToDelete.PimsResearchFileDocuments.Count > 0)
            {
                Context.PimsResearchFileDocuments.RemoveRange(documentToDelete.PimsResearchFileDocuments);
            }

            if (documentToDelete.PimsAcquisitionFileDocuments.Count > 0)
            {
                Context.PimsAcquisitionFileDocuments.RemoveRange(documentToDelete.PimsAcquisitionFileDocuments);
            }

            if(documentToDelete.PimsProjectDocuments.Count > 0)
            {
                Context.PimsProjectDocuments.RemoveRange(documentToDelete.PimsProjectDocuments);
            }

            if(documentToDelete.PimsLeaseDocuments.Count > 0)
            {
                Context.PimsLeaseDocuments.RemoveRange(documentToDelete.PimsLeaseDocuments);
            }

            if (documentToDelete.PimsManagementFileDocuments.Count > 0)
            {
                Context.PimsManagementFileDocuments.RemoveRange(documentToDelete.PimsManagementFileDocuments);
            }

            if (documentToDelete.PimsMgmtActivityDocuments.Count > 0)
            {
                Context.PimsMgmtActivityDocuments.RemoveRange(documentToDelete.PimsMgmtActivityDocuments);
            }

            if(documentToDelete.PimsDispositionFileDocuments.Count > 0)
            {
                Context.PimsDispositionFileDocuments.RemoveRange(documentToDelete.PimsDispositionFileDocuments);
            }

            foreach (var pimsFormTypeDocument in documentToDelete.PimsFormTypes.ToList())
            {
                var updatedFormType = pimsFormTypeDocument;
                updatedFormType.DocumentId = null;
                Context.Entry(updatedFormType).Property(x => x.DocumentId).IsModified = true;
            }

            if (commitTransaction)
            {
                Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.
            }

            Context.PimsDocuments.Remove(documentToDelete);

            return true;
        }

        /// <summary>
        /// Deletes the passed document from the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool DeleteDocument(PimsDocument document)
        {
            document.ThrowIfNull(nameof(document));

            Context.PimsDocuments.Remove(document);

            return true;
        }

        public int DocumentRelationshipCount(long documentId)
        {
            var documentRelationships = Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsResearchFileDocuments)
                .Include(d => d.PimsAcquisitionFileDocuments)
                .Include(d => d.PimsProjectDocuments)
                .Include(d => d.PimsFormTypes)
                .Include(d => d.PimsLeaseDocuments)
                .Include(d => d.PimsManagementFileDocuments)
                .Include(d => d.PimsMgmtActivityDocuments)
                .Include(d => d.PimsDispositionFileDocuments)
                .Where(d => d.DocumentId == documentId)
                .AsNoTracking()
                .FirstOrDefault();

            return documentRelationships.PimsResearchFileDocuments.Count +
                    documentRelationships.PimsAcquisitionFileDocuments.Count +
                    documentRelationships.PimsProjectDocuments.Count +
                    documentRelationships.PimsFormTypes.Count +
                    documentRelationships.PimsLeaseDocuments.Count +
                    documentRelationships.PimsManagementFileDocuments.Count +
                    documentRelationships.PimsMgmtActivityDocuments.Count +
                    documentRelationships.PimsDispositionFileDocuments.Count;
        }

        #endregion
    }
}
