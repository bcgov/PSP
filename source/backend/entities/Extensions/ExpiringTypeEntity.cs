using System;

namespace Pims.Dal.Entities.Extensions
{
    public static class ExpiringTypeEntity
    {
        /// <summary>
        /// Determines whether a given type is currently expired, based on its EffectiveDate and ExpiryDate values.
        /// </summary>
        /// <param name="type">The type code.</param>
        /// <param name="currentDate">The current date.</param>
        /// <returns>True if the type is expired; false otherwise.</returns>
        public static bool IsExpiredType(this IExpiringTypeEntity<DateTime, DateTime?> type, DateTime? currentDate = null)
        {
            DateTime now = currentDate.HasValue ? (DateTime)currentDate?.Date : DateTime.UtcNow.Date;
            return (type.EffectiveDate.Date > now) || (type.ExpiryDate.HasValue && type.ExpiryDate?.Date <= now);
        }

        /// <summary>
        /// Determines whether a given type is currently expired, based on its EffectiveDate and ExpiryDate values.
        /// </summary>
        /// <param name="type">The type code.</param>
        /// <param name="currentDate">The current date.</param>
        /// <returns>True if the type is expired; false otherwise.</returns>
        public static bool IsExpiredType(this IExpiringTypeEntity<DateOnly, DateOnly?> type, DateTime? currentDate = null)
        {
            DateOnly now = DateOnly.FromDateTime(currentDate.HasValue ? (DateTime)currentDate?.Date : DateTime.Now.Date);
            return (type.EffectiveDate > now) || (type.ExpiryDate.HasValue && type.ExpiryDate <= now);
        }
    }
}
