using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DispositionFormTypes
    {
        [EnumMember(Value = "H179RC")]
        H179RC,
    }
}
