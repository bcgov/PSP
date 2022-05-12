using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ContactService class, provides a service layer to interact with contacts within the datasource.
    /// </summary>
    public class ContactRepository : BaseRepository, IContactRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a ContactService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ContactRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<ContactRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the total number of contacts in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.PimsContactMgrVws.Count();
        }

        /// <summary>
        /// Get an array of contacts within the specified filters.
        /// Note that the 'contactFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<PimsContactMgrVw> Get(ContactFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            IEnumerable<PimsContactMgrVw> contacts = this.Context.GenerateContactQuery(filter).ToArray();

            return contacts;
        }

        /// <summary>
        /// Get a contact with the given id.
        /// </summary>
        /// <param name="id">The id of the contact to retrieve</param>
        /// <returns></returns>
        public PimsContactMgrVw Get(string id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            var contact = Context.PimsContactMgrVws.Where(x => x.Id == id).FirstOrDefault();

            return contact;
        }

        /// <summary>
        /// Get a page with an array of contacts within the specified filters.
        /// Note that the 'contactFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsContactMgrVw> GetPage(ContactFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = this.Context.GenerateContactQuery(filter);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            return new Paged<PimsContactMgrVw>(items, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Find the entity for the specified 'keyValues'.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public PimsContactMgrVw Find(params object[] keyValues)
        {
            return this.Context.Find<PimsContactMgrVw>(keyValues);
        }
        #endregion
    }
}
