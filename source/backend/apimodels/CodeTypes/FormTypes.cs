using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    /// <summary>
    /// [PIMS_FORM_TYPE] table.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum FormTypes
    {
        [EnumMember(Value = "FORM1")]
        FORM1,

        [EnumMember(Value = "FORM5")]
        FORM5,

        [EnumMember(Value = "FORM8")]
        FORM8,

        [EnumMember(Value = "FORM9")]
        FORM9,

        [EnumMember(Value = "H0074")]
        H0074,

        [EnumMember(Value = "H0443")]
        H0443,

        [EnumMember(Value = "H1005A")]
        H1005A,

        [EnumMember(Value = "H1005")]
        H1005,

        [EnumMember(Value = "H120")]
        H120,

        [EnumMember(Value = "H179A")]
        H179A,

        [EnumMember(Value = "H179P")]
        H179P,

        [EnumMember(Value = "H179T")]
        H179T,

        [EnumMember(Value = "LETTER")]
        LETTER,
    }
}
