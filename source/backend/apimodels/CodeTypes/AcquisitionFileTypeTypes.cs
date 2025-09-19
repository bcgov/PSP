using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum AcquisitionFileTypeTypes
    {
        [EnumMember(Value = "CONSEN")]
        CONSEN,

        [EnumMember(Value = "CRWNTNR")]
        CRWNTNR,

        [EnumMember(Value = "HISTORY")]
        HISTORY,

        [EnumMember(Value = "SECTN16")]
        SECTN16,

        [EnumMember(Value = "SECTN3")]
        SECTN3,

        [EnumMember(Value = "SECTN6")]
        SECTN6,

        [EnumMember(Value = "XFR")]
        XFR,
    }
}
