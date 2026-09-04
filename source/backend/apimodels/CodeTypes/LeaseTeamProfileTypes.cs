using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseTeamProfileTypes
    {
        [EnumMember(Value = "KEYCNTCT")]
        KEYCNTCT,

        [EnumMember(Value = "LANDOPSMGR")]
        LANDOPSMGR,

        [EnumMember(Value = "LANDPRJMGR")]
        LANDPRJMGR,

        [EnumMember(Value = "MOTTCONTACT")]
        MOTTCONTACT,

        [EnumMember(Value = "MOTTLAWYER")]
        MOTTLAWYER,

        [EnumMember(Value = "PROPADMIN")]
        PROPADMIN,

        [EnumMember(Value = "PROPANALYST")]
        PROPANALYST,

        [EnumMember(Value = "PROPCOORD")]
        PROPCOORD,
    }
}
