using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pims.Core.Json;

namespace Pims.Core.Converters
{
    /// <summary>
    /// EnumValueJsonConverter class, provides a way to convert enum values.
    /// Serialization - Extract value from 'EnumValueAttribute' otherwise lowercase the enum name value.
    /// Deserialization - Ignore case.
    /// </summary>
    /// <typeparam name="T_Enum">The enum type.</typeparam>
    public class EnumValueJsonConverter<T_Enum> : JsonConverter<T_Enum>
        where T_Enum : struct, IConvertible
    {
        #region Methods

        /// <summary>
        /// Ignore case when parsing, otherwise return default.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override T_Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            var valid = Enum.TryParse(value, true, out T_Enum result);

            if (valid)
            {
                return result;
            }
            else
            {
                var fields = Enum.GetValues(typeof(T_Enum));
                foreach (var field in fields)
                {
                    var mi = typeof(T_Enum).GetMember(field.ToString());
                    var attr = mi[0].GetCustomAttribute<EnumValueAttribute>();
                    if (attr != null && string.CompareOrdinal(value, attr.Value) == 0)
                    {
                        return (T_Enum)field;
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// Extract name from 'EnumJsonAttribute' if exists, or return enum property naem in lowercase.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, T_Enum value, JsonSerializerOptions options)
        {
            var fi = typeof(T_Enum).GetField(value.ToString());
            var attr = fi.GetCustomAttribute<EnumValueAttribute>();

            if (attr != null)
            {
                writer.WriteStringValue(attr.Value);
            }
            else
            {
                writer.WriteStringValue($"{value}".ToLower());
            }
        }
        #endregion
    }
}
