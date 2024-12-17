using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DataSourceTypes
    {
        [EnumMember(Value = "BIP")]
        BIP,

        [EnumMember(Value = "GAZ")]
        GAZ,

        [EnumMember(Value = "GWP")]
        GWP,

        [EnumMember(Value = "LIS")]
        LIS,

        [EnumMember(Value = "LIS_OPSS")]
        LIS_OPSS,

        [EnumMember(Value = "LIS_OPSS_PAIMS")]
        LIS_OPSS_PAIMS,

        [EnumMember(Value = "LIS_OPSS_PAIMS_PMBC")]
        LIS_OPSS_PAIMS_PMBC,

        [EnumMember(Value = "LIS_PAIMS")]
        LIS_PAIMS,

        [EnumMember(Value = "LIS_PAIMS_PMBC")]
        LIS_PAIMS_PMBC,

        [EnumMember(Value = "LIS_PMBC")]
        LIS_PMBC,

        [EnumMember(Value = "OPSS")]
        OPSS,

        [EnumMember(Value = "OPSS_PAIMS")]
        OPSS_PAIMS,

        [EnumMember(Value = "PAIMS")]
        PAIMS,

        [EnumMember(Value = "PAIMS_PMBC")]
        PAIMS_PMBC,

        [EnumMember(Value = "PAT")]
        PAT,

        [EnumMember(Value = "PIMS")]
        PIMS,

        [EnumMember(Value = "PMBC")]
        PMBC,

        [EnumMember(Value = "SHAREPOINT")]
        SHAREPOINT,

        [EnumMember(Value = "TAP")]
        TAP,
    }
}
