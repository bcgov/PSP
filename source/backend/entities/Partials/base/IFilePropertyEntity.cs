using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// An interface for entities that have an associated PimsProperty (ie - LeaseProperty, ResearchProperty, AcquisitionProperty, DispositionProperty, etc).
    /// </summary>
    public interface IFilePropertyEntity
    {
        /// <summary>
        /// get/set - The property entity for this relationship.
        /// </summary>
        PimsProperty Property { get; set; }

        /// <summary>
        /// get/set - The property id.
        /// </summary>
        long PropertyId { get; set; }

        /// <summary>
        /// Geo-spatial location (pin) of property.
        /// </summary>
        public Geometry Location { get; set; }

        /// <summary>
        /// Spatial boundary of property. Supports upload of custom shape file by user.
        /// </summary>
        public Geometry Boundary { get; set; }
    }
}
