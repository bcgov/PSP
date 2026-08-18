using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ConsultationTypeTypes
    {
        [EnumMember(Value = "1STNATION")]
        FIRSTNATION,

        [EnumMember(Value = "DISTRICT")]
        DISTRICT,

        [EnumMember(Value = "ENGINEERG")]
        ENGINEERG,

        [EnumMember(Value = "HQ")]
        HQ,

        [EnumMember(Value = "OTHER")]
        OTHER,

        [EnumMember(Value = "REGPLANG")]
        REGPLANG,

        [EnumMember(Value = "REGPRPSVC")]
        REGPRPSVC,

        [EnumMember(Value = "STRATRE")]
        STRATRE,
    }
}
