namespace Pims.Geocoder.Parameters
{
    /// <summary>
    /// NearestParameters class, used with the geocoder /nearest endpoint.
    /// </summary>
    public class NearestParameters : BaseParameters
    {
        /// <summary>
        /// get/set - The point (x,y) from which the nearby sites will be identified. The coordinates must be specified in the same SRS as given by the 'outputSRS' parameter.
        /// example": "-122.377,50.121".
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// get/set - If true, excludes sites that are units of a parent site.
        /// </summary>
        public bool ExcludeUnits { get; set; }

        /// <summary>
        /// get/set - If true, excludes sites without a civic address.
        /// </summary>
        public bool OnlyCivic { get; set; }
    }
}
