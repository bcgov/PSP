namespace Pims.Dal.Helpers.Extensions
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Pims.Core.Extensions;
    using Entity = Pims.Dal.Entities;

    /// <summary>
    /// ResearchExtensions static class, provides extension methods for research.
    /// </summary>
    public static class ResearchExtensions
    {
        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entities.PimsResearchFile> GenerateResearchQuery(this PimsContext context, Entity.Models.ResearchFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            var query = context.PimsResearchFiles.AsNoTracking();

            query = query.GenerateCommonResearchQuery(filter);

            return query;
        }

        /// <summary>
        /// Generate an SQL statement for the specified 'research file' and 'filter'.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static IQueryable<Entities.PimsResearchFile> GenerateCommonResearchQuery(this IQueryable<Entities.PimsResearchFile> query, Entity.Models.ResearchFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            if (filter.RegionCode > 0)
            {
                query = query.Where(r => r.PimsPropertyResearchFiles.Any(pr => pr.Property != null && pr.Property.RegionCode == filter.RegionCode));
            }

            if (!string.IsNullOrWhiteSpace(filter.ResearchFileStatusTypeCode))
            {
                query = query.Where(r => r.ResearchFileStatusTypeCode == filter.ResearchFileStatusTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.RFileNumber))
            {
                query = query.Where(r => EF.Functions.Like(r.RfileNumber, $"%{filter.RFileNumber}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(r => EF.Functions.Like(r.Name, $"%{filter.Name}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.RoadOrAlias))
            {
                query = query.Where(r => EF.Functions.Like(r.RoadAlias, $"%{filter.RoadOrAlias}%") || EF.Functions.Like(r.RoadName, $"%{filter.RoadOrAlias}%"));
            }

            if (filter.CreatedOnStartDate.HasValue)
            {
                query = query.Where(l => (l.AppCreateTimestamp.Date >= filter.CreatedOnStartDate.Value.Date));
            }

            if (filter.CreatedOnEndDate.HasValue)
            {
                query = query.Where(l => (l.AppCreateTimestamp.Date <= filter.CreatedOnEndDate.Value.Date));
            }

            if (filter.UpdatedOnStartDate.HasValue)
            {
                query = query.Where(l => (l.AppLastUpdateTimestamp.Date >= filter.UpdatedOnStartDate.Value.Date));
            }

            if (filter.UpdatedOnEndDate.HasValue)
            {
                query = query.Where(l => (l.AppLastUpdateTimestamp.Date <= filter.UpdatedOnEndDate.Value.Date));
            }

            if (!string.IsNullOrWhiteSpace(filter.AppCreateUserid))
            {
                query = query.Where(r => EF.Functions.Like(r.AppCreateUserid, $"%{filter.AppCreateUserid}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.AppLastUpdateUserid))
            {
                query = query.Where(r => EF.Functions.Like(r.AppLastUpdateUserid, $"%{filter.AppLastUpdateUserid}%"));
            }

            if (filter.Sort?.Any() == true)
            {
                // If its sorting by rFileNumber, change it to sort by id given that the R-File is the id with a "R-" prefix .
                int sortRFileNumberIndex = filter.Sort.ToList().FindIndex(x => x.Contains("RfileNumber"));
                if (sortRFileNumberIndex >= 0)
                {
                    filter.Sort[sortRFileNumberIndex] = filter.Sort[sortRFileNumberIndex].Replace("RfileNumber", "ResearchFileId");
                }
                query = query.OrderByProperty(filter.Sort);
            }
            else
            {
                query = query.OrderBy(l => l.ResearchFileId);
            }

            return query.Include(r => r.ResearchFileStatusTypeCodeNavigation)
                .Include(r => r.PimsPropertyResearchFiles)
                .ThenInclude(p => p.Property)
                .ThenInclude(p => p.RegionCodeNavigation);
        }
    }
}
