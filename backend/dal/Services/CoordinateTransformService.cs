using NetTopologySuite.Geometries;
using SharpProj;

namespace Pims.Dal.Services
{
    public class CoordinateTransformService : ICoordinateTransformService
    {
        public Geometry TransformCoordinates(int fromSRID, int toSRID, Geometry location)
        {
            using var fromCRS = CoordinateReferenceSystem.CreateFromEpsg(fromSRID);
            using var toCRS = CoordinateReferenceSystem.CreateFromEpsg(toSRID);

            using var transform = CoordinateTransform.Create(fromCRS, toCRS);
            var result = transform.Apply(new PPoint(location.Coordinate.X, location.Coordinate.Y));

            return new Point(result.X, result.Y) { SRID = toSRID };
        }
    }
}
