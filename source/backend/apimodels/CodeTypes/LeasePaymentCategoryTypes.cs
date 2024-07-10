using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeasePaymentCategoryTypes
    {
        [EnumMember(Value = "ADDL")]
        ADDL,
        [EnumMember(Value = "BASE")]
        BASE,
        [EnumMember(Value = "VBL")]
        VBL,
    }
}
