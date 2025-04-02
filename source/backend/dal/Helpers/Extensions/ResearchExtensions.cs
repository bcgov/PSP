using System.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
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
        public static IQueryable<PimsResearchFile> GenerateResearchQuery(this PimsContext context, Entity.Models.ResearchFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            var predicate = PredicateBuilder.New<PimsResearchFile>(acq => true);

            if (!string.IsNullOrWhiteSpace(filter.Pid))
            {
                var pidValue = filter.Pid.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(r => r.PimsPropertyResearchFiles.Any(pr => pr != null && EF.Functions.Like(pr.Property.Pid.ToString(), $"%{pidValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Pin))
            {
                var pinValue = filter.Pin.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(acq => acq.PimsPropertyResearchFiles.Any(pr => pr != null && EF.Functions.Like(pr.Property.Pin.ToString(), $"%{pinValue}%")));
            }

            if (filter.RegionCode > 0)
            {
                predicate.And(r => r.PimsPropertyResearchFiles.Any(pr => pr.Property != null && pr.Property.RegionCode == filter.RegionCode));
            }

            if (!string.IsNullOrWhiteSpace(filter.ResearchFileStatusTypeCode))
            {
                predicate.And(r => r.ResearchFileStatusTypeCode == filter.ResearchFileStatusTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.RFileNumber))
            {
                predicate.And(r => EF.Functions.Like(r.RfileNumber, $"%{filter.RFileNumber}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                predicate.And(r => EF.Functions.Like(r.Name, $"%{filter.Name}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.RoadOrAlias))
            {
                predicate.And(r => EF.Functions.Like(r.RoadAlias, $"%{filter.RoadOrAlias}%") || EF.Functions.Like(r.RoadName, $"%{filter.RoadOrAlias}%"));
            }

            if (filter.CreatedOnStartDate.HasValue)
            {
                predicate.And(l => (l.AppCreateTimestamp.Date >= filter.CreatedOnStartDate.Value.Date));
            }

            if (filter.CreatedOnEndDate.HasValue)
            {
                predicate.And(l => (l.AppCreateTimestamp.Date <= filter.CreatedOnEndDate.Value.Date));
            }

            if (filter.UpdatedOnStartDate.HasValue)
            {
                predicate.And(l => (l.AppLastUpdateTimestamp.Date >= filter.UpdatedOnStartDate.Value.Date));
            }

            if (filter.UpdatedOnEndDate.HasValue)
            {
                predicate.And(l => (l.AppLastUpdateTimestamp.Date <= filter.UpdatedOnEndDate.Value.Date));
            }

            if (!string.IsNullOrWhiteSpace(filter.AppCreateUserid))
            {
                predicate.And(r => EF.Functions.Like(r.AppCreateUserid, $"%{filter.AppCreateUserid}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.AppLastUpdateUserid))
            {
                predicate.And(r => EF.Functions.Like(r.AppLastUpdateUserid, $"%{filter.AppLastUpdateUserid}%"));
            }

            var query = context.PimsResearchFiles.AsNoTracking()
                .Include(r => r.ResearchFileStatusTypeCodeNavigation)
                .Include(r => r.PimsPropertyResearchFiles)
                    .ThenInclude(p => p.Property)
                    .ThenInclude(p => p.RegionCodeNavigation)
                .Where(predicate);

            if (filter.Sort?.Any() == true)
            {
                // If its sorting by rFileNumber, change it to sort by id given that the R-File is the id with a "R-" prefix .
                int sortRFileNumberIndex = filter.Sort.ToList().FindIndex(x => x.Contains("RfileNumber"));
                if (sortRFileNumberIndex >= 0)
                {
                    filter.Sort[sortRFileNumberIndex] = filter.Sort[sortRFileNumberIndex].Replace("RfileNumber", "ResearchFileId");
                }
                query = query.OrderByProperty(true, filter.Sort);
            }
            else
            {
                query = query.OrderBy(l => l.ResearchFileId);
            }

            return query;
        }
    }
}
