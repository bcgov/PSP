using Mapster;
using NetTopologySuite.Geometries;
using Pims.Dal.Constants;
using Pims.Dal.Helpers;

namespace Pims.Api.Models.Concepts
{
    public class GeometryMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Geometry, GeometryModel>()
                .Map(dest => dest.Coordinate, src => src.Coordinate);

            config.NewConfig<GeometryModel, Geometry>()
                .ConstructUsing(src => GeometryHelper.CreatePoint(src.Coordinate.X, src.Coordinate.Y, SpatialReference.WGS84));

            config.NewConfig<Coordinate, CoordinateModel>()
                .Map(dest => dest.X, src => src.X)
                .Map(dest => dest.Y, src => src.Y);
        }
    }
}
