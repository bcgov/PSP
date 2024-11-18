using System.Linq;
using System.Security.Claims;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    public static class OrganizationExtensions
    {

        /// <summary>
        /// A user is supposed to only belong to one child organization or one parent organization.
        /// While in Keycloak these rules can be broken, we have to assume the first parent organization, or the first child organization is the user's primary.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static PimsOrganization GetOrganization(this ClaimsPrincipal user, PimsContext context)
        {
            var organizationIds = user.GetOrganizations();

            if (organizationIds == null || !organizationIds.Any())
            {
                return null;
            }

            var organizations = context.PimsOrganizations.Where(a => organizationIds.Contains(a.OrganizationId)).OrderBy(a => a.PrntOrganizationId);

            // If one of the organizations is a parent, return it.
            var parentOrganization = organizations.FirstOrDefault(a => a.PrntOrganizationId == null);
            if (parentOrganization != null)
            {
                return parentOrganization;
            }

            // Assume the first organization is their primary
            return organizations.FirstOrDefault();
        }
    }
}
