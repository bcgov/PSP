using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum ConvertToTypes
    {
        [EnumMember(Value = "pdf")]
        PDF,
        [EnumMember(Value = "xlsx")]
        XLSX,
        [EnumMember(Value = "docx")]
        DOCX,
        [EnumMember(Value = "ods")]
        ODS,
        [EnumMember(Value = "csv")]
        CSV,
        [EnumMember(Value = "txt")]
        TXT,
    }
}
