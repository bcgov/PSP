using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyImprovement class, provides an entity for the datamodel to manage property improvements.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_IMPROVEMENT", "PIMPRV")]
    public class PropertyImprovement : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the property lease.
        /// </summary>
        [Column("PROPERTY_IMPROVEMENT_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease.
        /// </summary>
        [Column("PROPERTY_LEASE_ID")]
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The lease this property improvement is linked to.
        /// </summary>
        public Lease Lease { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property improvement type.
        /// </summary>
        [Column("PROPERTY_IMPROVEMENT_TYPE_CODE")]
        public string PropertyImprovementTypeId { get; set; }

        /// <summary>
        /// get/set - The property improvement type.
        /// </summary>
        public PropertyImprovementType PropertyImprovementType { get; set; }

        /// <summary>
        /// get/set - The improvement description.
        /// </summary>
        [Column("IMPROVEMENT_DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The size of the structure of the improvement.
        /// </summary>
        [Column("STRUCTURE_SIZE")]
        public string StructureSize { get; set; }

        /// <summary>
        /// get/set - Notes related to any units within the improvement
        /// </summary>
        [Column("UNIT")]
        public string Unit { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyImprovement class.
        /// </summary>
        public PropertyImprovement() { }

        /// <summary>
        /// Create a new instance of a PropertyImprovement class.
        /// </summary>
        /// <param name="propertyImprovementType"></param>
        public PropertyImprovement(PropertyImprovementType propertyImprovementType)
        {
            this.PropertyImprovementType = propertyImprovementType;
        }
        #endregion
    }
}
