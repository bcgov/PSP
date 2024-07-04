using Mapster;
using NetTopologySuite.Geometries;
using Pims.Api.Models.CodeTypes;
using Pims.Dal.Helpers;

namespace Pims.Api.Models.Concepts.Property
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

            // This mapping is needed to copy the NTS Geometry instance rather than deep copy the object.
            // The NTS geometry is deserialized automatically from GeoJSON by the NetTopologySuite.IO.GeoJSON4STJ library
            // see: https://github.com/MapsterMapper/Mapster/wiki/Custom-conversion-logic
            config.NewConfig<Geometry, Geometry>()
               .MapWith(geom => geom);
        }
    }
}
