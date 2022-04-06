using System;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ResearchRepository class, provides a service layer to interact with research files within the datasource.
    /// </summary>
    public class ResearchRepository : BaseRepository<PimsResearchFile>, IResearchRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeaseRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ResearchRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<ResearchRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the total number of research files in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.PimsResearchFiles.Count();
        }

        /// <summary>
        /// Get a page with an array of leases within the specified filters.
        /// Note that the 'researchFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsResearchFile> GetPage(ResearchFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid()) throw new ArgumentException("Argument must have a valid filter", nameof(filter));

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = this.Context.GenerateResearchQuery(filter);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();


            return new Paged<PimsResearchFile>(items, filter.Page, filter.Quantity, query.Count());
        }
        #endregion
    }
}
