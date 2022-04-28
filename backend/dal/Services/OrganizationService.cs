using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        readonly Repositories.IOrganizationRepository _organizationRepository;

        /// <summary>
        /// Creates a new instance of a OrganizationService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="organizationRepository"></param>
        public OrganizationService(ClaimsPrincipal user, ILogger<BaseService> logger, Repositories.IOrganizationRepository organizationRepository) : base(user, logger)
        {
            _organizationRepository = organizationRepository;
        }

        public PimsOrganization GetOrganization(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            return _organizationRepository.Get(id);
        }

        public PimsOrganization AddOrganization(PimsOrganization organization, bool userOverride)
        {
            organization.ThrowIfNull(nameof(organization));
            this.User.ThrowIfNotAuthorized(Permissions.ContactAdd);

            var createdOrganization = _organizationRepository.Add(organization, userOverride);
            _organizationRepository.CommitTransaction();

            return GetOrganization(createdOrganization.Id);
        }

        public PimsOrganization UpdateOrganization(PimsOrganization organization, long rowVersion)
        {
            organization.ThrowIfNull(nameof(organization));
            this.User.ThrowIfNotAuthorized(Permissions.ContactEdit);
            ValidateRowVersion(organization.Id, rowVersion);

            var updatedOrganization = _organizationRepository.Update(organization);
            _organizationRepository.CommitTransaction();

            return GetOrganization(updatedOrganization.Id);
        }

        public void ValidateRowVersion(long organizationId, long rowVersion)
        {
            if (_organizationRepository.GetRowVersion(organizationId) != rowVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this contact, please refresh the application and retry.");
            }
        }
    }
}
