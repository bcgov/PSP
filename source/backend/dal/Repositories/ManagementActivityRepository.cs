using System;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    public class ManagementActivityRepository : BaseRepository<PimsPropertyActivity>, IManagementActivityRepository
    {
        public ManagementActivityRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ManagementActivityRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        /// <summary>
        /// Returns the total number of Management Actities in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Context.PimsPropertyActivities.Count();
        }

        public Paged<PimsPropertyActivity> GetPageDeep(ManagementActivityFilter filter)
        {
            using var scope = Logger.QueryScope();

            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var query = GetCommonManagementActivityQuery(filter);

            var skip = (filter.Page - 1) * filter.Quantity;
            var pageItems = query.Skip(skip).Take(filter.Quantity).ToList();

            return new Paged<PimsPropertyActivity>(pageItems, filter.Page, filter.Quantity, query.Count());
        }

        private IQueryable<PimsPropertyActivity> GetCommonManagementActivityQuery(ManagementActivityFilter filter)
        {
            var predicate = PredicateBuilder.New<PimsPropertyActivity>(act => true);

            if (!string.IsNullOrWhiteSpace(filter.Pid))
            {
                var pidValue = filter.Pid.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(x => x.PimsPropPropActivities.Any(pd => pd != null && EF.Functions.Like(pd.Property.Pid.ToString(), $"%{pidValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Pin))
            {
                var pinValue = filter.Pin.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(x => x.PimsPropPropActivities.Any(pd => pd != null && EF.Functions.Like(pd.Property.Pin.ToString(), $"%{pinValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                predicate = predicate.And(x => x.PimsPropPropActivities.Any(pd => pd != null &&
                    (EF.Functions.Like(pd.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.MunicipalityName, $"%{filter.Address}%"))));

                predicate = predicate.Or(x => x.ManagementFile.PimsManagementFileProperties.Any(pd => pd != null &&
                    (EF.Functions.Like(pd.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.MunicipalityName, $"%{filter.Address}%"))));
            }

            if (!string.IsNullOrWhiteSpace(filter.FileNameOrNumberOrReference))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.ManagementFile.FileName, $"%{filter.FileNameOrNumberOrReference}%")
                || EF.Functions.Like(x.ManagementFile.ManagementFileId.ToString(), $"%{filter.FileNameOrNumberOrReference}%")
                || EF.Functions.Like(x.ManagementFile.LegacyFileNum, $"%{filter.FileNameOrNumberOrReference}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.ActivityTypeCode))
            {
                predicate = predicate.And(x => x.PropMgmtActivityTypeCode == filter.ActivityTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ActivitySubTypeCode))
            {
                predicate = predicate.And(x => x.PropMgmtActivitySubtypeCode == filter.ActivitySubTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ActivityStatusCode))
            {
                predicate = predicate.And(x => x.PropMgmtActivityStatusTypeCode == filter.ActivityStatusCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectNameOrNumber))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.ManagementFile.Project.Code, $"%{filter.ProjectNameOrNumber}%") || EF.Functions.Like(x.ManagementFile.Project.Description, $"%{filter.ProjectNameOrNumber}%"));
            }

            var query = Context.PimsPropertyActivities.AsNoTracking()
                .Include(s => s.PropMgmtActivityStatusTypeCodeNavigation)
                .Include(t => t.PropMgmtActivityTypeCodeNavigation)
                .Include(st => st.PropMgmtActivitySubtypeCodeNavigation)
                .Include(pp => pp.PimsPropPropActivities)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(a => a.Address)
                .Include(f => f.ManagementFile)
                    .ThenInclude(pr => pr.PimsManagementFileProperties)
                        .ThenInclude(p => p.Property)
                            .ThenInclude(a => a.Address)

                .Where(predicate);

            if (filter.Sort?.Any() == true)
            {
                var field = filter.Sort.FirstOrDefault()?.Split(" ")?.FirstOrDefault();
                var direction = filter.Sort.FirstOrDefault()?.Split(" ")?.LastOrDefault();

                if (field == "ActivityStatus")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.PropMgmtActivityStatusTypeCodeNavigation.Description) : query.OrderByDescending(c => c.PropMgmtActivityStatusTypeCodeNavigation.Description);
                }
                else if (field == "ActivityType")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.PropMgmtActivityStatusTypeCodeNavigation.Description) : query.OrderByDescending(c => c.PropMgmtActivityStatusTypeCodeNavigation.Description);
                }
                else if (field == "ActivitySubType")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.PropMgmtActivitySubtypeCodeNavigation.Description) : query.OrderByDescending(c => c.PropMgmtActivitySubtypeCodeNavigation.Description);
                }
                else if (field == "FileName")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.ManagementFile.FileName) : query.OrderByDescending(c => c.ManagementFile.FileName);
                }
                else if (field == "LegacyFileNum")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.ManagementFile.LegacyFileNum) : query.OrderByDescending(c => c.ManagementFile.LegacyFileNum);
                }
            }
            else
            {
                query = query.OrderBy(x => x.PimsPropertyActivityId);
            }

            return query;
        }
    }
}
