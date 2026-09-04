using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AcquisitionTeamProfileTypes
    {
        [EnumMember(Value = "EXPRAGENT")]
        EXPRAGENT,

        [EnumMember(Value = "KEYCNTCT")]
        KEYCNTCT,

        [EnumMember(Value = "MOTILAWYER")]
        MOTILAWYER,

        [EnumMember(Value = "NEGOTAGENT")]
        NEGOTAGENT,

        [EnumMember(Value = "PROPAGENT")]
        PROPAGENT,

        [EnumMember(Value = "PROPANLYS")]
        PROPANLYS,

        [EnumMember(Value = "PROPCOORD")]
        PROPCOORD,
    }
}
