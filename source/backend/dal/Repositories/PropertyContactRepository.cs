using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// PropertyContactRepository class, provides a service layer to interact with property contacts within the datasource.
    /// </summary>
    public class PropertyContactRepository : BaseRepository<PimsPropertyContact>, IPropertyContactRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyContactRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyContactRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyContactRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the property contacts for the specified property with 'propertyId' value.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public IList<PimsPropertyContact> GetContactsByProperty(long propertyId)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var contacts = this.Context.PimsPropertyContacts
                .Include(p => p.Person)
                .Include(p => p.Organization)
                .Include(p => p.PrimaryContact)
                .Where(p => p.PropertyId == propertyId)
                .AsNoTracking()
                .ToList() ?? throw new KeyNotFoundException();
            return contacts;
        }

        /// <summary>
        /// Get the property contact for the specified contact with 'contactId' value.
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public PimsPropertyContact GetContact(long contactId)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var contact = this.Context.PimsPropertyContacts
                .Include(p => p.Person)
                .Include(p => p.Organization)
                .Include(p => p.PrimaryContact)
                .AsNoTracking()
                .FirstOrDefault(p => p.PropertyContactId == contactId) ?? throw new KeyNotFoundException();
            return contact;
        }

        /// <summary>
        /// Creates the passed property contact in the database.
        /// </summary>
        /// <param name="propertyContact"></param>
        /// <returns></returns>
        public PimsPropertyContact Create(PimsPropertyContact propertyContact)
        {
            propertyContact.ThrowIfNull(nameof(propertyContact));

            // update main entity - PimsPropertyContact
            var entityEntry = Context.PimsPropertyContacts.Add(propertyContact);

            return entityEntry.Entity;
        }

        /// <summary>
        /// Update the passed property contact in the database.
        /// </summary>
        /// <param name="propertyContact"></param>
        /// <returns></returns>
        public PimsPropertyContact Update(PimsPropertyContact propertyContact)
        {
            propertyContact.ThrowIfNull(nameof(propertyContact));

            var existingPropertyContact = this.Context.PimsPropertyContacts
                .FirstOrDefault(p => p.PropertyContactId == propertyContact.PropertyContactId) ?? throw new KeyNotFoundException();

            // The contact cannot be updated by bussiness requirements.
            if (existingPropertyContact.PersonId != propertyContact.PersonId || existingPropertyContact.OrganizationId != propertyContact.OrganizationId)
            {
                throw new InvalidOperationException("Property contact's contact cannot be updated");
            }

            // update main entity - PimsPropertyContact
            Context.Entry(existingPropertyContact).CurrentValues.SetValues(propertyContact);

            return existingPropertyContact;
        }

        /// <summary>
        /// Delete a property contact. Note that this method will fail unless all dependencies are removed first.
        /// </summary>
        /// <param name="propertyContactId"></param>
        public void Delete(long propertyContactId)
        {
            this.Context.Entry(new PimsPropertyContact() { PropertyContactId = propertyContactId }).State = EntityState.Deleted;
        }

        #endregion
    }
}
