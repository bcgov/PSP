using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with projects within the datasource.
    /// </summary>
    public class ProjectRepository : BaseRepository<PimsProject>, IProjectRepository
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of a ProjectRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ProjectRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ProjectRepository> logger, IMapper mapper)
            : base(dbContext, user, logger)
        {
            _mapper = mapper;
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
                .Where(p => EF.Functions.Like(p.Description, $"%{filter}%") || EF.Functions.Like(p.Code, $"%{filter}%") || EF.Functions.Like(p.Code + " " + p.Description, $"%{filter}%"))
                .Where(p => regions.Contains(p.RegionCode))
                .OrderBy(a => a.Code)
                .Take(maxResults)
                .ToArray();
        }

        /// <summary>
        /// Returns a Paged Result of Projects based on ProjectFilter params.
        /// </summary>
        /// <returns></returns>
        public Task<Paged<PimsProject>> GetPageAsync(ProjectFilter filter, IEnumerable<short> userRegions)
        {
            User.ThrowIfNotAuthorized(Permissions.ProjectView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            return GetPage(filter, userRegions);
        }

        /// <summary>
        /// Get by ID - Search Projects by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsProject TryGet(long id)
        {
            User.ThrowIfNotAuthorized(Permissions.ProjectView);

            return Context.PimsProjects
                    .AsNoTracking()
                    .Include(x => x.PimsProjectProducts)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.PimsProjectPeople)
                        .ThenInclude(x => x.Person)
                    .Include(x => x.ProjectStatusTypeCodeNavigation)
                    .Include(x => x.RegionCodeNavigation)
                    .Include(x => x.CostTypeCode)
                    .Include(x => x.BusinessFunctionCode)
                    .Include(x => x.WorkActivityCode)
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Get by ID - Search Projects by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<PimsProject> GetAllByName(string name)
        {
            User.ThrowIfNotAuthorized(Permissions.ProjectView);

            return Context.PimsProjects
                    .AsNoTracking()
                    .Include(x => x.PimsProjectProducts)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.ProjectStatusTypeCodeNavigation)
                    .Include(x => x.RegionCodeNavigation)
                    .Include(x => x.CostTypeCode)
                    .Include(x => x.BusinessFunctionCode)
                    .Include(x => x.WorkActivityCode)
                    .Where(x => x.Description == name).ToArray();
        }

        /// <summary>
        /// Add Project to Context.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public PimsProject Add(PimsProject project)
        {
            foreach (var projectProduct in project.PimsProjectProducts)
            {
                if (projectProduct.ProductId != 0)
                {
                    this.Context.Entry(projectProduct.Product).State = EntityState.Modified;
                }
                else
                {
                    this.Context.Entry(projectProduct.Product).State = EntityState.Added;
                }
            }

            Context.PimsProjects.Add(project);

            return project;
        }

        /// <summary>
        /// Update Project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public PimsProject Update(PimsProject project)
        {
            using var queryScope = Logger.QueryScope();
            project.ThrowIfNull(nameof(project));

            var existingProject = Context.PimsProjects
                .FirstOrDefault(x => x.Id == project.Id) ?? throw new KeyNotFoundException();

            Func<PimsContext, PimsProjectProduct, bool> canDeleteGrandchild = (context, pa) => !context.PimsProducts.Any(o => o.Id == pa.ProductId);

            Context.UpdateChild<PimsProject, long, PimsProjectPerson, long>(p => p.PimsProjectPeople, project.Id, project.PimsProjectPeople.ToArray());
            Context.UpdateGrandchild<PimsProject, long, PimsProjectProduct>(p => p.PimsProjectProducts, pp => pp.Product, project.Id, project.PimsProjectProducts.ToArray(), canDeleteGrandchild);

            Context.Entry(existingProject).CurrentValues.SetValues(project);

            return project;
        }

        public PimsProject GetProjectAtTime(long projectId, DateTime time)
        {
            var projectHist = Context
                .PimsProjectHists.AsNoTracking()
                .Where(pacr => pacr.Id == projectId)
                .Where(pacr => pacr.EffectiveDateHist <= time
                    && (pacr.EndDateHist == null || pacr.EndDateHist > time))
                .GroupBy(pacr => pacr.Id)
                .Select(gpacr => gpacr.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                .FirstOrDefault();

            var workActivityHist = Context
                .PimsWorkActivityCodeHists.AsNoTracking()
                .Where(pacr => pacr.Id == projectHist.WorkActivityCodeId)
                .Where(pacr => pacr.EffectiveDateHist <= time
                    && (pacr.EndDateHist == null || pacr.EndDateHist > time))
                .GroupBy(pacr => pacr.Id)
                .Select(gpacr => gpacr.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                .FirstOrDefault();

            var costTypeHist = Context
                .PimsCostTypeCodeHists.AsNoTracking()
                .Where(pacr => pacr.Id == projectHist.CostTypeCodeId)
                .Where(pacr => pacr.EffectiveDateHist <= time
                    && (pacr.EndDateHist == null || pacr.EndDateHist > time))
                .GroupBy(pacr => pacr.Id)
                .Select(gpacr => gpacr.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                .FirstOrDefault();

            var businessFunctionHist = Context
                .PimsBusinessFunctionCodeHists.AsNoTracking()
                .Where(pacr => pacr.Id == projectHist.BusinessFunctionCodeId)
                .Where(pacr => pacr.EffectiveDateHist <= time
                    && (pacr.EndDateHist == null || pacr.EndDateHist > time))
                .GroupBy(pacr => pacr.Id)
                .Select(gpacr => gpacr.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                .FirstOrDefault();

            var project = _mapper.Map<PimsProject>(projectHist);
            project.WorkActivityCode = _mapper.Map<PimsWorkActivityCode>(workActivityHist);
            project.CostTypeCode = _mapper.Map<PimsCostTypeCode>(costTypeHist);
            project.BusinessFunctionCode = _mapper.Map<PimsBusinessFunctionCode>(businessFunctionHist);
            return project;
        }

        private async Task<Paged<PimsProject>> GetPage(ProjectFilter filter, IEnumerable<short> userRegions)
        {
            var query = Context.PimsProjects.AsNoTracking()
                .Where(p => userRegions.Contains(p.RegionCode));

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
                query = query.Where(x => x.RegionCode == short.Parse(filter.ProjectRegionCode));
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
                    query = query.OrderByProperty(true, filter.Sort);
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
