using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
{
    /// <summary>
    /// PropertyManagementModel class, provides a model to represent the property management information.
    /// </summary>
    public class PropertyManagementModel : BaseAuditModel
    {
        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The property management purposes.
        /// </summary>
        public IList<PropertyManagementPurposeModel> ManagementPurposes { get; set; }

        /// <summary>
        /// get/set - Additional details when property management purpose is OTHER.
        /// </summary>
        public string AdditionalDetails { get; set; }

        /// <summary>
        /// get/set - Whether utilities are payable for this property.
        /// </summary>
        public bool? IsUtilitiesPayable { get; set; }

        /// <summary>
        /// get/set - Whether taxes are payable for this property.
        /// </summary>
        public bool? IsTaxesPayable { get; set; }

        /// <summary>
        /// get/set - The number of leases for this property. Returns 0 when there are no leases.
        /// </summary>
        public long RelatedLeases { get; set; }

        /// <summary>
        /// get/set - The expiry date of the lease when there is only ONE lease for this property (regardless of status).
        /// This field is null when the lease does not expire.
        /// </summary>
        public DateOnly? LeaseExpiryDate { get; set; }

        /// <summary>
        /// The property has at least one active lease.
        /// </summary>
        public bool? HasActiveLease { get; set; }

        /// <summary>
        /// The active lease has expiry date.
        /// </summary>
        public bool? ActiveLeaseHasExpiryDate { get; set; }
    }
}
