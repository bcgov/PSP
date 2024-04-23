using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LandActTypes
    {
        [EnumMember(Value = "Crown Grant")]
        CROWN_GRANT,

        [EnumMember(Value = "NOI")]
        NOI,

        [EnumMember(Value = "Section 15")]
        SECTION_15,

        [EnumMember(Value = "Section 16")]
        SECTION_16,

        [EnumMember(Value = "Section 17")]
        SECTION_17,

        [EnumMember(Value = "Section 66")]
        SECTION_66,

        [EnumMember(Value = "Transfer Admin")]
        TRANSFER_OF_ADMIN_AND_CONTROL,
    }
}
