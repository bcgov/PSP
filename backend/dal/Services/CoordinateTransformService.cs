using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Collections.Generic;
using System;

namespace Pims.Dal.Services
{
    public class CoordinateTransformService : ICoordinateTransformService
    {
        protected readonly CoordinateSystemFactory CoordinateSystemFactory = new CoordinateSystemFactory();
        protected readonly CoordinateTransformationFactory CoordinateTransformationFactory = new CoordinateTransformationFactory();

        private readonly Dictionary<int, string> _projections = new Dictionary<int, string> {
            { 3005, @"PROJCS[""NAD83 / BC Albers"",GEOGCS[""NAD83"",DATUM[""North_American_Datum_1983"",SPHEROID[""GRS 1980"",6378137,298.257222101,AUTHORITY[""EPSG"",""7019""]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[""EPSG"",""6269""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4269""]],PROJECTION[""Albers_Conic_Equal_Area""],PARAMETER[""standard_parallel_1"",50],PARAMETER[""standard_parallel_2"",58.5],PARAMETER[""latitude_of_center"",45],PARAMETER[""longitude_of_center"",-126],PARAMETER[""false_easting"",1000000],PARAMETER[""false_northing"",0],UNIT[""metre"",1,AUTHORITY[""EPSG"",""9001""]],AXIS[""Easting"",EAST],AXIS[""Northing"",NORTH],AUTHORITY[""EPSG"",""3005""]]"},
            { 4326, @"GEOGCS[""WGS 84"",DATUM[""WGS_1984"",SPHEROID[""WGS 84"",6378137,298.257223563,AUTHORITY[""EPSG"",""7030""]],AUTHORITY[""EPSG"",""6326""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4326""]]GEOGCS[""WGS 84"",DATUM[""WGS_1984"",SPHEROID[""WGS 84"",6378137,298.257223563,AUTHORITY[""EPSG"",""7030""]],AUTHORITY[""EPSG"",""6326""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4326""]]" }
        };

        public Geometry TransformCoordinates(int fromSRID, int toSRID, Geometry location)
        {
            if (!_projections.ContainsKey(fromSRID))
            {
                throw new InvalidOperationException($"Spatial Reference {fromSRID} not supported");
            }
            if (!_projections.ContainsKey(toSRID))
            {
                throw new InvalidOperationException($"Spatial Reference {toSRID} not supported");
            }

            var sourceCS = CoordinateSystemFactory.CreateFromWkt(_projections[fromSRID]);
            var targetCS = CoordinateSystemFactory.CreateFromWkt(_projections[toSRID]);

            var ct = CoordinateTransformationFactory.CreateFromCoordinateSystems(sourceCS, targetCS);
            (var projectedX, var projectedY) = ct.MathTransform.Transform(location.Coordinate.X, location.Coordinate.Y);

            return new Point(projectedX, projectedY) { SRID = toSRID };
        }
    }
}
