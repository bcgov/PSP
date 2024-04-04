using System.Linq;
using Mapster;
using NetTopologySuite.Geometries;
using Pims.Api.Models.CodeTypes;
using Pims.Dal.Helpers;

namespace Pims.Api.Models.Concepts.Property
{
    public class PolygonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Geometry, PolygonModel>()
                .Map(dest => dest.Coordinates, src => src.Coordinates);

            config.NewConfig<PolygonModel, Geometry>()
                .ConstructUsing(src => src != null ? NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326)
                .CreatePolygon(src.Coordinates.Select(coordinate => GeometryHelper.CreatePoint(coordinate.X, coordinate.Y, SpatialReference.WGS84).Coordinate).ToArray()) : null);

            config.NewConfig<Coordinate[], PolygonModel>()
                .Map(dest => dest.Coordinates, src => src);
        }
    }
}
