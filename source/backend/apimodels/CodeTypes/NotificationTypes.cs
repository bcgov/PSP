using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum NotificationTypes
    {
        [EnumMember(Value = "TAKE_SRW")]
        TAKE_SRW,

        [EnumMember(Value = "TAKE_LAT")]
        TAKE_LAT,

        [EnumMember(Value = "TAKE_LTC")]
        TAKE_LTC,

        [EnumMember(Value = "TAKE_LPYBLE")]
        TAKE_LPYBLE,

        [EnumMember(Value = "L_RENEWAL")]
        L_RENEWAL,

        [EnumMember(Value = "L_INSURANCE")]
        L_INSURANCE,

        [EnumMember(Value = "L_CONSULTFN")]
        L_CONSULTFN,

        [EnumMember(Value = "NOC")]
        NOC,

        [EnumMember(Value = "ADV_PAY")]
        ADV_PAY,

        [EnumMember(Value = "AGMT_SIGND")]
        AGMT_SIGND,

        [EnumMember(Value = "EXPROPH_APPEFFDT")]
        EXPROPH_APPEFFDT,
    }
}
