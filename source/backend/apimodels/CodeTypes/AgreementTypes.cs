using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AgreementTypes
    {
        [EnumMember(Value = "H0074")]
        H0074,

        [EnumMember(Value = "H179A")]
        H179A,

        [EnumMember(Value = "H179P")]
        H179P,

        [EnumMember(Value = "H179T")]
        H179T,

        [EnumMember(Value = "H179FSPART")]
        H179FSPART,

        [EnumMember(Value = "TOTAL")]
        TOTAL,

        [EnumMember(Value = "H179PTO")]
        H179PTO,
    }
}
