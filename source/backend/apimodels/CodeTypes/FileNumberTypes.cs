using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum FileNumberTypes
    {
        [EnumMember(Value = "LISNO")]
        LISNO,

        [EnumMember(Value = "OTHER")]
        OTHER,

        [EnumMember(Value = "PROPNEG")]
        PROPNEG,

        [EnumMember(Value = "PSNO")]
        PSNO,

        [EnumMember(Value = "PUBWORKS")]
        PUBWORKS,

        [EnumMember(Value = "REGLSLIC")]
        REGLSLIC,

        [EnumMember(Value = "RESERVE")]
        RESERVE,
    }
}
