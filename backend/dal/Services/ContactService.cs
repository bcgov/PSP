using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// ContactService class, provides a service layer to interact with contacts within the datasource.
    /// </summary>
    public class ContactService : BaseService, IContactService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a ContactService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ContactService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<ContactService> logger) : base(dbContext, user, service, logger) { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the total number of contacts in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.Persons.Count() + this.Context.Organizations.Count();
        }

        /// <summary>
        /// Get an array of contacts within the specified filters.
        /// Note that the 'contactFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Contact> Get(ContactFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid()) throw new ArgumentException("Argument must have a valid filter", nameof(filter));

            IEnumerable<Contact> contacts = this.Context.GenerateContactQuery(filter).ToArray();

            return contacts;
        }

        /// <summary>
        /// Get a page with an array of contacts within the specified filters.
        /// Note that the 'contactFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<Contact> GetPage(ContactFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid()) throw new ArgumentException("Argument must have a valid filter", nameof(filter));

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = this.Context.GenerateContactQuery(filter);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            return new Paged<Contact>(items, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Find the entity for the specified 'keyValues'.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public Contact Find(params object[] keyValues)
        {
            return this.Context.Find<Contact>(keyValues);
        }
        #endregion
    }
}
