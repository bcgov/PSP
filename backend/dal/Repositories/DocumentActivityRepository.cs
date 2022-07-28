using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// DocumentActivityRepository class, provides a service layer to interact with document types within the datasource.
    /// </summary>
    public class DocumentActivityRepository : BaseRepository<PimsActivityInstanceDocument>, IDocumentActivityRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a DocumentActivityRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public DocumentActivityRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<DocumentActivityRepository> logger, IMapper mapper)
            : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods


        /// <summary>
        /// Get a list of all the documents for the given activity.
        /// </summary>
        /// <returns></returns>
        public IList<PimsActivityInstanceDocument> GetAll(long activityId)
        {
            return this.Context.PimsActivityInstanceDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Where(ad => ad.ActivityInstanceId == activityId)
                .ToList();
        }

        /// <summary>
        /// Adds the passed document activity to the database.
        /// </summary>
        /// <param name="activityDocument"></param>
        /// <returns></returns>
        public PimsActivityInstanceDocument Add(PimsActivityInstanceDocument activityDocument)
        {
            if (activityDocument == null)
            {
                throw new ArgumentNullException(nameof(activityDocument), "documentActivity cannot be null.");
            }

            var newEntry = this.Context.PimsActivityInstanceDocuments.Add(activityDocument);
            return newEntry.Entity;
        }

        /// <summary>
        /// Deletes the passed document activity to the database.
        /// </summary>
        /// <param name="activityDocument"></param>
        /// <returns></returns>
        public bool Delete(PimsActivityInstanceDocument activityDocument)
        {
            if (activityDocument == null)
            {
                throw new ArgumentNullException(nameof(activityDocument), "activityDocument cannot be null.");
            }

            this.Context.PimsActivityInstanceDocuments.Remove(activityDocument);
            return true;
        }


        #endregion
    }
}