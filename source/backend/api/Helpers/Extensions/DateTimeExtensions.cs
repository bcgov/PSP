using System;

namespace Pims.Api.Helpers.Extensions
{
    /// <summary>
    /// DateTimeExtensions static class, provides extension methods for DateTime.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert the specified date to the fiscal year.
        /// This returns the last year of the fiscal calendar (i.e. 2020/2021 will return 2021).
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int ToFiscalYear(this DateTime date)
        {
            return date.Month >= 4 ? date.AddYears(1).Year : date.Year;
        }
    }
}
