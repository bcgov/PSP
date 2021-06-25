namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyTypes enum, provides the valid property types that can be used.
    /// </summary>
    public enum PropertyTypes : long
    {
        /// <summary>
        /// Represents a parcel of land.
        /// </summary>
        Land = 1,
        /// <summary>
        /// Represents a building on land.
        /// </summary>
        Building = 2,
        /// <summary>
        /// Represents a subdivision draft parcel.
        /// </summary>
        Subdivision = 3
    }
}
