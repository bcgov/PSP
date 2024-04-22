using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum FormDocumentType
    {
        // Payment requisition (H120)
        [EnumMember(Value = "H120")]
        H120,

        // Offer agreement - Section 3 (H179 A)
        [EnumMember(Value = "H179A")]
        H179A,

        // Offer agreement - Partial (H179 P)
        [EnumMember(Value = "H179P")]
        H179P,

        // Offer agreement - Total (H179 T)
        [EnumMember(Value = "H179T")]
        H179T,

        // Letter
        [EnumMember(Value = "LETTER")]
        LETTER,

        // Conditions of Entry (H0443)
        [EnumMember(Value = "H0443")]
        H0443,

        // License of Occupation (H0074)
        [EnumMember(Value = "H0074")]
        H0074,

        // Notice of Expropriation (Form 1)
        [EnumMember(Value = "FORM1")]
        FORM1,

        // Certificate of Approval of Expropriation (Form 5)
        [EnumMember(Value = "FORM5")]
        FORM5,

        // Notice of Advance Payment (Form 8)
        [EnumMember(Value = "FORM8")]
        FORM8,

        // Vesting Notice (Form 9)
        [EnumMember(Value = "FORM9")]
        FORM9,

        // H1005A
        [EnumMember(Value = "H1005A")]
        H1005A,
    }
}
