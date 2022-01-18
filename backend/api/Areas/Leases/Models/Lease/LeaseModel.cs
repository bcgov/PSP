using System;
using System.Collections.Generic;
using Pims.Api.Models;

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
        /// get/set - The location of documents related to this lease.
        /// </summary>
        public string DocumentationReference { get; set; }

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
        /// get/set - The text description if the lease category type is set to "other"
        /// </summary>
        /// <value></value>
        public string OtherCategoryType { get; set; }

        /// <summary>
        /// get/set - The text description if the lease program type is set to "other"
        /// </summary>
        /// <value></value>
        public string OtherProgramType { get; set; }

        /// <summary>
        /// get/set - The text description if the lease purpose type is set to "other"
        /// </summary>
        /// <value></value>
        public string OtherPurposeType { get; set; }

        /// <summary>
        /// get/set - The text description if the lease type is set to "other"
        /// </summary>
        /// <value></value>
        public string OtherType { get; set; }


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
        public TypeModel<string> PaymentReceivableType { get; set; }

        /// <summary>
        /// get/set - The entity that initiated this lease.
        /// </summary>
        public TypeModel<string> Type { get; set; }

        /// <summary>
        /// get/set - The entity that initiated this lease.
        /// </summary>
        public TypeModel<string> InitiatorType { get; set; }

        /// <summary>
        /// get/set - The entity responsible for this lease.
        /// </summary>
        public TypeModel<string> ResponsibilityType { get; set; }

        /// <summary>
        /// get/set - The entity responsible for this lease.
        /// </summary>
        public TypeModel<string> CategoryType { get; set; }

        /// <summary>
        /// get/set - The entity responsible for this lease.
        /// </summary>
        public TypeModel<string> PurposeType { get; set; }

        /// <summary>
        /// get/set - The status of this lease within PIMS, Draft by default.
        /// </summary>
        public TypeModel<string> StatusType { get; set; }

        /// <summary>
        /// get/set - The region of this lease within PIMS.
        /// </summary>
        public RegionModel Region { get; set; }

        /// <summary>
        /// get/set - The status of this lease within PIMS, Draft by default.
        /// </summary>
        public TypeModel<string> ProgramType { get; set; }

        /// <summary>
        /// get/set - The date this entity assumed responsibility for this lease.
        /// </summary>
        public DateTime? ResponsibilityEffectiveDate { get; set; }

        /// <summary>
        /// get/set - A list of tenant notes.
        /// </summary>
        public IEnumerable<TermModel> Terms { get; set; }

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
        /// get/set - A list of organization tenants associated with this lease
        /// </summary>
        public IEnumerable<TenantModel> Tenants { get; set; }

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
        /// get/set - A collection of Security Deposits associated to this Lease
        /// </summary>
        public IEnumerable<SecurityDepositModel> SecurityDeposits { get; set; }

        /// <summary>
        /// get/set - A collection of Security Deposit Returns associated to this Lease
        /// </summary>
        public IEnumerable<SecurityDepositReturnModel> SecurityDepositReturns { get; set; }

        /// <summary>
        /// get/set - Notes accompanying Lease.
        /// </summary>
        public string ReturnNotes { get; set; }

        /// <summary>
        /// get/set - Whether this improvement contains a building that is subject to RTA (Residential Tenancy Act).
        /// </summary>
        public bool IsResidential { get; set; }

        /// <summary>
        /// get/set - Whether this improvement contains a commercial building.
        /// </summary>
        public bool IsCommercialBuilding { get; set; }

        /// <summary>
        /// get/set - Whether this improvement is of type other.
        /// </summary>
        public bool IsOtherImprovement { get; set; }
        public bool HasPhysicalFile { get; set; }
        public bool HasDigitalFile { get; set; }
        public bool HasPhysicalLicense { get; set; }
        public bool HasDigitalLicense { get; set; }
        public bool IsExpired { get; set; }
        #endregion
    }
}
