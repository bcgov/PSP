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
    /// PropertyActivityDocumentRepository class, provides a service layer to interact with document acquisition files within the datasource.
    /// </summary>
    public class PropertyActivityDocumentRepository : BaseRepository<PimsPropertyActivityDocument>, IPropertyActivityDocumentRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyActivityDocumentRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyActivityDocumentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AcquisitionFileDocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document file relationships for a a given acquisition file.
        /// </summary>
        /// <returns></returns>
        public IList<PimsPropertyActivityDocument> GetAllByPropertyActivityFile(long fileId)
        {
            return this.Context.PimsPropertyActivityDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Where(ad => ad.PimsPropertyActivityId == fileId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Adds the passed property document activity file to the database.
        /// </summary>
        /// <param name="propertyActivityDocument"></param>
        /// <returns></returns>
        public PimsPropertyActivityDocument AddPropertyActivityDocument(PimsPropertyActivityDocument propertyActivityDocument)
        {
            propertyActivityDocument.ThrowIfNull(nameof(propertyActivityDocument));

            var newEntry = this.Context.PimsPropertyActivityDocuments.Add(propertyActivityDocument);
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
        /// Deletes the passed property document activity file in the database.
        /// </summary>
        /// <param name="propertyActivityDocument"></param>
        /// <returns></returns>
        public bool DeletePropertyActivityDocument(PimsPropertyActivityDocument propertyActivityDocument)
        {
            if (propertyActivityDocument == null)
            {
                throw new ArgumentNullException(nameof(propertyActivityDocument), "propertyActivityDocument cannot be null.");
            }

            this.Context.PimsPropertyActivityDocuments.Remove(new PimsPropertyActivityDocument() { PropertyActivityDocumentId = propertyActivityDocument.PropertyActivityDocumentId });
            return true;
        }

        #endregion
    }
}
