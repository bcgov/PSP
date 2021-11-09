using System;
using System.Collections.Generic;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// Provides a lease-oriented model.
    /// </summary>
    public class LeaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the lease.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - The lease amount.
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// get/set - The value of the tenant name.
        /// </summary>
        /// <value></value>
        public string TenantName { get; set; }

        /// <summary>
        /// get/set - The value of the moti resource assigned to this lease.
        /// </summary>
        /// <value></value>
        public string MotiName { get; set; }

        /// <summary>
        /// get/set - The value of the program name.
        /// </summary>
        /// <value></value>
        public string ProgramName { get; set; }

        /// <summary>
        /// get/set - The lease notes.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// get/set - The lease description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The string value of the street address.
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The LIS L File #.
        /// </summary>
        /// <value></value>
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - The LIS TFA File #.
        /// </summary>
        /// <value></value>
        public string TfaFileNo { get; set; }

        /// <summary>
        /// get/set - The LIS Ps File #.
        /// </summary>
        /// <value></value>
        public string PsFileNo { get; set; }

        /// <summary>
        /// get/set - The calculated expiry date of the lease
        /// </summary>
        /// <value></value>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The original start date of the lease
        /// </summary>
        /// <value></value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// get/set - The most recent renewal date on the lease
        /// </summary>
        /// <value></value>
        public DateTime? RenewalDate { get; set; }

        /// <summary>
        /// get/set - The lease renewal count.
        /// </summary>
        public int RenewalCount { get; set; }

        /// <summary>
        /// get/set - The receivable payment type code identifier
        /// </summary>
        /// <value></value>
        public string PaymentReceivableTypeId { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease payment frequency type.
        /// </summary>
        public string PaymentFrequencyTypeId { get; set; }

        /// <summary>
        /// get/set - The lease payment frequency type.
        /// </summary>
        public string PaymentFrequencyType { get; set; }

        /// <summary>
        /// get/set - A list of tenant notes.
        /// </summary>
        public IEnumerable<String> TenantNotes { get; set; }

        /// <summary>
        /// get/set - A list of persons tenants associated with this lease
        /// </summary>
        public IEnumerable<PersonModel> Persons { get; set; }

        /// <summary>
        /// get/set - A list of organization tenants associated with this lease
        /// </summary>
        public IEnumerable<OrganizationModel> Organizations { get; set; }

        /// <summary>
        /// get/set - A list of properties associated with this lease
        /// </summary>
        public IEnumerable<PropertyModel> Properties { get; set; }

        /// <summary>
        /// get/set - A list of insurance objects associated with this lease
        /// </summary>
        public IEnumerable<InsuranceModel> Insurances { get; set; }

        /// <summary>
        /// get/set - A collection of Improvements associated to this Lease
        /// </summary>
        public IEnumerable<PropertyImprovementModel> Improvements { get; set; }

        /// <summary>
        /// get/set - Whether this improvement contains a building that is subject to RTA.
        /// </summary>
        public bool IsSubjectToRta { get; set; }

        /// <summary>
        /// get/set - Whether this improvement contains a commercial building.
        /// </summary>
        public bool IsCommBldg { get; set; }

        /// <summary>
        /// get/set - Whether this improvement is of type other.
        /// </summary>
        public bool IsOtherImprovement { get; set; }
        #endregion
    }
}
