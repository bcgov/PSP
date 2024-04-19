using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    /// <summary>
    /// Status of an external call.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ExternalResponseStatus
    {
        /// <summary>
        /// The call was successful.
        /// </summary>
        [EnumMember(Value = "Success")]
        Success,

        /// <summary>
        /// Error occured.
        /// </summary>
        [EnumMember(Value = "Error")]
        Error,

        /// <summary>
        /// The external call was not executed.
        /// </summary>
        [EnumMember(Value = "NotExecuted")]
        NotExecuted,
    }
}
