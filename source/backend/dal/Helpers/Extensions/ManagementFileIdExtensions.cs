using System;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// Extension and utility methods for Management File.
    /// </summary>
    public static class ManagementFileIdExtensions
    {
        /// <summary>
        /// Normalizes a management file ID search string by removing a leading 'M-' if present.
        /// </summary>
        /// <param name="input">The search string from the user.</param>
        /// <returns>The normalized string for numeric ID search.</returns>
        public static string NormalizeManagementFileIdSearch(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            var trimmed = input.Trim();
            if (trimmed.StartsWith("M-", StringComparison.OrdinalIgnoreCase))
            {
                return trimmed.Substring(2).Trim();
            }
            return trimmed;
        }
    }
}
