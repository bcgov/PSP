using System;
using System.Text.RegularExpressions;

namespace Pims.Api.Helpers.Converters
{
    /// <summary>
    /// PropertyConverter static class, provides converters for property.
    /// </summary>
    public static class PropertyConverter
    {
        /// <summary>
        /// Convert the formatted PID string into a number.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static int ConvertPID(string pid)
        {
            int.TryParse(pid?.Replace("-", "") ?? "", out int value);
            return value;
        }

        /// <summary>
        /// Try and convert a pid (in any format) to a pid in ddd-ddd-ddd format.
        /// Left pad the incoming strings with 0 before formatting to ensure the pid is in the desired format.
        /// The regex simply creates match groups of 3 digits.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string ConvertPIDToDash(string pid)
        {
            return String.Join("-", Regex.Split(pid.Replace("-", "").PadLeft(9, '0'), @"(?<=\G.{3})(?!$)"));
        }
    }
}
