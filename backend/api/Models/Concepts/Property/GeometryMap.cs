using Mapster;
using NetTopologySuite.Geometries;

namespace Pims.Api.Models.Concepts
{
    public class GeometryMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Geometry, GeometryModel>()
                .Map(dest => dest.Coordinate, src => src.Coordinate);

            config.NewConfig<GeometryModel, Geometry>()
                .ConstructUsing(src => new GeometryFactory().CreatePoint(new Coordinate(src.Coordinate.X, src.Coordinate.Y)));

            config.NewConfig<Coordinate, CoordinateModel>()
                .Map(dest => dest.X, src => src.X)
                .Map(dest => dest.Y, src => src.Y);
        }
    }
}
