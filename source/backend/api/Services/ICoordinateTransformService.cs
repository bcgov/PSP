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
        /// <returns></returns>
        Coordinate TransformCoordinates(int sourceSrid, int targetSrid, Coordinate location);
    }
}
