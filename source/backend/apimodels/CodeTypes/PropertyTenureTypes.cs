using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    /// <summary>
    /// [PIMS_PROPERTY_TENURE_TYPE] table.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum PropertyTenureTypes
    {
        [EnumMember(Value = "CLOSEDRD")]
        CLOSEDRD,

        [EnumMember(Value = "FSBCTFA")]
        FSBCTFA,

        [EnumMember(Value = "FSCROWN")]
        FSCROWN,

        [EnumMember(Value = "FSMOTI")]
        FSMOTI,

        [EnumMember(Value = "FSPRIVAT")]
        FSPRIVAT,

        [EnumMember(Value = "HWYROAD")]
        HWYROAD,

        [EnumMember(Value = "IRESERVE")]
        IRESERVE,

        [EnumMember(Value = "LNDACTR")]
        LNDACTR,

        [EnumMember(Value = "LEASELIC")]
        LEASELIC,

        [EnumMember(Value = "NSRWBCTFA")]
        NSRWBCTFA,

        [EnumMember(Value = "NSRWMOTI")]
        NSRWMOTI,

        [EnumMember(Value = "SPECUPMT")]
        SPECUPMT,

        [EnumMember(Value = "SRWBCTFA")]
        SRWBCTFA,

        [EnumMember(Value = "SRWMOTI")]
        SRWMOTI,

        [EnumMember(Value = "SRWOTHER")]
        SRWOTHER,

        [EnumMember(Value = "UNCRWNLMD")]
        UNCRWNLMD,

        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,
    }
}
