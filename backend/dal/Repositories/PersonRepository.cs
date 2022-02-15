using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a service layer to administrate persons within the datasource.
    /// </summary>
    public class PersonRepository : BaseRepository<PimsPerson>, IPersonRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a PersonService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public PersonRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<PersonRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get a page of persons from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IEnumerable<PimsPerson> GetAll()
        {
            return this.Context.PimsPeople.AsNoTracking().ToArray();
        }

        public long GetRowVersion(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            return this.Context.PimsPeople.AsNoTracking().Where(p => p.PersonId == id)?.Select(p => p.ConcurrencyControlNumber)?.FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the person for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">Person does not exists for the specified 'id'.</exception>
        /// <returns></returns>
        public PimsPerson Get(long id)
        {
            return this.Context.PimsPeople
                .Include(p => p.PimsPersonAddresses)
                    .ThenInclude(pa => pa.Address)
                    .ThenInclude(a => a.Country)
                .Include(p => p.PimsPersonAddresses)
                    .ThenInclude(pa => pa.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(p => p.PimsPersonAddresses)
                    .ThenInclude(pa => pa.AddressUsageTypeCodeNavigation)
                .Include(p => p.PimsPersonOrganizations)
                    .ThenInclude(o => o.Organization)
                .Include(p => p.PimsContactMethods)
                    .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)
                .Where(p => p.PersonId == id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the specified person contact to the datasource.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="userOverride"></param>
        /// <returns></returns>
        public PimsPerson Add(PimsPerson person, bool userOverride)
        {
            person.ThrowIfNull(nameof(person));
            if (!userOverride)
            {

                List<PimsPerson> existingPersons = this.Context.PimsPeople.Where(
                    p => EF.Functions.Collate(p.FirstName, SqlCollation.LATIN_GENERAL_CASE_INSENSITIVE) == person.FirstName &&
                        EF.Functions.Collate(p.Surname, SqlCollation.LATIN_GENERAL_CASE_INSENSITIVE) == person.Surname
                ).Include(p => p.PimsContactMethods).ToList();

                var isDuplicate = existingPersons.Any(
                                p => p.PimsContactMethods.Any(
                                    ec => person.PimsContactMethods.Any(
                                        pc => pc.ContactMethodValue.ToLower() == ec.ContactMethodValue.ToLower() &&
                                        pc.ContactMethodTypeCode == ec.ContactMethodTypeCode)));

                if (isDuplicate)
                {
                    throw new DuplicateEntityException("Duplicate person found");
                }
            }

            this.Context.PimsPeople.Add(person);
            return person;
        }

        /// <summary>
        /// Update the passed person/contact in the database assuming the user has the require claims.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public PimsPerson Update(PimsPerson person)
        {
            person.ThrowIfNull(nameof(person));
            this.User.ThrowIfNotAuthorized(Permissions.ContactEdit);

            var personId = person.Id;
            var existingPerson = this.Context.PimsPeople.Where(p => p.PersonId == personId).FirstOrDefault()
                 ?? throw new KeyNotFoundException();

            // update main entity - PimsPerson
            Context.Entry(existingPerson).CurrentValues.SetValues(person);

            // update direct relationships - contact_methods, organizations
            this.Context.UpdateChild<PimsPerson, long, PimsContactMethod>(p => p.PimsContactMethods, personId, person.PimsContactMethods.ToArray());
            this.Context.UpdateChild<PimsPerson, long, PimsPersonOrganization>(p => p.PimsPersonOrganizations, personId, person.PimsPersonOrganizations.ToArray());

            // update addresses via UpdateGrandchild method
            this.Context.UpdateGrandchild<PimsPerson, long, PimsPersonAddress>(p => p.PimsPersonAddresses, pa => pa.Address, personId, person.PimsPersonAddresses.ToArray());

            return existingPerson;
        }

        public bool IsUsingOrganizationAddress(PimsPerson person, long organizationId)
        {
            person.ThrowIfNull(nameof(person));
            this.User.ThrowIfNotAuthorized(Permissions.ContactEdit);

            // get existing person along with all its organizations and addresses
            var personId = person.Id;
            var existingPerson = this.Context.PimsPeople.Where(p => p.PersonId == personId)
                .Include(p => p.PimsPersonAddresses)
                .Include(p => p.PimsPersonOrganizations)
                    .ThenInclude(o => o.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                .FirstOrDefault() ?? throw new KeyNotFoundException();

            // The database supports many organizations for a person but the app currently supports only one linked organization per person.
            var linkedOrganization = existingPerson
                .PimsPersonOrganizations?
                .FirstOrDefault(p => p != null && p.Organization != null && p.OrganizationId == organizationId)?.Organization;

            if (linkedOrganization == null)
            {
                return false; // person not linked to supplied org
            }

            var orgAddress = linkedOrganization.PimsOrganizationAddresses.Where(a => a.AddressUsageTypeCode == AddressUsageTypes.Mailing).FirstOrDefault();
            var personAddress = existingPerson.PimsPersonAddresses.Where(a => a.AddressUsageTypeCode == AddressUsageTypes.Mailing).FirstOrDefault();

            return orgAddress != null && personAddress != null && personAddress.AddressId == orgAddress.AddressId;
        }

        public void LinkAddressToOrganization(PimsPerson person, long organizationId)
        {
            person.ThrowIfNull(nameof(person));
            this.User.ThrowIfNotAuthorized(Permissions.ContactEdit);

            var personId = person.Id;
            var existingPerson = this.Context.PimsPeople.Where(p => p.PersonId == personId)
                .Include(p => p.PimsPersonAddresses)
                .FirstOrDefault() ?? throw new KeyNotFoundException();

            var organizationAddress = this.Context.PimsOrganizationAddresses.AsNoTracking()
                .Where(o => o.OrganizationId == organizationId && o.AddressUsageTypeCode == AddressUsageTypes.Mailing)
                .FirstOrDefault() ?? throw new KeyNotFoundException("Cannot link the organization address to this person. No mailing address found.");

            // delete any pre-existing mailing addresses on the person
            var existingMailing = existingPerson.PimsPersonAddresses.Where(a => a.AddressUsageTypeCode == AddressUsageTypes.Mailing).FirstOrDefault();
            if (existingMailing != null)
            {
                existingPerson.PimsPersonAddresses.Remove(existingMailing);
                this.Context.Remove(existingMailing);
            }

            // add new mailing address pointing to the organization address
            var newMailing = new PimsPersonAddress();
            newMailing.AddressUsageTypeCode = AddressUsageTypes.Mailing;
            newMailing.IsDisabled = false;
            newMailing.PersonId = existingPerson.PersonId;
            newMailing.AddressId = organizationAddress.AddressId;

            existingPerson.PimsPersonAddresses.Add(newMailing);
            this.Context.Add(newMailing);
        }

        public void UnlinkAddressFromOrganization(PimsPerson person, long organizationId)
        {
            person.ThrowIfNull(nameof(person));
            this.User.ThrowIfNotAuthorized(Permissions.ContactEdit);

            var personId = person.Id;
            var existingPerson = this.Context.PimsPeople.Where(p => p.PersonId == personId)
                .Include(p => p.PimsPersonAddresses)
                .FirstOrDefault() ?? throw new KeyNotFoundException();

            var organizationAddress = this.Context.PimsOrganizationAddresses.AsNoTracking()
                .Where(o => o.OrganizationId == organizationId && o.AddressUsageTypeCode == AddressUsageTypes.Mailing)
                .FirstOrDefault() ?? throw new KeyNotFoundException("Cannot unlink the organization address from this person. No mailing address found.");

            var existingMailing = existingPerson.PimsPersonAddresses.Where(a => a.AddressUsageTypeCode == AddressUsageTypes.Mailing).FirstOrDefault();
            if (existingMailing != null && existingMailing.AddressId == organizationAddress.AddressId)
            {
                existingPerson.PimsPersonAddresses.Remove(existingMailing);
                this.Context.Remove(existingMailing);
            }
        }

        #endregion
    }
}
