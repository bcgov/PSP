using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// DocumentTypeRepository class, provides a service layer to interact with document types within the datasource.
    /// </summary>
    public class DocumentTypeRepository : BaseRepository<PimsDocumentTyp>, IDocumentTypeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentTypeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DocumentTypeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<DocumentTypeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the PimsDocumetType by Id.
        /// </summary>
        /// <param name="id">The Document Type Id.</param>
        /// <returns></returns>
        public PimsDocumentTyp GetById(long id)
        {
            return this.Context.PimsDocumentTyps.AsNoTracking()
                .Where(x => x.DocumentTypeId == id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get a list of all the document types.
        /// </summary>
        /// <returns></returns>
        public IList<PimsDocumentTyp> GetAll()
        {
            return this.Context.PimsDocumentTyps.AsNoTracking()
                .Include(dt => dt.PimsDocumentCategorySubtypes)
                .OrderBy(dt => dt.DisplayOrder)
                .ToList();
        }

        /// <summary>
        /// Get a list of all the document types for a category type.
        /// </summary>
        /// <returns></returns>
        public IList<PimsDocumentTyp> GetByCategory(string category)
        {
            return this.Context.PimsDocumentTyps.AsNoTracking()
                .Where(dt => dt.PimsDocumentCategorySubtypes.Any(dcs => dcs.DocumentCategoryTypeCode == category))
                .OrderBy(dt => dt.DisplayOrder).ToList();
        }

        /// <summary>
        /// Adds the passed document type to the database.
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public PimsDocumentTyp Add(PimsDocumentTyp documentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType), "document type cannot be null.");
            }

            var newDocumentType = this.Context.PimsDocumentTyps.Add(documentType);
            return newDocumentType.Entity;
        }

        /// <summary>
        /// Updates the passed document type within the database.
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public PimsDocumentTyp Update(PimsDocumentTyp documentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType), "document type cannot be null.");
            }

            var existingDocumentType =
                this.Context.PimsDocumentTyps.FirstOrDefault(dt => documentType.DocumentTypeId == dt.DocumentTypeId) ?? throw new KeyNotFoundException($"Failed to find documentType for mayan ID: {documentType.MayanId}");

            existingDocumentType.DocumentTypeDescription = documentType.DocumentTypeDescription;
            existingDocumentType.DocumentTypeDefinition = documentType.DocumentTypeDefinition;
            existingDocumentType.MayanId = documentType.MayanId;
            existingDocumentType.DisplayOrder = documentType.DisplayOrder;
            existingDocumentType.IsDisabled = documentType.IsDisabled;

            this.Context.UpdateChild<PimsDocumentTyp, long, PimsDocumentCategorySubtype, long>(l => l.PimsDocumentCategorySubtypes, documentType.DocumentTypeId, documentType.PimsDocumentCategorySubtypes.ToArray());

            this.Context.Update(existingDocumentType);
            return existingDocumentType;
        }
        #endregion
    }
}
