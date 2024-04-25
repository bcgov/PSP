using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    /// Provides a service layer to administrate organizations within the datasource.
    /// </summary>
    public class OrganizationRepository : BaseRepository<PimsOrganization>, IOrganizationRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a OrganizationService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public OrganizationRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<OrganizationRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        public long GetRowVersion(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            return this.Context.PimsOrganizations.AsNoTracking().Where(o => o.OrganizationId == id)?.Select(p => p.ConcurrencyControlNumber)?.FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the organization for the specified 'id' with reference objects..
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">Organization does not exists for the specified 'id'.</exception>
        /// <returns></returns>
        public PimsOrganization GetById(long id)
        {
            return this.Context.PimsOrganizations
               .Include(o => o.PimsOrganizationAddresses)
                   .ThenInclude(pa => pa.Address)
                   .ThenInclude(a => a.Country)
               .Include(o => o.PimsOrganizationAddresses)
                   .ThenInclude(pa => pa.Address)
                   .ThenInclude(a => a.ProvinceState)
               .Include(o => o.PimsOrganizationAddresses)
                   .ThenInclude(pa => pa.AddressUsageTypeCodeNavigation)
               .Include(o => o.PimsPersonOrganizations)
                   .ThenInclude(po => po.Person)
                        .ThenInclude(o => o.PimsContactMethods)
                            .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)
               .Include(o => o.PimsContactMethods)
                   .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)
               .Where(o => o.OrganizationId == id)
               .AsNoTracking()
               .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the specified organization contact to the datasource.
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="userOverride"></param>
        /// <returns></returns>
        public PimsOrganization Add(PimsOrganization organization, bool userOverride)
        {
            organization.ThrowIfNull(nameof(organization));
            if (!userOverride)
            {

                List<PimsOrganization> existingOrgs = this.Context.PimsOrganizations
                    .Where(o => o.OrganizationName == organization.OrganizationName)
                    .Include(o => o.PimsContactMethods).ToList();

                var isDuplicate = existingOrgs.Any(
                                o => o.PimsContactMethods.Any(
                                    ec => organization.PimsContactMethods.Any(
                                        pc => pc.ContactMethodValue.ToLower() == ec.ContactMethodValue.ToLower() &&
                                        pc.ContactMethodTypeCode == ec.ContactMethodTypeCode)));

                if (isDuplicate)
                {
                    throw new DuplicateEntityException("Duplicate organization found");
                }
            }

            this.Context.PimsOrganizations.Add(organization);
            return organization;
        }

        /// <summary>
        /// Update the specified organization contact.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public PimsOrganization Update(PimsOrganization organization)
        {
            organization.ThrowIfNull(nameof(organization));
            this.User.ThrowIfNotAuthorized(Permissions.ContactEdit);

            var orgId = organization.OrganizationId;
            var existingOrganization = this.Context.PimsOrganizations.Where(o => o.OrganizationId == orgId).FirstOrDefault()
                 ?? throw new KeyNotFoundException();

            // update main entity - PimsOrganization
            Context.Entry(existingOrganization).CurrentValues.SetValues(organization);

            // update direct relationships - contact_methods, organizations
            this.Context.UpdateChild<PimsOrganization, long, PimsContactMethod, long>(o => o.PimsContactMethods, orgId, organization.PimsContactMethods.ToArray());

            // update addresses via UpdateGrandchild method
            this.Context.UpdateGrandchild<PimsOrganization, long, PimsOrganizationAddress>(o => o.PimsOrganizationAddresses, oa => oa.Address, orgId, organization.PimsOrganizationAddresses.ToArray());

            return existingOrganization;
        }
        #endregion
    }
}
