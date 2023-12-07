using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    public static class OrganizationExtensions
    {
        /// <summary>
        /// Get the organization name from the parent if there is a parent, or the current organization if there is no parent.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetOrganizationName(this PimsOrganization organization)
        {
            return organization?.PrntOrganizationId.HasValue ?? false ? organization?.PrntOrganization?.OrganizationName : organization?.OrganizationName;
        }

        /// <summary>
        /// Get the sub organization name if there is a parent, otherwise return null.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetSubOrganizationName(this PimsOrganization organization)
        {
            return organization?.PrntOrganizationId.HasValue ?? false ? organization?.OrganizationName : null;
        }
    }
}
