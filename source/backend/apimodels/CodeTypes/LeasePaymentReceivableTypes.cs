using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeasePaymentReceivableTypes
    {
        [EnumMember(Value = "PYBLBCTFA")]
        PYBLBCTFA,

        [EnumMember(Value = "PYBLMOTI")]
        PYBLMOTI,

        [EnumMember(Value = "RCVBL")]
        RCVBL,
    }
}
