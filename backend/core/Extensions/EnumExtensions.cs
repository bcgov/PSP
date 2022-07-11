using System;
using System.Linq;

namespace Pims.Core.Extensions
{
    /// <summary>
    /// EnumExtensions static class, provides extension methods for enums.
    /// </summary>
    public static class EnumExtensions
    {
        #region Methods

        /// <summary>
        /// Returns to name of the enum in lowercase.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLower(this Enum value)
        {
            return value.ToString().ToLower();
        }

        /// <summary>
        /// Provides a way to do a Contains function with the specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool In<T>(this T val, params T[] values)
            where T : struct
        {
            return values.Contains(val);
        }

        /// <summary>
        /// Convert the enum value to the destination enum value with the same name.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TDestination ConvertTo<TSource, TDestination>(this TSource value)
            where TSource : struct, IConvertible
            where TDestination : struct, IConvertible
        {
            return Enum.TryParse(typeof(TDestination), value.ToString(), out object result) ? (TDestination)result : default;
        }
        #endregion
    }
}
