using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AgreementStatusTypes
    {
        [EnumMember(Value = "CANCELLED")]
        CANCELLED,

        [EnumMember(Value = "DRAFT")]
        DRAFT,

        [EnumMember(Value = "FINAL")]
        FINAL,

    }
}
