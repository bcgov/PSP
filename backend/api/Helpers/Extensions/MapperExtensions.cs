using Pims.Api.Models;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Extensions
{
    /// <summary>
    /// MapperExtensions static class, provides extension methods to help mapping properties.
    /// </summary>
    public static class MapperExtensions
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

        /// <summary>
        /// Null coalescing method to get the type from an id.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeId(this TypeModel<string> type)
        {
            return type?.Id;
        }
    }
}
