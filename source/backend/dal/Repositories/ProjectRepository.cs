using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Provides a repository to interact with projects within the datasource.
    /// </summary>
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
        /// Retrieves the matching projects to the given filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="regions"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public IList<PimsProject> SearchProjects(string filter, HashSet<short> regions, int maxResults)
        {
            // business requirement - limit search results to user's assigned region(s)
            return this.Context.PimsProjects.AsNoTracking()
                .Where(p => EF.Functions.Like(p.Description, $"%{filter}%"))
                .Where(p => regions.Contains(p.RegionCode))
                .OrderBy(a => a.Code)
                .Take(maxResults)
                .ToArray();
        }

        /// <summary>
        /// Returns the total number of projects in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Context.PimsProjects.Count();
        }

        /// <summary>
        /// Returns a Paged Result of Projects based on ProjectFilter params.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get by ID - Search Projects by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsProject Get(long id)
        {
            User.ThrowIfNotAuthorized(Permissions.ProjectView);

            return Context.PimsProjects
                    .AsNoTracking()
                    .Include(x => x.PimsProducts)
                    .Include(x => x.ProjectStatusTypeCodeNavigation)
                    .Include(x => x.RegionCodeNavigation)
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Add Project to Context.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public PimsProject Add(PimsProject project)
        {
            User.ThrowIfNotAuthorized(Permissions.ProjectAdd);

            foreach (var product in project.PimsProducts)
            {
                product.ParentProject = project;
            }

            Context.PimsProjects.Add(project);
            return project;
        }

        public PimsProject Update(PimsProject project)
        {
            using var queryScope = Logger.QueryScope();
            project.ThrowIfNull(nameof(project));

            var existingProject = Context.PimsProjects
                .FirstOrDefault(x => x.Id == project.Id) ?? throw new KeyNotFoundException();

            this.Context.UpdateChild<PimsProject, long, PimsProduct>(p => p.PimsProducts, project.Id, project.PimsProducts.ToArray(), true);

            Context.Entry(existingProject).CurrentValues.SetValues(project);

            return project;
        }

        private async Task<Paged<PimsProject>> GetPage(ProjectFilter filter)
        {
            var query = Context.PimsProjects.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.ProjectNumber))
            {
                query = query.Where(x => EF.Functions.Like(x.Code, $"%{filter.ProjectNumber}%"));
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
                var direction = filter.Sort[0].Split(" ").LastOrDefault();
                if (filter.Sort[0].StartsWith("LastUpdatedBy"))
                {
                    query = direction == "asc" ? query.OrderBy(x => x.AppLastUpdateUserid) : query.OrderByDescending(x => x.AppLastUpdateUserid);
                }
                else if (filter.Sort[0].StartsWith("LastUpdatedDate"))
                {
                    query = direction == "asc" ? query.OrderBy(x => x.AppLastUpdateTimestamp) : query.OrderByDescending(x => x.AppLastUpdateTimestamp);
                }
                else
                {
                    query = query.OrderByProperty(filter.Sort);
                }
            }
            else
            {
                query = query.OrderBy(l => l.Code);
            }

            var skip = (filter.Page - 1) * filter.Quantity;

            var items = await query
                .Include(x => x.ProjectStatusTypeCodeNavigation)
                .Include(x => x.RegionCodeNavigation)
                .Skip(skip)
                .Take(filter.Quantity)
                .ToListAsync();

            return new Paged<PimsProject>(items, filter.Page, filter.Quantity, query.Count());
        }
    }
}
