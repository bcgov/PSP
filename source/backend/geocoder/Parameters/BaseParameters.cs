namespace Pims.Geocoder.Parameters
{
    /// <summary>
    /// BaseParameters class, shared request parameters used by /site endpoints within geocoder.
    /// </summary>
    public abstract class BaseParameters
    {
        /// <summary>
        /// get/set - The EPSG code of the spatial reference system used to state the coordination location of a named feature. It is ignored if KML output is specified since KML only supports 4326 (WGS84). Allowed values are:
        /// 3005: BC Albers
        /// 4326: WGS 84 (default)
        /// 26907-26911: NAD83/UTM Zones 7N through 11N
        /// 32607-32611: WGS84/UTM Zones 7N through 11N
        /// 26707-26711: NAD27/UTM Zones 7N through 11N.
        /// </summary>
        public int OutputSRS { get; set; } = 4326;

        /// <summary>
        /// get/set - Describes the nature of the address location. Values include accessPoint, frontDoorPoint, parcelPoint, rooftopPoint, and routingPoint. As an input parameter, a value of any is allowed. When any is specified, a point type other than accessPoint will be returned if one is defined; otherwise, an accessPoint will be returned.
        /// </summary>
        public string LocationDescriptor { get; set; } = "any";

        /// <summary>
        /// get/set - The distance to move the accessPoint away from the curb and towards the inside of the parcel (in metres). Ignored if locationDescriptor not set to accessPoint.
        /// </summary>
        public int SetBack { get; set; }

        /// <summary>
        /// get/set - The maximum distance (in metres) to search from the given point. If not specified, the search distance is unlimited.
        /// </summary>
        public virtual double? MaxDistance { get; set; }

        /// <summary>
        /// get/set - If true, include only basic match and address details in results. Not supported for shp, csv, and gml formats.
        /// </summary>
        public bool Brief { get; set; }
    }
}
