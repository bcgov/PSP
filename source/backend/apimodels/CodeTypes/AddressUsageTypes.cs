using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AddressUsageTypes
    {
        [EnumMember(Value = "BILLING")]
        BILLING,

        [EnumMember(Value = "ETLUNKN")]
        ETLUNKN,

        [EnumMember(Value = "MAILING")]
        MAILING,

        [EnumMember(Value = "RESIDNT")]
        RESIDNT,

        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,
    }
}
