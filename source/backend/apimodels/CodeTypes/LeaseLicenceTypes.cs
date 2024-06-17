using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseLicenceTypes
    {
        [EnumMember(Value = "LICONSTRC")]
        LICONSTRC,

        [EnumMember(Value = "LIMOTIPRJ")]
        LIMOTIPRJ,

        [EnumMember(Value = "LIOCCACCS")]
        LIOCCACCS,

        [EnumMember(Value = "LIOCCTTLD")]
        LIOCCTTLD,

        [EnumMember(Value = "LIOCCUSE")]
        LIOCCUSE,

        [EnumMember(Value = "LIOCCUTIL")]
        LIOCCUTIL,

        [EnumMember(Value = "LIPPUBHWY")]
        LIPPUBHWY,

        [EnumMember(Value = "LSGRND")]
        LSGRND,

        [EnumMember(Value = "LSREG")]
        LSREG,

        [EnumMember(Value = "LSUNREG")]
        LSUNREG,

        [EnumMember(Value = "MANUFHOME")]
        MANUFHOME,

        [EnumMember(Value = "OTHER")]
        OTHER,

        [EnumMember(Value = "RESLNDTEN")]
        RESLNDTEN,

        [EnumMember(Value = "ROADXING")]
        ROADXING,
    }
}
