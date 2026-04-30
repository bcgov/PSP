using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;
using Pims.Core.Helpers;
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

        public static string GetPropertyName(this PimsProperty property)
        {
            if (property == null)
            {
                return string.Empty;
            }

            return new PropertyIdentifiers(property).GetAsString();
        }

        public static string GetPropertyName(this IFilePropertyEntity fileProperty)
        {
            if (fileProperty == null)
            {
                return string.Empty;
            }

            return new PropertyIdentifiers(fileProperty).GetAsString();
        }
    }

    // Helper class to aggregate property identifiers into a single object.
    public class PropertyIdentifiers
    {
        public string Pid { get; set; }

        public string Pin { get; set; }

        public string SurveyPlanNumber { get; set; }

        public Geometry Location { get; set; }

        public PimsAddress Address { get; set; }

        public string DisplayName { get; set; }

        public PropertyIdentifiers()
        {
        }

        public PropertyIdentifiers(PimsProperty property)
        {
            Pid = property?.Pid != null ? property.Pid.ToString() : null;
            Pin = property?.Pin != null ? property.Pin.ToString() : null;
            SurveyPlanNumber = !string.IsNullOrEmpty(property?.SurveyPlanNumber) ? property?.SurveyPlanNumber : null;
            Location = property?.Location;
            Address = property?.Address;
        }

        public PropertyIdentifiers(IFilePropertyEntity fileProperty)
            : this(fileProperty?.Property)
        {
            DisplayName = fileProperty?.PropertyName;
        }

        public string GetAsString()
        {
            if (!string.IsNullOrEmpty(Pid))
            {
                return PidTranslator.ConvertPIDToDash(Pid);
            }

            if (!string.IsNullOrEmpty(Pin))
            {
                return Pin;
            }

            if (Address is not null)
            {
                return Address.FormatFullAddressString();
            }

            if (!string.IsNullOrEmpty(SurveyPlanNumber))
            {
                return SurveyPlanNumber;
            }

            if (!string.IsNullOrEmpty(DisplayName))
            {
                return DisplayName;
            }

            if (Location is not null)
            {
                var latitude = Location.Coordinate?.Y;
                var longitude = Location.Coordinate?.X;
                if (latitude is not null && longitude is not null)
                {
                    return $"({latitude}, {longitude})";
                }
            }

            // Fallback
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
