using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum LeasePurposeTypes
    {
        [EnumMember(Value = "ACCCCOM")]
        ACCCCOM,

        [EnumMember(Value = "ACCRES")]
        ACCRES,

        [EnumMember(Value = "ARCHEOLGY")]
        ARCHEOLGY,

        [EnumMember(Value = "BCFERRIES")]
        BCFERRIES,

        [EnumMember(Value = "BCTRANSIT")]
        BCTRANSIT,

        [EnumMember(Value = "CAMPING")]
        CAMPING,

        [EnumMember(Value = "COMMBLDG")]
        COMMBLDG,

        [EnumMember(Value = "EMERGSVCS")]
        EMERGSVCS,

        [EnumMember(Value = "ENCROACH")]
        ENCROACH,

        [EnumMember(Value = "ENVIRON")]
        ENVIRON,

        [EnumMember(Value = "FENCEGATE")]
        FENCEGATE,

        [EnumMember(Value = "GARDENING")]
        GARDENING,

        [EnumMember(Value = "GEOTECH")]
        GEOTECH,

        [EnumMember(Value = "GRAVEL")]
        GRAVEL,

        [EnumMember(Value = "GRAZING")]
        GRAZING,

        [EnumMember(Value = "HISTORY")]
        HISTORY,

        [EnumMember(Value = "HOUSING")]
        HOUSING,

        [EnumMember(Value = "LNDSCPVEG")]
        LNDSCPVEG,

        [EnumMember(Value = "LNDSRVY")]
        LNDSRVY,

        [EnumMember(Value = "MARINEFAC")]
        MARINEFAC,

        [EnumMember(Value = "MOBILEHM")]
        MOBILEHM,

        [EnumMember(Value = "MTCYARD")]
        MTCYARD,

        [EnumMember(Value = "OTHER")]
        OTHER,

        [EnumMember(Value = "PARK")]
        PARK,

        [EnumMember(Value = "PARKING")]
        PARKING,

        [EnumMember(Value = "PARKNRID")]
        PARKNRID,

        [EnumMember(Value = "PIPELINE")]
        PIPELINE,

        [EnumMember(Value = "PRELOAD")]
        PRELOAD,

        [EnumMember(Value = "PRVTRANS")]
        PRVTRANS,

        [EnumMember(Value = "RAILWAY")]
        RAILWAY,

        [EnumMember(Value = "REMEDIAL")]
        REMEDIAL,

        [EnumMember(Value = "RESTAREA")]
        RESTAREA,

        [EnumMember(Value = "RIPARIAN")]
        RIPARIAN,

        [EnumMember(Value = "RLWYTRSPS")]
        RLWYTRSPS,

        [EnumMember(Value = "SIDEWALK")]
        SIDEWALK,

        [EnumMember(Value = "SIGNAGE")]
        SIGNAGE,

        [EnumMember(Value = "SPCLEVNT")]
        SPCLEVNT,

        [EnumMember(Value = "STGNGAREA")]
        STGNGAREA,

        [EnumMember(Value = "STKPILING")]
        STKPILING,

        [EnumMember(Value = "STORAGE")]
        STORAGE,

        [EnumMember(Value = "TOURINFO")]
        TOURINFO,

        [EnumMember(Value = "TRAIL")]
        TRAIL,

        [EnumMember(Value = "TRANSITO")]
        TRANSITO,

        [EnumMember(Value = "TRANSLINK")]
        TRANSLINK,

        [EnumMember(Value = "UTILINFRA")]
        UTILINFRA,

        [EnumMember(Value = "UTILOHDXG")]
        UTILOHDXG,

        [EnumMember(Value = "UTILUGDXG")]
        UTILUGDXG,

        [EnumMember(Value = "WATERRES")]
        WATERRES,

        [EnumMember(Value = "WEIGHSCL")]
        WEIGHSCL,

        [EnumMember(Value = "XING")]
        XING,
    }
}
