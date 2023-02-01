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
        /// <param name="maxResult"></param>
        /// <returns></returns>
        public IList<PimsProject> SearchProjects(string filter, int maxResult)
        {
            return this.Context.PimsProjects.AsNoTracking()
                .Where(o => EF.Functions.Like(o.Description, $"%{filter}%"))
                .OrderBy(a => a.Code)
                .Take(maxResult)
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
        public async Task<PimsProject> Get(long id)
        {
            User.ThrowIfNotAuthorized(Permissions.ProjectView);

            return await Context.PimsProjects
                    .AsNoTracking()
                    .Include(x => x.ProjectStatusTypeCodeNavigation)
                    .Include(x => x.RegionCodeNavigation)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Add Project to Context.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<PimsProject> Add(PimsProject project)
        {
            User.ThrowIfNotAuthorized(Permissions.ProjectAdd);

            await Context.PimsProjects.AddAsync(project);
            return project;
        }

        public PimsProject Update(PimsProject project)
        {
            using var queryScope = Logger.QueryScope();
            project.ThrowIfNull(nameof(project));

            var existingAcqFile = Context.PimsProjects
                .FirstOrDefault(x => x.Id == project.Id) ?? throw new KeyNotFoundException();

            Context.Entry(existingAcqFile).CurrentValues.SetValues(project);

            return project;
        }

        /// <summary>
        /// Retrieves the version of project
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The project row version.</returns>
        public long GetRowVersion(long id)
        {
            using var log = Logger.QueryScope();

            return this.Context.PimsProjects.AsNoTracking()
                .Where(p => p.Id == id)?
                .Select(p => p.ConcurrencyControlNumber)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
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
                query = query.OrderByProperty(filter.Sort);
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
