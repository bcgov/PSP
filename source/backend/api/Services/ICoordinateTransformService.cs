using NetTopologySuite.Geometries;

namespace Pims.Api.Services
{
    public interface ICoordinateTransformService
    {
        /// <summary>
        /// Determines whether the supplied coordinate system is supported.
        /// </summary>
        /// <param name="srid">The identifier for the spatial reference system.</param>
        /// <returns>true if supported; false otherwise.</returns>
        bool IsCoordinateSystemSupported(int srid);

        /// <summary>
        /// Transforms (re-projects) coordinates between two spatial reference systems, defined by their identifiers.
        /// </summary>
        /// <param name="sourceSrid">The identifier for the source spatial reference system.</param>
        /// <param name="targetSrid">The identifier for the target spatial reference system.</param>
        /// <param name="location">The coordinates to re-project.</param>
        /// <returns>A new Coordinate object with the transformed values.</returns>
        Coordinate TransformCoordinates(int sourceSrid, int targetSrid, Coordinate location);

        /// <summary>
        /// Transforms (re-projects) the supplied Geometry between two spatial reference systems, defined by their identifiers.
        /// This method is used to perform in-place coordinate transformation for Polygons and MultiPolygons.
        /// </summary>
        /// <param name="sourceSrid">The identifier for the source spatial reference system.</param>
        /// <param name="targetSrid">The identifier for the target spatial reference system.</param>
        /// <param name="geometry">The geometry to re-project.</param>
        void TransformGeometry(int sourceSrid, int targetSrid, Geometry geometry);
    }
}
