using System.Collections.Generic;
using System.Linq;
using Pims.Dal.Entities;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// PropertyExtensions static class, provides extension methods for properties.
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Generates a string concatenating the property historical numbers into a string.
        /// Example: 'LIS: 123; PS, 1234;'.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetHistoricalNumbersAsString(this PimsProperty property)
        {
            // Group by type
            var groupedHistorical = property.PimsHistoricalFileNumbers.GroupBy(ph => ph.HistoricalFileNumberTypeCode, ph => ph, (key, g) => new HistoricalGroup(g));

            // Sort by group

            // Print Group

            // Print item
            return string.Join("; ", groupedHistorical.Select(g => g.GetAsString()));
        }

        public static string GetPropertyName(this IFilePropertyEntity fileProperty)
        {
            var property = fileProperty?.Property;
            if(property == null)
            {
                return string.Empty;
            }

            if (property.Pid.HasValue && property?.Pid.Value.ToString().Length > 0 && property?.Pid != '0')
            {
                return $"{property.Pid:000-000-000}";
            }
            else if (property.Pin.HasValue && property?.Pin.Value.ToString()?.Length > 0 && property?.Pin != '0')
            {
                return property.Pin.ToString();
            }
            else if (property?.SurveyPlanNumber != null && property?.SurveyPlanNumber.Length > 0)
            {
                return property.SurveyPlanNumber;
            }
            else if (property?.Location != null)
            {
                return $"{property.Location.Coordinate.X}, {property.Location.Coordinate.Y}";
            }
            else if (property?.Address != null)
            {
                return property.Address.FormatAddress();
            }
            return string.Empty;
        }
    }

    // Helper class to aggregate historical numbers by type.
    public class HistoricalGroup
    {
        private readonly IEnumerable<PimsHistoricalFileNumber> historicalFileNumbers;

        public HistoricalGroup(IEnumerable<PimsHistoricalFileNumber> historicalFileNumbers)
        {
            // Remove duplicates
            this.historicalFileNumbers = historicalFileNumbers.DistinctBy(hf => hf.HistoricalFileNumber);
        }

        public string GetAsString()
        {
            return string.Join(", ", this.historicalFileNumbers.Select(h => h.HistoricalFileNumberTypeCodeNavigation.GetTypeDescriptionOther(h.OtherHistFileNumberTypeCode) + ": " + h.HistoricalFileNumber));
        }
    }
}
