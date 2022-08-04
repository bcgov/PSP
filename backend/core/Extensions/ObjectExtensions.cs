using System;
using System.Linq;
using System.Reflection;
using NetTopologySuite.Geometries;

namespace Pims.Core.Extensions
{
    /// <summary>
    /// ObjectExtensions static class, provides extension methods for generic objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Copies the public properties from the 'source' object to the 'destination' object.
        /// It will only copy public properties that have a 'set' and are of the following types, primitive, enum, string.
        /// Or an array of those same types.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <typeparam name="T_Source"></typeparam>
        /// <typeparam name="T_Destination"></typeparam>
        public static T_Destination CopyValues<T_Source, T_Destination>(this T_Source source, T_Destination destination)
            where T_Source : class
            where T_Destination : class
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var type = typeof(T_Destination);
            var sProps = typeof(T_Source)
                .GetCachedProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType.IsPrimitive
                    || p.PropertyType.IsEnum
                    || p.PropertyType == typeof(string)
                    || p.PropertyType == typeof(Point)
                    || p.PropertyType == typeof(Geometry)
                    || (p.PropertyType.IsGenericType
                        && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                        && Nullable.GetUnderlyingType(p.PropertyType).IsPrimitive)
                    || (p.PropertyType.IsEnumerable()
                        && p.PropertyType.GetItemType().IsPrimitive)).ToDictionary(p => p.Name);
            var dProps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            foreach (var dProp in dProps)
            {
                if (!sProps.ContainsKey(dProp.Name))
                {
                    continue;
                }

                var sProp = sProps[dProp.Name];

                if (sProp.PropertyType == dProp.PropertyType && dProp.GetSetMethod() != null)
                {
                    var value = sProp.GetValue(source);
                    if (dProp.PropertyType.IsEnumerable())
                    {
                        if (dProp.PropertyType.IsArray)
                        {
                            var array = value as Array;
                            dProp.SetValue(destination, array, null);
                        }
                    }
                    else
                    {
                        dProp.SetValue(destination, value);
                    }
                }
            }

            return destination;
        }

        /// <summary>
        /// Checks if the 'value' is in the specified 'set'.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="set"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsIn<T>(this T value, params T[] set)
        {
            return set.Contains(value);
        }
    }
}
