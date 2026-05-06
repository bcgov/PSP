using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum CotactMethodTypes
    {
        [EnumMember(Value = "FAX")]
        FAX,

        [EnumMember(Value = "PERSEMAIL")]
        PERSEMAIL,

        [EnumMember(Value = "PERSMOBIL")]
        PERSMOBIL,

        [EnumMember(Value = "PERSPHONE")]
        PERSPHONE,

        [EnumMember(Value = "WORKEMAIL")]
        WORKEMAIL,

        [EnumMember(Value = "WORKMOBIL")]
        WORKMOBIL,

        [EnumMember(Value = "WORKPHONE")]
        WORKWORKPHONEMOBIL,
    }
}
