using Entity = Pims.Dal.Entities;


namespace Pims.Api.Helpers.Converters
{
    /// <summary>
    /// OrganizationConverter static class, provides converters for organization.
    /// </summary>
    public static class OrganizationConverter
    {
        /// <summary>
        /// Extracts the organization name, or the specified 'organization' is a child it will return the parent name.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertOrganizationFullName(Entity.Organization source)
        {
            if (source?.ParentId == null) return source?.Name;
            return source.Parent?.Name;
        }

        /// <summary>
        /// Extract the sub-organization name.
        /// If the specified 'organization' is a parent it will return null.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertSubOrganizationFullName(Entity.Organization source)
        {
            return source?.ParentId == null ? null : source.Name;
        }
    }
}
