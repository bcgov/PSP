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
    /// PropertyDocumentRepository class, provides a service layer to interact with document properties within the datasource.
    /// </summary>
    public class PropertyDocumentRepository : BaseRepository<PimsPropertyDocument>, IDocumentRelationshipRepository<PimsPropertyDocument>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a given property.
        /// </summary>
        /// <returns></returns>
        public IList<PimsPropertyDocument> GetAllByParentId(long parentId)
        {
            /*
            return this.Context.PimsPropertyDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Include(ad => ad.Document)
                    .ThenInclude(q => q.PimsDocumentQueues)
                        .ThenInclude(s => s.DocumentQueueStatusTypeCodeNavigation)
                .Where(ad => ad.PropertyId == parentId)
                .AsNoTracking()
                .ToList();
                */

            return new List<PimsPropertyDocument>();
        }

        /// <summary>
        /// Adds the passed document property to the database.
        /// </summary>
        /// <param name="propertyDocument"></param>
        /// <returns></returns>
        public PimsPropertyDocument AddDocument(PimsPropertyDocument propertyDocument)
        {
            propertyDocument.ThrowIfNull(nameof(propertyDocument));

            /*
                        var newEntry = this.Context.PimsPropertyDocuments.Add(propertyDocument);
                        if (newEntry.State == EntityState.Added)
                        {
                            return newEntry.Entity;
                        }
                        else
                        {
                            throw new InvalidOperationException("Could not create document");
                        }
                        */
            return new PimsPropertyDocument();
        }

        /// <summary>
        /// Deletes the passed document property in the database.
        /// </summary>
        /// <param name="propertyDocument"></param>
        /// <returns></returns>
        public bool DeleteDocument(PimsPropertyDocument propertyDocument)
        {
            if (propertyDocument == null)
            {
                throw new ArgumentNullException(nameof(propertyDocument), "propertyDocument cannot be null.");
            }

            /*
            this.Context.PimsPropertyDocuments.Remove(new PimsPropertyDocument() { PropertyDocumentId = propertyDocument.PropertyDocumentId });
            */
            return true;
        }

        #endregion
    }
}
