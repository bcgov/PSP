using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DocumentRelationType
    {
        [EnumMember(Value = "Templates")]
        Templates,
        [EnumMember(Value = "ResearchFiles")]
        ResearchFiles,
        [EnumMember(Value = "AcquisitionFiles")]
        AcquisitionFiles,
        [EnumMember(Value = "Leases")]
        Leases,
        [EnumMember(Value = "Projects")]
        Projects,
        [EnumMember(Value = "ManagementFiles")]
        ManagementFiles,
        [EnumMember(Value = "DispositionFiles")]
        DispositionFiles,
    }
}
