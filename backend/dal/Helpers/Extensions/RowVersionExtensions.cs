using System;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// RowVersionExtensions static class, provides extension methods for RowVersions.
    /// </summary>
    public static class RowVersionExtensions
    {
        /// <summary>
        /// Convert the string rowversion to a byte array.
        /// </summary>
        /// <param name="rowversion"></param>
        /// <returns></returns>
        public static byte[] ConvertRowVersion(this string rowversion)
        {
            return Convert.FromBase64String(rowversion);
        }

        /// <summary>
        /// Convert the byte array rowversion to a string.
        /// </summary>
        /// <param name="rowversion"></param>
        /// <returns></returns>
        public static string ConvertRowVersion(this byte[] rowversion)
        {
            return Convert.ToBase64String(rowversion);
        }
    }
}
