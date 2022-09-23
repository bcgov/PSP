using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    /// <summary>
    /// Common area for error messages.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum UpdateType
    {
        /// <summary>
        /// Add a new entry.
        /// </summary>
        [EnumMember(Value = "add")]
        Add,

        /// <summary>
        /// Update or modify.
        /// </summary>
        [EnumMember(Value = "update")]
        Update,

        /// <summary>
        /// Remove an entry.
        /// </summary>
        [EnumMember(Value = "delete")]
        Delete,
    }
}
