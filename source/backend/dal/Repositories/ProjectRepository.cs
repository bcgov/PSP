using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    public class ProjectRepository : BaseRepository<PimsProject>, IProjectRepository
    {
        /// <summary>
        /// Creates a new instance of a ProjectRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ProjectRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ProjectRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        /// <summary>
        /// Returns the total number of projects in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Context.PimsProjects.Count();
        }

        public Task<Paged<PimsProject>> GetPageAsync(ProjectFilter filter)
        {
            User.ThrowIfNotAuthorized(Permissions.ProjectView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            return GetPage(filter);
        }

        private async Task<Paged<PimsProject>> GetPage(ProjectFilter filter)
        {
            var query = Context.PimsProjects.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.ProjectNumber))
            {
                query = query.Where(x => EF.Functions.Like(x.Code.ToString(), $"%{filter.ProjectNumber}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectName))
            {
                query = query.Where(x => EF.Functions.Like(x.Description, $"%{filter.ProjectName}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectStatusCode))
            {
                query = query.Where(x => x.ProjectStatusTypeCodeNavigation.ProjectStatusTypeCode == filter.ProjectStatusCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectRegionCode))
            {
                query = query.Where(x => x.RegionCode.ToString() == filter.ProjectRegionCode);
            }

            if (filter.Sort?.Any() == true)
            {
                query = query.OrderByProperty(filter.Sort);
            }

            if (filter.Sort?.Any() == true)
            {
                query = query.OrderByProperty(filter.Sort);
            }
            else
            {
                query = query.OrderBy(l => l.Code);
            }

            var skip = (filter.Page - 1) * filter.Quantity;

            var items = await query
                .Include(x => x.ProjectStatusTypeCodeNavigation)
                .Skip(skip)
                .Take(filter.Quantity)
                .ToListAsync();

            return new Paged<PimsProject>(items, filter.Page, filter.Quantity, query.Count());
        }
    }
}
