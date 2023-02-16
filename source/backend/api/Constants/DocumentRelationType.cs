using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DocumentRelationType
    {
        [EnumMember(Value = "activities")]
        Activities,
        [EnumMember(Value = "templates")]
        Templates,
        [EnumMember(Value = "researchfiles")]
        ResearchFiles,
        [EnumMember(Value = "acquisitionfiles")]
        AcquisitionFiles,
        [EnumMember(Value = "researchfileactivities")]
        ResearchFileActivities,
        [EnumMember(Value = "acquisitionfileactivities")]
        AcquisitionFileActivities,
        [EnumMember(Value = "leases")]
        Leases,
    }
}
