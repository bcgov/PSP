using System;
using Pims.Api.Helpers.Exceptions;

namespace Pims.Api.Helpers.Extensions
{
    /// <summary>
    /// ExceptionExtensions static class, provides extention methods for exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Throw an BadRequestException if the item is null.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="message"></param>
        /// <typeparam name="T">The type of the passed item.</typeparam>
        /// <exception type="BadRequestException">The item cannot be null.</exception>
        public static T ThrowBadRequestIfNull<T>(this T item, string message)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Argument cannot be null, empty or whitespace.", nameof(message));
            }

            return item ?? throw new BadRequestException(message);
        }
    }
}
