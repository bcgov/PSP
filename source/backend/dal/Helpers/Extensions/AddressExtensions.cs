using System;
using System.Text;
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
            string municipality = includeMunicipality ? address.MunicipalityName : string.Empty;
            return address != null ? $"{address.StreetAddress1} {address.StreetAddress2} {address.StreetAddress3} {municipality}".Trim() : string.Empty;
        }

        /// <summary>
        /// Format full address to string.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string FormatFullAddressString(this PimsAddress address)
        {
            StringBuilder stringBuilder = new();
            if (address is null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            if (!string.IsNullOrEmpty(address.StreetAddress1))
            {
                stringBuilder.Append(address.StreetAddress1);
            }
            if (!string.IsNullOrEmpty(address.StreetAddress2))
            {
                stringBuilder.Append(" " + address.StreetAddress2);
            }
            if (!string.IsNullOrEmpty(address.StreetAddress3))
            {
                stringBuilder.Append(" " + address.StreetAddress3);
            }
            if (!string.IsNullOrEmpty(address.MunicipalityName))
            {
                stringBuilder.Append(" " + address.MunicipalityName);
            }
            if (address.ProvinceState != null)
            {
                stringBuilder.Append(" " + address.ProvinceState.Code);
            }
            if (address.Country != null)
            {
                stringBuilder.Append(" " + address.Country.Description);
            }
            if (!string.IsNullOrEmpty(" " + address.PostalCode))
            {
                stringBuilder.Append(" " + address.PostalCode);
            }

            return stringBuilder.ToString();
        }
    }
}
