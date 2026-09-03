using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TeamProfileTypeTypes
    {
        [EnumMember(Value = "KEYCNTCT")]
        KEY_CONTACT,

        [EnumMember(Value = "LISTAGENT")]
        LISTING_AGENT,

        [EnumMember(Value = "MOTILEAD")]
        MOTT_LEAD,

        [EnumMember(Value = "MOTILAWYER")]
        MOTT_SOLICITOR,

        [EnumMember(Value = "NEGOTAGENT")]
        NEGOTIATION_AGENT,

        [EnumMember(Value = "PROPCOORD")]
        PROPERTY_COORDINATOR,

        [EnumMember(Value = "PROPAGENT")]
        PROPERTY_AGENT,

        [EnumMember(Value = "PROPANLYS")]
        PROPERTY_ANALYST,
    }
}