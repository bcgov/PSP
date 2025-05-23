using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum NoteType
    {
        [EnumMember(Value = "acquisition_file")]
        Acquisition_File,
        [EnumMember(Value = "lease_file")]
        Lease_File,
        [EnumMember(Value = "project")]
        Project,
        [EnumMember(Value = "research_file")]
        Research_File,
        [EnumMember(Value = "disposition_file")]
        Disposition_File,
        [EnumMember(Value = "management_file")]
        Management_File,
        [EnumMember(Value = "property")]
        Property,
    }
}
