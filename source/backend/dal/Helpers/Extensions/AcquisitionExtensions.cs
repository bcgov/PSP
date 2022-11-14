namespace Pims.Dal.Helpers.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Pims.Core.Extensions;
    using Entity = Pims.Dal.Entities;

    /// <summary>
    /// AcquisitionExtensions static class, provides extension methods for acquisition files.
    /// </summary>
    public static class AcquisitionExtensions
    {
        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filter"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
        public static IQueryable<Entities.PimsAcquisitionFile> GenerateAcquisitionQuery(this PimsContext context, Entity.Models.AcquisitionFilter filter, HashSet<short> regions)
        {
            filter.ThrowIfNull(nameof(filter));

            var query = context.PimsAcquisitionFiles.AsNoTracking();

            query = query.GenerateCommonAcquisitionQuery(filter, regions);

            return query;
        }

        /// <summary>
        /// Generate an SQL statement for the specified 'research file' and 'filter'.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
        private static IQueryable<Entities.PimsAcquisitionFile> GenerateCommonAcquisitionQuery(this IQueryable<Entities.PimsAcquisitionFile> query, Entity.Models.AcquisitionFilter filter, HashSet<short> regions)
        {
            filter.ThrowIfNull(nameof(filter));

            if (!string.IsNullOrWhiteSpace(filter.Pid))
            {
                var pidValue = filter.Pid.Replace("-", string.Empty).Trim().TrimStart('0');
                query = query.Where(l => l.PimsPropertyAcquisitionFiles.Any(pa => pa != null && EF.Functions.Like(pa.Property.Pid.ToString(), $"%{pidValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Pin))
            {
                var pinValue = filter.Pin.Replace("-", string.Empty).Trim().TrimStart('0');
                query = query.Where(l => l.PimsPropertyAcquisitionFiles.Any(pa => pa != null && EF.Functions.Like(pa.Property.Pin.ToString(), $"%{pinValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                query = query.Where(l => l.PimsPropertyAcquisitionFiles.Any(pl => pl != null &&
                    (EF.Functions.Like(pl.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.MunicipalityName, $"%{filter.Address}%"))));
            }

            if (!string.IsNullOrWhiteSpace(filter.AcquisitionFileStatusTypeCode))
            {
                query = query.Where(r => r.AcquisitionFileStatusTypeCode == filter.AcquisitionFileStatusTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.AcquisitionFileNameOrNumber))
            {
                query = query.Where(r => EF.Functions.Like(r.FileName, $"%{filter.AcquisitionFileNameOrNumber}%") || EF.Functions.Like(r.FileNumber, $"%{filter.AcquisitionFileNameOrNumber}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectNameOrNumber))
            {
                query = query.Where(r => EF.Functions.Like(r.MinistryProjectName, $"%{filter.ProjectNameOrNumber}%") || EF.Functions.Like(r.MinistryProjectNumber, $"%{filter.ProjectNameOrNumber}%"));
            }

            // Business Requirement: limit search results to user's assigned region(s)
            query = query.Where(r => regions.Contains(r.RegionCode));

            if (filter.Sort?.Any() == true)
            {
                query = query.OrderByProperty(filter.Sort);
            }
            else
            {
                query = query.OrderBy(l => l.AcquisitionFileId);
            }

            return query
                .Include(r => r.AcquisitionFileStatusTypeCodeNavigation)
                .Include(r => r.RegionCodeNavigation)
                .Include(r => r.PimsPropertyAcquisitionFiles)
                    .ThenInclude(pa => pa.Property)
                    .ThenInclude(p => p.Address);
        }
    }
}
