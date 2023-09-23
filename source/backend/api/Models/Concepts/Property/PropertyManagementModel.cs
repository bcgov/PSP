using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// PropertyManagementModel class, provides a model to represent the property management information.
    /// </summary>
    public class PropertyManagementModel : BaseAppModel
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
        /// get/set - Whether utilities are payable for this property..
        /// </summary>
        public bool? IsUtilitiesPayable { get; set; }

        /// <summary>
        /// get/set - Whether taxes are payable for this property.
        /// </summary>
        public bool? IsTaxesPayable { get; set; }

        /// <summary>
        /// get/set - Whether this property has an "active lease".
        /// </summary>
        public bool IsLeaseActive { get; set; }

        /// <summary>
        /// get/set - Whether this property has an "active lease" that is expired.
        /// </summary>
        public bool IsLeaseExpired { get; set; }

        /// <summary>
        /// get/set - The expiry date on the active lease for this property (if any).
        /// </summary>
        public string LeaseExpiryDate { get; set; }
    }
}
