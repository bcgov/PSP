namespace Pims.Dal.Entities
{
    /// <summary>
    /// An interface for entities that have an associated PimsProperty (ie - LeaseProperty, ResearchProperty, AcquisitionProperty, DispositionProperty, etc).
    /// </summary>
    public interface IWithPropertyEntity
    {
        /// <summary>
        /// get/set - The property entity for this relationship.
        /// </summary>
        PimsProperty Property { get; set; }

        /// <summary>
        /// get/set - The property id.
        /// </summary>
        long PropertyId { get; set; }
    }
}
