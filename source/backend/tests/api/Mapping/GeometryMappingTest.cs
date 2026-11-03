using MapsterMapper;
using NetTopologySuite.Geometries;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Test;
using Xunit;

namespace Pims.Api.Test.Mapping
{
    public class GeometryMappingTest
    {
        [Fact]
        public void GeometryToGeometry_PreservesInstance()
        {
            // Ensure the model assembly is loaded before TestHelper scans it.
            _ = typeof(GeometryMap).Assembly.GetTypes();

            var helper = new TestHelper();
            var mapper = helper.GetService<IMapper>();

            Geometry source = new Point(-123.1, 49.2)
            {
                SRID = 4326,
            };

            var result = mapper.Map<Geometry>(source);

            Assert.Same(source, result);
            Assert.Equal(4326, result.SRID);
        }

        [Fact]
        public void CoordinateMapping_PreservesCoordinates()
        {
            _ = typeof(GeometryMap).Assembly.GetTypes();

            var helper = new TestHelper();
            var mapper = helper.GetService<IMapper>();

            var source = new Coordinate(-123.1, 49.2);

            var result = mapper.Map<CoordinateModel>(source);

            Assert.NotNull(result);
            Assert.Equal(source.X, result.X);
            Assert.Equal(source.Y, result.Y);
        }
    }
}