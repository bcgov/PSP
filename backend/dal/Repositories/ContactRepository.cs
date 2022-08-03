using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        /// <param name="logger"></param>
        public ContactRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ContactRepository> logger)
            : base(dbContext, user, logger)
        {
        }
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

            IEnumerable<PimsContactMgrVw> contacts = GetFilteredContacts(filter, out _);

            return contacts;
        }

        /// <summary>
        /// Get a contact with the given id.
        /// </summary>
        /// <param name="id">The id of the contact to retrieve.</param>
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
            IEnumerable<PimsContactMgrVw> results = GetFilteredContacts(filter, out int totalItems);

            return new Paged<PimsContactMgrVw>(results, filter.Page, filter.Quantity, totalItems);
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

        /// <summary>
        /// Generate an SQL statement for the specified 'filter'.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="totalItems">To hold and return total actual items.</param>
        /// <returns></returns>
        private IEnumerable<PimsContactMgrVw> GetFilteredContacts(ContactFilter filter, out int totalItems)
        {
            filter.ThrowIfNull(nameof(filter));

            var query = from c in this.Context.PimsContactMgrVws
                        join o in this.Context.PimsOrganizations
                            on c.Organization equals o into contactOrganization
                        from co in contactOrganization.DefaultIfEmpty()
                        join po in this.Context.PimsPersonOrganizations
                            on co equals po.Organization into contactPersonOrganization
                        from copo in contactPersonOrganization.DefaultIfEmpty()
                        join p in this.Context.PimsPeople
                            on new { person = copo != null ? copo.Person : null } equals new { person = p } into contactPersonOrganizationPerson
                        from copop in contactPersonOrganizationPerson.DefaultIfEmpty()
                        select new { Contact = c, Organization = co, PersonOrganization = copo, Person = copop };

            if (!string.IsNullOrWhiteSpace(filter.Municipality))
            {
                query = query.Where(c => c.Contact.MunicipalityName != null && c.Contact.MunicipalityName.Contains(filter.Municipality));
            }

            var summary = filter.Summary?.Trim() ?? string.Empty;

            if (filter.SearchBy == "persons")
            {
                query = query.Where(c => c.Contact.PersonId != null && c.Contact.Id.StartsWith("P"));
                string[] nameParts = summary.Split(' ');
                if (!string.IsNullOrWhiteSpace(summary))
                {
                    foreach (string namePart in nameParts)
                    {
                        query = query.Where(c => (c.Contact.FirstName != null && c.Contact.FirstName.Contains(namePart)) ||
                        (c.Contact.Surname != null && c.Contact.Surname.Contains(namePart)) ||
                        (c.Contact.MiddleNames != null && c.Contact.MiddleNames.Contains(namePart)));
                    }
                }
            }
            else if (filter.SearchBy == "organizations")
            {
                query = query.Where(c => c.Contact.OrganizationId != null && c.Contact.Id.StartsWith("O"));
                if (!string.IsNullOrWhiteSpace(summary))
                {
                    query = query.Where(c => c.Contact.Summary != null && c.Contact.Summary.Contains(summary));
                }
            }
            else
            {
                string[] nameParts = summary.Split(' ');
                if (!string.IsNullOrWhiteSpace(summary))
                {
                    foreach (string namePart in nameParts)
                    {
                        query = query.Where(c => (c.Contact.FirstName != null && c.Contact.FirstName.Contains(namePart)) ||
                        (c.Contact.Surname != null && c.Contact.Surname.Contains(namePart)) ||
                        (c.Contact.MiddleNames != null && c.Contact.MiddleNames.Contains(namePart)) ||
                        (c.Contact.OrganizationName != null && c.Contact.OrganizationName.Contains(summary)));
                    }
                }
            }

            if (filter.ActiveContactsOnly)
            {
                query = query.Where(c => !c.Contact.IsDisabled);
            }

            if (filter.Sort?.Any() == true)
            {
                var field = filter.Sort.FirstOrDefault()?.Split(" ")?.FirstOrDefault();
                var direction = filter.Sort.FirstOrDefault()?.Split(" ")?.LastOrDefault();
                if (field == "Surname")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.Contact.Surname) : query.OrderByDescending(c => c.Contact.Surname);
                }
                else if (field == "FirstName")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.Contact.FirstName) : query.OrderByDescending(c => c.Contact.FirstName);
                }
                else if (field == "OrganizationName")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.Contact.OrganizationName) : query.OrderByDescending(c => c.Contact.OrganizationName);
                }
                else if (field == "MunicipalityName")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.Contact.MunicipalityName) : query.OrderByDescending(c => c.Contact.MunicipalityName);
                }
                else
                {
                    query = direction == "asc" ? query.OrderBy(c => c.Contact.Summary) : query.OrderByDescending(c => c.Contact.Summary);
                }
            }
            else
            {
                query = query.OrderBy(c => c.Contact.Summary);
            }

            var skip = (filter.Page - 1) * filter.Quantity;

            totalItems = query.Count();

            var contactsWithOrganizations = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            return contactsWithOrganizations.GroupBy(copop => copop.Contact.Id).Select(contactGroup =>
            {
                PimsContactMgrVw contact = contactGroup.FirstOrDefault().Contact;
                PimsOrganization organization = contactGroup.FirstOrDefault().Organization;
                if (organization != null)
                {
                    IEnumerable<PimsPersonOrganization> pimsPersonOrganizations = contactsWithOrganizations
                        .Where(copop => copop.Contact.Id == contactGroup.Key && copop.PersonOrganization != null)
                        .Select(copop =>
                        {
                            copop.PersonOrganization.Person = copop.Person;
                            return copop.PersonOrganization;
                        });

                    organization.PimsPersonOrganizations = pimsPersonOrganizations.ToArray();
                    contact.Organization = organization;
                }
                return contact;
            });
        }
        #endregion
    }
}
