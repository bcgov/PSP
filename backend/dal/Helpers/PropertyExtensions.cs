using System.Text.RegularExpressions;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyExtensions static class, provides an extensions methods for property entities.
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Convert the formatted PID string into a number.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static int ConvertPID(this string pid)
        {
            return int.TryParse(pid?.Replace("-", string.Empty) ?? string.Empty, out int value) ? value : 0;
        }

        /// <summary>
        /// Try and convert a pid (in any format) to a pid in ddd-ddd-ddd format.
        /// Left pad the incoming strings with 0 before formatting to ensure the pid is in the desired format.
        /// The regex simply creates match groups of 3 digits.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string ConvertPIDToDash(this string pid)
        {
            return string.Join("-", Regex.Split(pid.Replace("-", string.Empty).PadLeft(9, '0'), @"(?<=\G.{3})(?!$)"));
        }
    }
}
