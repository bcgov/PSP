using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseSecurityDepositTypes
    {
        [EnumMember(Value = "SECURITY")]
        SECURITY,

        [EnumMember(Value = "PET")]
        PET,

        [EnumMember(Value = "OTHER")]
        OTHER,
    }
}
