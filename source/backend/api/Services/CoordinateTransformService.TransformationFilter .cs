using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems.Transformations;

namespace Pims.Api.Services
{
    /// <summary>
    /// CoordinateTransformService implementation provides capabilities to transform geographic coordinates between various coordinate systems.
    /// </summary>
    public partial class CoordinateTransformService : ICoordinateTransformService
    {
        /// <summary>
        /// Internal (nested) class that is only meant to be used by the Coordinate Transform Service.
        ///
        /// This filter class applies a <see cref="ProjNet.CoordinateSystems.Transformations.MathTransform"/>
        /// to a <see cref="NetTopologySuite.Geometries.Geometry"/>.
        /// </summary>
        private class TransformationFilter : NetTopologySuite.Geometries.IEntireCoordinateSequenceFilter
        {
            private readonly MathTransform _mt;

            /// <summary>
            /// Reports whether the application of this filter can be terminated.
            /// </summary>
            public bool Done => false;

            /// <summary>
            /// Reports whether the execution of this filter has modified the coordinates of the geometry.
            /// If so, <see cref="Geometry.GeometryChanged()"/> will be executed
            /// after this filter has finished being executed.
            /// </summary>
            public bool GeometryChanged => true;

            /// <summary>
            /// Creates an instance of this class using the provided <see cref="ProjNet.CoordinateSystems.Transformations.MathTransform"/>.
            /// </summary>
            /// <param name="mt">A math transform object.</param>
            public TransformationFilter(MathTransform mt)
            {
                _mt = mt;
            }

            public void Filter(CoordinateSequence seq)
            {
                if (seq == null || seq.Count == 0)
                {
                    return;
                }

                // Perform in-place coordinate transformation
                for (int i = 0; i < seq.Count; i++)
                {
                    // Get coordinate values
                    var x = seq.GetX(i);
                    var y = seq.GetY(i);

                    (var projectedX, var projectedY) = _mt.Transform(x, y);

                    // Set coordinate values back on the original geometry
                    seq.SetX(i, projectedX);
                    seq.SetY(i, projectedY);
                }
            }
        }
    }
}
