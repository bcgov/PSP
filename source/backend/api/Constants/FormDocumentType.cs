using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
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
        [EnumMember(Value = "Letter")]
        LETTER,

        // Conditions of Entry (H0443)
        [EnumMember(Value = "H0443")]
        H0443,
    }
}
