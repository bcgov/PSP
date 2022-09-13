using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public PimsDocument Get(long documentId)
        {
            return this.Context.PimsDocuments.FirstOrDefault(x => x.DocumentId == documentId);
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

            this.Context.PimsDocuments.Remove(document);
            return true;
        }

        #endregion
    }
}
