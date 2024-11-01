using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum SubfileInterestTypes
    {
        [EnumMember(Value = "EASEMENT")]
        EASEMENT,

        [EnumMember(Value = "LICOCCUPY")]
        LICOCCUPY,

        [EnumMember(Value = "LIEN")]
        LIEN,

        [EnumMember(Value = "MORTGAGE")]
        MORTGAGE,

        [EnumMember(Value = "OTHER")]
        OTHER,

        [EnumMember(Value = "SRWUTILITY")]
        SRWUTILITY,

        [EnumMember(Value = "STRATALOT")]
        STRATALOT,

        [EnumMember(Value = "SUBTENANT")]
        SUBTENANT,

        [EnumMember(Value = "TENANT")]
        TENANT,

        [EnumMember(Value = "XINGPERMIT")]
        XINGPERMIT,
    }
}
