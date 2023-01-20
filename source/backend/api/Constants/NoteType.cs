using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum NoteType
    {
        [EnumMember(Value = "activity")]
        Activity,
        [EnumMember(Value = "acquisition_file")]
        Acquisition_File,
    }
}
