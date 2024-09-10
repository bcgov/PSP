using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeaseInsuranceTypes
    {
        [EnumMember(Value = "AIRCRAFT")]
        AIRCRAFT,

        [EnumMember(Value = "GENERAL")]
        GENERAL,

        [EnumMember(Value = "MARINE")]
        MARINE,

        [EnumMember(Value = "ACCIDENT")]
        ACCIDENT,

        [EnumMember(Value = "UAVDRONE")]
        UAVDRONE,

        [EnumMember(Value = "VEHICLE")]
        VEHICLE,

        [EnumMember(Value = "OTHER")]
        OTHER,
    }
}
