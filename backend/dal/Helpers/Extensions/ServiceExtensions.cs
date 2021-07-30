using Pims.Dal.Services;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// ServiceExtensions static class, provides extension methods for services.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// A parcel can only be updated or removed if not within an active project or user has admin-properties permission
        /// </summary>
        /// <param name="service"></param>
        /// <param name="parcel"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static void ThrowIfNotAllowedToUpdate(this BaseService service, Entity.Parcel parcel, ProjectOptions options)
        {
            // Stub after projects removed.
        }

        /// <summary>
        /// A building can only be updated or removed if not within an active project or user has admin-properties permission
        /// </summary>
        /// <param name="service"></param>
        /// <param name="building"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static void ThrowIfNotAllowedToUpdate(this BaseService service, Entity.Building building, ProjectOptions options)
        {
            // Stub after projects removed.
        }
    }
}
