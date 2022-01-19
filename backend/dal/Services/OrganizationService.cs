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

namespace Pims.Dal.Services
{
    /// <summary>
    /// Provides a service layer to administrate organizations within the datasource.
    /// </summary>
    public class OrganizationService : BaseService<PimsOrganization>, IOrganizationService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a OrganizationService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public OrganizationService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<OrganizationService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get all organizations from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IEnumerable<PimsOrganization> GetAll()
        {
            return this.Context.PimsOrganizations.AsNoTracking().ToArray();
        }

        /// <summary>
        /// Get the organization for the specified 'id' with reference objects..
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">Organization does not exists for the specified 'id'.</exception>
        /// <returns></returns>
        public PimsOrganization Get(long id)
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
               .Include(o => o.PimsContactMethods)
                   .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)
               .Where(o => o.OrganizationId == id)
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
                    .Where(o => EF.Functions.Collate(o.OrganizationName, SqlCollation.LATIN_GENERAL_CASE_INSENSITIVE) == organization.OrganizationName)
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

            var createdOrganization = this.Context.PimsOrganizations.Add(organization);
            this.Context.CommitTransaction();

            return createdOrganization.Entity;
        }
        #endregion
    }
}
