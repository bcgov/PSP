using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Dal.Constants
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum NoteType
    {
        [EnumMember(Value = "activity")]
        ACTIVITY,
    }
}
