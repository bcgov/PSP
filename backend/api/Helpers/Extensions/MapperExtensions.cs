using System.Collections.Generic;
using System.Linq;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Extensions
{
    /// <summary>
    /// MapperExtensions static class, provides extension methods to help mapping properties.
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Get the agency code from the parent if there is a parent, or the current agency if there is no parent.
        /// </summary>
        /// <param name="agency"></param>
        /// <returns></returns>
        public static string GetAgencyCode(this Agency agency)
        {
            return agency?.ParentId.HasValue ?? false ? agency?.Parent?.Code : agency?.Code;
        }

        /// <summary>
        /// Get the agency name from the parent if there is a parent, or the current agency if there is no parent.
        /// </summary>
        /// <param name="agency"></param>
        /// <returns></returns>
        public static string GetAgencyName(this Agency agency)
        {
            return agency?.ParentId.HasValue ?? false ? agency?.Parent?.Name : agency?.Name;
        }

        /// <summary>
        /// Get the sub agency code if there is no parent, otherwise return null.
        /// </summary>
        /// <param name="agency"></param>
        /// <returns></returns>
        public static string GetSubAgencyCode(this Agency agency)
        {
            return agency?.ParentId.HasValue ?? false ? agency?.Code : null;
        }

        /// <summary>
        /// Get the sub agency name if there is a parent, otherwise return null.
        /// </summary>
        /// <param name="agency"></param>
        /// <returns></returns>
        public static string GetSubAgencyName(this Agency agency)
        {
            return agency?.ParentId.HasValue ?? false ? agency?.Name : null;
        }

        /// <summary>
        /// Get the land area of the first parcel;
        /// </summary>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public static double GetLandArea(this IEnumerable<ParcelBuilding> parcels)
        {
            return parcels.FirstOrDefault()?.Parcel.LandArea ?? 0;
        }

        /// <summary>
        /// Get the zoning of the first parcel.
        /// </summary>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public static string GetZoning(this IEnumerable<ParcelBuilding> parcels)
        {
            return parcels.FirstOrDefault()?.Parcel.Zoning ?? "";
        }

        /// <summary>
        /// Get the zoning potential of the first parcel.
        /// </summary>
        /// <param name="parcels"></param>
        /// <returns></returns>
        public static string GetZoningPotential(this IEnumerable<ParcelBuilding> parcels)
        {
            return parcels.FirstOrDefault()?.Parcel.ZoningPotential ?? "";
        }
    }
}
