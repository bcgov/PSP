using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TakeStatusTypes
    {
        [EnumMember(Value = "INPROGRESS")]
        INPROGRESS,

        [EnumMember(Value = "CANCELLED")]
        CANCELLED,

        [EnumMember(Value = "COMPLETE")]
        COMPLETE,
    }
}
