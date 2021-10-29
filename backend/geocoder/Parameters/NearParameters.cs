namespace Pims.Geocoder.Parameters
{
    /// <summary>
    /// NearestParameters class, used with the geocoder /near endpoint.
    /// </summary>
    public class NearParameters : NearestParameters
    {
        /// <summary>
        /// get/set - The maximum number of search results to return.
        /// </summary>
        public int MaxResults { get; set; } = 5;
    }
}
