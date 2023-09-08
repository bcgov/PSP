using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// AcquisitionExtensions static class, provides extension methods for acquisition files.
    /// </summary>
    public static class AcquisitionExtensions
    {
        private const string LEGACYSTAKEHOLDERSPATTER = @"<([^>]+)\|\|([^>]+)>";

        /// <summary>
        /// Extension method that returns a list of Legacy Interest Holders split by a pattern.
        /// </summary>
        /// <param name="acquisitionFile"></param>
        /// <returns></returns>
        public static List<string> GetLegacyInterestHolders(this PimsAcquisitionFile acquisitionFile)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));

            List<string> list = new();
            if (string.IsNullOrEmpty(acquisitionFile.LegacyStakeholder))
            {
                return list;
            }

            MatchCollection matches = Regex.Matches(acquisitionFile.LegacyStakeholder, LEGACYSTAKEHOLDERSPATTER);
            foreach (Match match in matches.Cast<Match>())
            {
                string value = match.Groups[1].Value;
                string property = match.Groups[2].Value;

                list.Add(value);
            }

            return list;
        }
    }
}
