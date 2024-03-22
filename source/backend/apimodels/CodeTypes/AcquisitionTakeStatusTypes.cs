using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AcquisitionTakeStatusTypes
    {
        [EnumMember(Value = "CANCELLED")]
        CANCELLED,

        [EnumMember(Value = "COMPLETE")]
        COMPLETE,

        [EnumMember(Value = "INPROGRESS")]
        INPROGRESS,
    }
}
