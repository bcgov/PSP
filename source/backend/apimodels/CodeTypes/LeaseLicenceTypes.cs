using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseLicenceTypes
    {
        [EnumMember(Value = "AMNDAGREE")]
        AMNDAGREE,

        [EnumMember(Value = "BLDGLSRCV")]
        BLDGLSRCV,

        [EnumMember(Value = "LIOCCHMK")]
        LIOCCHMK,

        [EnumMember(Value = "LIPPUBHWY")]
        LIPPUBHWY,

        [EnumMember(Value = "LOOBCTFA")]
        LOOBCTFA,

        [EnumMember(Value = "LSGRND")]
        LSGRND,

        [EnumMember(Value = "LSREG")]
        LSREG,

        [EnumMember(Value = "LSUNREG")]
        LSUNREG,

        [EnumMember(Value = "LTRINDMNY")]
        LTRINDMNY,

        [EnumMember(Value = "LTRINTENT")]
        LTRINTENT,

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
