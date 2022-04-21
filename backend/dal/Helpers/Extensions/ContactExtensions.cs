using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// ContactExtensions static class, provides extension methods for contacts.
    /// </summary>
    public static class ContactExtensions
    {
        /// <summary>
        /// Generate an SQL statement for the specified 'filter'.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static IQueryable<Entity.PimsContactMgrVw> GenerateCommonContactQuery(this IQueryable<Entity.PimsContactMgrVw> query, Entity.Models.ContactFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            if (!String.IsNullOrWhiteSpace(filter.Municipality))
            {
                query = query.Where(c => EF.Functions.Like(c.MunicipalityName, $"%{filter.Municipality}%"));
            }

            var summary = filter.Summary.Trim();

            if (filter.SearchBy == "persons")
            {
                query = query.Where(c => c.PersonId != null && c.Id.StartsWith("P"));
                string[] nameParts = filter.Summary.Split(' ');
                if (!String.IsNullOrWhiteSpace(summary))
                {
                    foreach (string namePart in nameParts)
                    {
                        query = query.Where(c => EF.Functions.Like(c.FirstName, $"%{namePart}%") || EF.Functions.Like(c.Surname, $"%{namePart}%") || EF.Functions.Like(c.MiddleNames, $"%{namePart}%"));
                    }
                }
            }
            else if (filter.SearchBy == "organizations")
            {
                query = query.Where(c => c.OrganizationId != null && c.Id.StartsWith("O"));
                if (!String.IsNullOrWhiteSpace(summary))
                {
                    query = query.Where(c => EF.Functions.Like(c.Summary, $"%{summary}%"));
                }
            }
            else
            {
                string[] nameParts = summary.Split(' ');
                if (!String.IsNullOrWhiteSpace(summary))
                {
                    foreach (string namePart in nameParts)
                    {
                        query = query.Where(c => EF.Functions.Like(c.FirstName, $"%{namePart}%") || EF.Functions.Like(c.Surname, $"%{namePart}%") || EF.Functions.Like(c.MiddleNames, $"%{namePart}%") || EF.Functions.Like(c.OrganizationName, $"%{summary}%"));
                    }
                }
            }

            if (filter.ActiveContactsOnly)
            {
                query = query.Where(c => !c.IsDisabled);
            }

            if (filter.Sort?.Any() == true)
            {
                query = query.OrderByProperty(filter.Sort);
            }
            else
            {
                query = query.OrderBy(c => c.Summary);
            }

            return query;
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entity.PimsContactMgrVw> GenerateContactQuery(this PimsContext context, Entity.Models.ContactFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            IQueryable<PimsContactMgrVw> query = context.PimsContactMgrVws.AsNoTracking();

            query = query.GenerateCommonContactQuery(filter);

            return query;
        }
    }
}
