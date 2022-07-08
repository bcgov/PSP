using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

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
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public DocumentTypeRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<DocumentTypeRepository> logger, IMapper mapper)
            : base(dbContext, user, service, logger, mapper)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the document types.
        /// </summary>
        /// <returns></returns>
        public IList<PimsDocumentTyp> GetAll()
        {
            return this.Context.PimsDocumentTyps.OrderBy(dt => dt.DocumentTypeId).ToList();
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
        #endregion
    }
}