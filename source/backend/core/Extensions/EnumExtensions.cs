using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Pims.Core.Security;

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
        /// <typeparam name="T_Source"></typeparam>
        /// <typeparam name="T_Destination"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T_Destination ConvertTo<T_Source, T_Destination>(this T_Source value)
            where T_Source : struct, IConvertible
            where T_Destination : struct, IConvertible
        {
            return Enum.TryParse(typeof(T_Destination), value.ToString(), out object result) ? (T_Destination)result : default;
        }

        /// <summary>
        /// Get the Keycloak name value of the specified permission.
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static string GetName(this Permissions permission)
        {
            var enumType = typeof(Permissions);
            var memberInfos = enumType.GetMember(permission.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
            var attribute = (DisplayAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            return attribute.Name;
        }
        #endregion
    }
}
