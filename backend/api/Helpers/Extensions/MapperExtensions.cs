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
        public static string GetOrganizationName(this Organization organization)
        {
            return organization?.ParentId.HasValue ?? false ? organization?.Parent?.Name : organization?.Name;
        }

        /// <summary>
        /// Get the sub organization name if there is a parent, otherwise return null.
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static string GetSubOrganizationName(this Organization organization)
        {
            return organization?.ParentId.HasValue ?? false ? organization?.Name : null;
        }
    }
}
