using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace Pims.Api.Services
{
    public class CoordinateTransformService : ICoordinateTransformService
    {
        private readonly CoordinateSystemFactory _coordinateSystemFactory = new CoordinateSystemFactory();
        private readonly CoordinateTransformationFactory _coordinateTransformationFactory = new CoordinateTransformationFactory();
        private readonly Dictionary<int, CoordinateSystem> _projections = new Dictionary<int, CoordinateSystem>();

        private static IEnumerable<KeyValuePair<int, string>> DefaultProjections()
        {
            yield return new KeyValuePair<int, string>(4326, @"GEOGCS[""WGS 84"",DATUM[""WGS_1984"",SPHEROID[""WGS 84"",6378137,298.257223563,AUTHORITY[""EPSG"",""7030""]],AUTHORITY[""EPSG"",""6326""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4326""]]GEOGCS[""WGS 84"",DATUM[""WGS_1984"",SPHEROID[""WGS 84"",6378137,298.257223563,AUTHORITY[""EPSG"",""7030""]],AUTHORITY[""EPSG"",""6326""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4326""]]");
            yield return new KeyValuePair<int, string>(3005, @"PROJCS[""NAD83 / BC Albers"",GEOGCS[""NAD83"",DATUM[""North_American_Datum_1983"",SPHEROID[""GRS 1980"",6378137,298.257222101,AUTHORITY[""EPSG"",""7019""]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[""EPSG"",""6269""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4269""]],PROJECTION[""Albers_Conic_Equal_Area""],PARAMETER[""standard_parallel_1"",50],PARAMETER[""standard_parallel_2"",58.5],PARAMETER[""latitude_of_center"",45],PARAMETER[""longitude_of_center"",-126],PARAMETER[""false_easting"",1000000],PARAMETER[""false_northing"",0],UNIT[""metre"",1,AUTHORITY[""EPSG"",""9001""]],AXIS[""Easting"",EAST],AXIS[""Northing"",NORTH],AUTHORITY[""EPSG"",""3005""]]");
        }

        private CoordinateSystem CreateCoordinateSystem(string wkt)
        {
            try
            {
                return _coordinateSystemFactory.CreateFromWkt(wkt);
            }
            catch (Exception)
            {
                // as a fallback we ignore projections not supported
                return null;
            }
        }

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        public CoordinateTransformService()
        {
            var enumeration = DefaultProjections();
            foreach (var sridWkt in enumeration)
            {
                var cs = CreateCoordinateSystem(sridWkt.Value);
                if (cs != null)
                {
                    _projections.Add(sridWkt.Key, cs);
                }
            }
        }

        /// <inheritdoc />
        public bool IsCoordinateSystemSupported(int srid)
        {
            return _projections.ContainsKey(srid);
        }

        /// <inheritdoc />
        public Coordinate TransformCoordinates(int sourceSrid, int targetSrid, Coordinate location)
        {
            if (!IsCoordinateSystemSupported(sourceSrid))
            {
                throw new InvalidOperationException($"Spatial Reference {sourceSrid} not supported");
            }
            if (!IsCoordinateSystemSupported(targetSrid))
            {
                throw new InvalidOperationException($"Spatial Reference {targetSrid} not supported");
            }

            var source = _projections[sourceSrid];
            var target = _projections[targetSrid];

            var ct = _coordinateTransformationFactory.CreateFromCoordinateSystems(source, target);
            (var projectedX, var projectedY) = ct.MathTransform.Transform(location.X, location.Y);

            return new Coordinate(projectedX, projectedY);
        }
    }
}
