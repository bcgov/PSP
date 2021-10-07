using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyLease class, provides an entity for the datamodel to manage lease properties.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_LEASE", "PROPLS")]
    public class PropertyLease : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the property lease.
        /// </summary>
        [Column("PROPERTY_LEASE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property.
        /// </summary>
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - The property.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease.
        /// </summary>
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The lease this expected amount is linked to.
        /// </summary>
        public Lease Lease { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyLease class.
        /// </summary>
        public PropertyLease() { }

        /// <summary>
        /// Create a new instance of a PropertyLease class.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="lease"></param>
        public PropertyLease(Property property, Lease lease)
        {
            this.PropertyId = property?.Id ?? throw new ArgumentNullException(nameof(property));
            this.Property = property;
            this.LeaseId = lease?.Id ?? throw new ArgumentNullException(nameof(lease));
            this.Lease = lease;
        }
        #endregion
    }
}
