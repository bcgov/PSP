using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// AddressExtensions static class, provides extension methods for projects.
    /// </summary>
    public static class AddressExtensions
    {
        /// <summary>
        /// Format address to string.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string FormatAddress(this PimsAddress address, bool includeMunicipality = false)
        {
            string municipality = includeMunicipality ? address.MunicipalityName : "";
            return address != null ? $"{address.StreetAddress1} {address.StreetAddress2} {address.StreetAddress3} {municipality}".Trim() : "";
        }
    }
}
