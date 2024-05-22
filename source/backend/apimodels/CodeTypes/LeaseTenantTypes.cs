using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseTenantTypes
    {

        [EnumMember(Value = "ASGN")]
        ASGN,

        [EnumMember(Value = "PMGR")]
        PMGR,

        [EnumMember(Value = "REP")]
        REP,

        [EnumMember(Value = "TEN")]
        TEN,

        [EnumMember(Value = "UNK")]
        UNK,
    }
}
