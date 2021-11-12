using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using System;
using System.Linq;
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
        private static IQueryable<Entity.Contact> GenerateCommonContactQuery(this IQueryable<Entity.Contact> query, Entity.Models.ContactFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            if (!String.IsNullOrWhiteSpace(filter.Summary))
            {
                query = query.Where(c => EF.Functions.Like(c.Summary, $"%{filter.Summary}%"));
            }

            if (!String.IsNullOrWhiteSpace(filter.Municipality))
            {
                query = query.Where(c => EF.Functions.Like(c.Municipality, $"%{filter.Municipality}%"));
            }

            if (filter.SearchBy == "persons")
            {
                query = query.Where(c => c.Person != null);
            } else if (filter.SearchBy == "organizations")
            {
                query = query.Where(c => c.Organization != null);
            }

            if (filter.ActiveContactsOnly)
            {
                query = query.Where(c => !c.IsDisabled);
            }

            if (filter.Sort?.Any() == true)
                query = query.OrderByProperty(filter.Sort);
            else
                query = query.OrderBy(c => c.Summary);

            return query;
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<Entity.Contact> GenerateContactQuery(this PimsContext context, Entity.Models.ContactFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));

            IQueryable<Contact> query = context.Contacts.AsNoTracking()
                .Include(c => c.Person)
                .ThenInclude(p => p.ContactMethods);

            query = query.GenerateCommonContactQuery(filter);

            return query;
        }
    }
}
