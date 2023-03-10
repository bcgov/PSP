using System;
using System.Data.SqlTypes;
using System.Globalization;

namespace Pims.Core.Extensions
{
    /// <summary>
    /// DateExtensions static class, provides extension methods for dates.
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// Return the appropriate year that represents the fiscal year for the specified 'date'.
        /// The beginning of the fiscal year is April 1st.
        /// </summary>
        /// <example>
        /// mm/dd/yyyy
        /// 12/01/2018 = 2018/2019
        /// 01/01/2019 = 2018/2019
        /// 05/01/2019 = 2019/2020.
        /// </example>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetFiscalYear(this DateTime date)
        {
            return date.Month >= 4 ? date.Year + 1 : date.Year;
        }

        /// <summary>
        /// convert an int representing a year into a date representing the start of that fiscal year.
        /// </summary>
        /// <param name="fiscalYear"></param>
        /// <returns></returns>
        public static DateTime ToFiscalYearDate(this int fiscalYear)
        {
            return new DateTime(fiscalYear, 4, 1);
        }

        /// <summary>
        /// Generate the fiscal year string value (i.e. 20/21).
        /// The result treats the specified 'fiscalYear' as the first year.
        /// </summary>
        /// <param name="fiscalYear"></param>
        /// <returns></returns>
        public static string FiscalYear(this int fiscalYear)
        {
            return $"{fiscalYear.ToString(CultureInfo.InvariantCulture).Substring(2, 2)}/{(fiscalYear + 1).ToString(CultureInfo.InvariantCulture).Substring(2, 2)}";
        }

        /// <summary>
        /// Return null if this date value is set to or less then SQLDATETIME.MIN_VALUE.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? FilterSqlMinDate(this DateTime? date)
        {
            return date.HasValue && date.Value <= (DateTime)SqlDateTime.MinValue ? null : date;
        }

        /// <summary>
        /// Return null if this date value is set to or less then SQLDATETIME.MIN_VALUE.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? FilterSqlMinDate(this DateTime date)
        {
            return date <= (DateTime)SqlDateTime.MinValue ? null : date;
        }
    }
}
