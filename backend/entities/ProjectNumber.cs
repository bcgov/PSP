namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectNumber class, provides an entity for the datamodel to fetch project number sequence values.
    /// </summary>
    public class ProjectNumber
    {
        #region Properties
        /// <summary>
        /// get/set - The Value of the next number in the sequence.
        /// </summary>
        public int Value { get; set; }
        #endregion
    }
}
