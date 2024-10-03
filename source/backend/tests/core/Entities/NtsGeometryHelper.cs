using NetTopologySuite.Geometries;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Creates a new instance of a Polygon.
        /// </summary>
        /// <param name="spatialReferenceId">The target spatial reference Id (4396 = lat/lon), (3005 = BC ALBERS).</param>
        /// <returns>A polygon geometry instance.</returns>
        public static Polygon CreatePolygon(int spatialReferenceId = 4396)
        {
            return CreatePolygon(
                new[]
                {
                    new Coordinate(-100, 45),
                    new Coordinate(-98, 45),
                    new Coordinate(-99, 46),
                    new Coordinate(-100, 45),
                },
                spatialReferenceId);
        }

        /// <summary>
        /// Creates a new instance of a Polygon.
        /// </summary>
        /// <param name="coordinates">An array without null elements, or an empty array, or null.</param>
        /// <param name="spatialReferenceId">The target spatial reference Id (4396 = lat/lon), (3005 = BC ALBERS).</param>
        /// <returns>A polygon geometry instance.</returns>
        public static Polygon CreatePolygon(Coordinate[] coordinates, int spatialReferenceId = 4396)
        {
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(spatialReferenceId);
            return gf.CreatePolygon(gf.CreateLinearRing(coordinates));
        }

        /// <summary>
        /// Creates a geometric point object for the specified 'longitude' and 'latitude'.
        /// </summary>
        /// <param name="longitude">The x coordinate.</param>
        /// <param name="latitude">The y coordinate.</param>
        /// <param name="spatialReferenceId">Spatial Reference Identifier (SRID) is a unique identifier associated with a specific coordinate system, tolerance, and resolution (default 4326).</param>
        /// <returns>A NetTopologySuite.Geometries.Point object.</returns>
        public static Point CreatePoint(double longitude, double latitude, int spatialReferenceId)
        {
            return CreatePoint(new Coordinate(longitude, latitude), spatialReferenceId);
        }

        /// <summary>
        /// Creates a Point using the given Coordinate. A null coordinate creates an empty Geometry.
        /// </summary>
        /// <param name="coordinate">a Coordinate, or null.</param>
        /// <param name="spatialReferenceId">Spatial Reference Identifier (SRID) is a unique identifier associated with a specific coordinate system, tolerance, and resolution (default 4326).</param>
        /// <returns>A NetTopologySuite.Geometries.Point object.</returns>
        public static Point CreatePoint(Coordinate coordinate, int spatialReferenceId)
        {
            // Spatial Reference Identifier (SRID) is a unique identifier associated with a specific coordinate system, tolerance, and resolution (default 4326)
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(spatialReferenceId);
            return gf.CreatePoint(coordinate);
        }
    }
}
