using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum UserTypeTypes
    {
        [EnumMember(Value = "CONTRACT")]
        CONTRACT,

        [EnumMember(Value = "MINSTAFF")]
        MINSTAFF,
    }
}
