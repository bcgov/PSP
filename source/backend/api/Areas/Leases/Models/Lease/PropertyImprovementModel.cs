namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// PropertyImprovementModel class, provides lease-oriented property improvements.
    /// </summary>
    public class PropertyImprovementModel
    {
        #region PropertyImprovement

        /// <summary>
        /// get/set - The primary key to identify the property improvement.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The id of the parent lease.
        /// </summary>
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property improvement type.
        /// </summary>
        public string PropertyImprovementTypeId { get; set; }

        /// <summary>
        /// get/set - The property improvement type.
        /// </summary>
        public string PropertyImprovementType { get; set; }

        /// <summary>
        /// get/set - The improvement description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The size of the structure of the improvement.
        /// </summary>
        public string StructureSize { get; set; }

        /// <summary>
        /// get/set - Notes related to any address within the improvement.
        /// </summary>
        public string Address { get; set; }
        #endregion
    }
}
