using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ConsultationOutcomeTypes
    {
        [EnumMember(Value = "APPRDENIED")]
        APPRDENIED,

        [EnumMember(Value = "APPRGRANTED")]
        APPRGRANTED,

        [EnumMember(Value = "CONSCOMPLTD")]
        CONSCOMPLTD,

        [EnumMember(Value = "CONSDISCONT")]
        CONSDISCONT,

        [EnumMember(Value = "INPROGRESS")]
        INPROGRESS,
    }
}
