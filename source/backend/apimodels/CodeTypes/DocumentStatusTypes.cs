using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DocumentStatusTypes
    {

        [EnumMember(Value = "AMENDD")]
        AMENDD,

        [EnumMember(Value = "APPROVD")]
        APPROVD,

        [EnumMember(Value = "CNCLD")]
        CNCLD,

        [EnumMember(Value = "DRAFT")]
        DRAFT,

        [EnumMember(Value = "FINAL")]
        FINAL,

        [EnumMember(Value = "NONE")]
        NONE,

        [EnumMember(Value = "RGSTRD")]
        RGSTRD,

        [EnumMember(Value = "SENT")]
        SENT,

        [EnumMember(Value = "SIGND")]
        SIGND,

        [EnumMember(Value = "UNREGD")]
        UNREGD,
    }
}
