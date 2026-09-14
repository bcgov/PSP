using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DispositionTeamProfileTypes
    {
        [EnumMember(Value = "KEYCNTCT")]
        KEYCNTCT,

        [EnumMember(Value = "LISTAGENT")]
        LISTAGENT,

        [EnumMember(Value = "MOTILAWYER")]
        MOTILAWYER,

        [EnumMember(Value = "MOTILEAD")]
        MOTILEAD,

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
