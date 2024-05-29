using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.CodeTypes
{
    /// <summary>
    /// Defines the GeoJSON Objects types.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum GeoJsonTypes
    {

        [EnumMember(Value = "Point")]
        Point,

        [EnumMember(Value = "MultiPoint")]
        MultiPoint,

        [EnumMember(Value = "LineString")]
        LineString,

        [EnumMember(Value = "MultiLineString")]
        MultiLineString,

        [EnumMember(Value = "Polygon")]
        Polygon,

        [EnumMember(Value = "MultiPolygon")]
        MultiPolygon,
    }
}
