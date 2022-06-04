using NetTopologySuite.Geometries;

namespace Pims.Dal.Services
{
    public interface ICoordinateTransformService
    {
        Geometry TransformCoordinates(int fromSRID, int toSRID, Geometry location);
    }
}
