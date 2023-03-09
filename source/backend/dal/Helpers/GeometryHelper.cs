using Pims.Dal.Constants;

namespace Pims.Dal.Helpers
{
    /// <summary>
    /// GeometryHelper static class, provides methods to help with geometric shapes.
    /// </summary>
    public static class GeometryHelper
    {
        /// <summary>
        /// Create a geometric point object for the specified 'longitude' and 'latitude'.
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static NetTopologySuite.Geometries.Point CreatePoint(double longitude, double latitude)
        {
            return CreatePoint(longitude, latitude, SpatialReference.WGS84);
        }

        /// <summary>
        /// Create a geometric point object for the specified 'longitude' and 'latitude'.
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="srid"></param>
        /// <returns></returns>
        public static NetTopologySuite.Geometries.Point CreatePoint(double longitude, double latitude, int srid)
        {
            // Spatial Reference Identifier (SRID) is a unique identifier associated with a specific coordinate system, tolerance, and resolution (default 4326).
            return new NetTopologySuite.Geometries.Point(longitude, latitude) { SRID = srid };
        }

        /// <summary>
        /// Create a geometric point object for the specified 'coordinate' and 'spatial reference id (4326)'.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static NetTopologySuite.Geometries.Point CreatePoint(NetTopologySuite.Geometries.Coordinate coordinate)
        {
            return CreatePoint(coordinate, SpatialReference.WGS84);
        }

        /// <summary>
        /// Create a geometric point object for the specified 'coordinate' and 'spatial reference id (srid)'.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="srid"></param>
        /// <returns></returns>
        public static NetTopologySuite.Geometries.Point CreatePoint(NetTopologySuite.Geometries.Coordinate coordinate, int srid)
        {
            // Spatial Reference Identifier (SRID) is a unique identifier associated with a specific coordinate system, tolerance, and resolution (default 4326).
            return new NetTopologySuite.Geometries.Point(coordinate) { SRID = srid };
        }
    }
}
