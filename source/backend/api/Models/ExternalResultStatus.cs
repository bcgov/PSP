using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models
{
    /// <summary>
    /// Status of an external call.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ExternalResultStatus
    {
        /// <summary>
        /// The call was successful.
        /// </summary>
        [EnumMember(Value = "success")]
        Success,

        /// <summary>
        /// Error occured.
        /// </summary>
        [EnumMember(Value = "error")]
        Error,

        /// <summary>
        /// The external call was not executed.
        /// </summary>
        [EnumMember(Value = "not-executed")]
        NotExecuted,
    }
}
