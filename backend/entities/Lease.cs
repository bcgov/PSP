using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Lease class, provides an entity for the datamodel to manage leases.
    /// </summary>
    [MotiTable("PIMS_LEASE", "LEASE")]
    public class Lease : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the lease.
        /// </summary>
        [Column("LEASE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - string description of the lease purpose if type code is other.
        /// </summary>
        [Column("LEASE_PURPOSE_OTHER_DESC")]
        public string LeasePurposeOtherDesc { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease purpose type.
        /// </summary>
        [Column("LEASE_PURPOSE_TYPE_CODE")]
        public string PurposeTypeId { get; set; }

        /// <summary>
        /// get/set - The lease purpose type.
        /// </summary>
        public LeasePurposeType PurposeType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease status type.
        /// </summary>
        [Column("LEASE_STATUS_TYPE_CODE")]
        public string StatusTypeId { get; set; }

        /// <summary>
        /// get/set - The lease status type.
        /// </summary>
        public LeaseStatusType StatusType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease payment frequency type.
        /// </summary>
        [Column("LEASE_PMT_FREQ_TYPE_CODE")]
        public string PaymentFrequencyTypeId { get; set; }

        /// <summary>
        /// get/set - The lease payment frequency type.
        /// </summary>
        public LeasePaymentFrequencyType PaymentFrequencyType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease payment rvbl type.
        /// </summary>
        [Column("LEASE_PAY_RVBL_TYPE_CODE")]
        public string PaymentReceivableTypeId { get; set; }

        /// <summary>
        /// get/set - The lease payment rvbl type.
        /// </summary>
        public LeasePaymentReceivableType PaymentRvblType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease program type.
        /// </summary>
        [Column("LEASE_PROGRAM_TYPE_CODE")]
        public string ProgramTypeId { get; set; }

        /// <summary>
        /// get/set - The lease program type.
        /// </summary>
        public LeaseProgramType ProgramType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease category type.
        /// </summary>
        [Column("LEASE_CATEGORY_TYPE_CODE")]
        public string CategoryTypeId { get; set; }

        /// <summary>
        /// get/set - The lease category type.
        /// </summary>
        public LeaseCategoryType CategoryType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease responsibility type.
        /// </summary>
        [Column("LEASE_RESPONSIBILITY_TYPE_CODE")]
        public string LeaseResponsibilityTypeId { get; set; }

        /// <summary>
        /// get/set - The lease responsibility type.
        /// </summary>
        public LeaseResponsibilityType LeaseResponsibilityType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease initiator type.
        /// </summary>
        [Column("LEASE_INITIATOR_TYPE_CODE")]
        public string LeaseInitiatorTypeId { get; set; }

        /// <summary>
        /// get/set - The lease initiator type.
        /// </summary>
        public LeaseIntiatorType LeaseInitiatorType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease type.
        /// </summary>
        [Column("LEASE_LICENSE_TYPE_CODE")]
        public string LeaseTypeId { get; set; }

        /// <summary>
        /// get/set - The lease type.
        /// </summary>
        public LeaseLicenseType LeaseLicenseType { get; set; }

        /// <summary>
        /// get/set - The MOTI resource associated with this lease.
        /// </summary>
        [Column("MOTI_NAME_ID")]
        public long MotiNameId { get; set; }

        /// <summary>
        /// get/set - The MOTI resource associated with this lease.
        /// </summary>
        public Person MotiName { get; set; }

        /// <summary>
        /// get/set - The lease LIS file number.
        /// </summary>
        [Column("L_FILE_NO")]
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - The lease TFA file number.
        /// </summary>
        [Column("TFA_FILE_NO")]
        public int? TfaFileNo { get; set; }

        /// <summary>
        /// get/set - The lease PS file number.
        /// </summary>
        [Column("PS_FILE_NO")]
        public string PsFileNo { get; set; }

        /// <summary>
        /// get/set - The lease start date.
        /// </summary>
        [Column("TERM_START_DATE")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// get/set - The original lease start date.
        /// </summary>
        [Column("ORIG_START_DATE")]
        public DateTime OrigStartDate { get; set; }

        /// <summary>
        /// get/set - The lease renewal date.
        /// </summary>
        [Column("TERM_RENEWAL_DATE")]
        public DateTime? RenewalDate { get; set; }

        /// <summary>
        /// get/set - The number of renewals included in this release.
        /// </summary>
        [Column("INCLUDED_RENEWALS")]
        public int? IncludedRenewals { get; set; }

        /// <summary>
        /// get/set - The lease renewal count.
        /// </summary>
        [Column("RENEWAL_COUNT")]
        public int RenewalCount { get; set; }

        /// <summary>
        /// get/set - The lease renewal term in months.
        /// </summary>
        [Column("RENEWAL_TERM_MONTHS")]
        public int RenewalTermMonths { get; set; }

        /// <summary>
        /// get/set - The lease expiry date.
        /// </summary>
        [Column("TERM_EXPIRY_DATE")]
        public DateTime? TermExpiryDate { get; set; }

        /// <summary>
        /// get/set - The original lease expiry date.
        /// </summary>
        [Column("ORIG_EXPIRY_DATE")]
        public DateTime? OrigExpiryDate { get; set; }

        /// <summary>
        /// get/set - The lease amount.
        /// </summary>
        [Column("LEASE_AMOUNT")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// get/set - The lease insurance start date.
        /// </summary>
        [Column("INSURANCE_START_DATE")]
        public DateTime? InsuranceStartDate { get; set; }

        /// <summary>
        /// get/set - The lease insurance end date.
        /// </summary>
        [Column("INSURANCE_END_DATE")]
        public DateTime? InsuranceEndDate { get; set; }

        /// <summary>
        /// get/set - The lease security start date.
        /// </summary>
        [Column("SECURITY_START_DATE")]
        public DateTime? SecurityStartDate { get; set; }

        /// <summary>
        /// get/set - The lease security end date.
        /// </summary>
        [Column("SECURITY_END_DATE")]
        public DateTime? SecurityEndDate { get; set; }

        /// <summary>
        /// get/set - The lease inspection date.
        /// </summary>
        [Column("INSPECTION_DATE")]
        public DateTime? InspectionDate { get; set; }

        /// <summary>
        /// get/set - The lease responsibility effective date.
        /// </summary>
        [Column("RESPONSIBILITY_EFFECTIVE_DATE")]
        public DateTime? ResponsibilityEffectiveDate { get; set; }

        /// <summary>
        /// get/set - The lease inspection notes.
        /// </summary>
        [Column("INSPECTION_NOTES")]
        public string InspectionNote { get; set; }

        /// <summary>
        /// get/set - The lease notes.
        /// </summary>
        [Column("LEASE_NOTES")]
        public string Note { get; set; }

        /// <summary>
        /// get/set - The lease description.
        /// </summary>
        [Column("LEASE_DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The lease unit.
        /// </summary>
        [Column("UNIT")]
        public string Unit { get; set; }

        /// <summary>
        /// get/set - Whether the lease has expired.
        /// </summary>
        [Column("IS_EXPIRED")]
        public bool IsExpired { get; set; }

        /// <summary>
        /// get/set - Whether the lease has expired.
        /// </summary>
        [Column("IS_ORIG_EXPIRY_REQUIRED")]
        public bool IsOrigExpiryRequired { get; set; }

        /// <summary>
        /// get/set - Whether the lease has a physical file.
        /// </summary>
        [Column("HAS_PHYSICAL_FILE")]
        public bool HasPhysicalFile { get; set; }

        /// <summary>
        /// get/set - Whether the lease has a digital file.
        /// </summary>
        [Column("HAS_DIGITAL_FILE")]
        public bool HasDigitalFile { get; set; }

        /// <summary>
        /// get/set - Whether the lease has a physical license.
        /// </summary>
        [Column("HAS_PHYSICAL_LICENSE")]
        public bool HasPhysicalLicense { get; set; }

        /// <summary>
        /// get/set - Whether the lease has a digital license.
        /// </summary>
        [Column("HAS_DIGITAL_LICENSE")]
        public bool HasDigitalLicense { get; set; }

        /// <summary>
        /// get/set - Whether this improvement contains a building that is subject to RTA.
        /// </summary>
        [Column("IS_SUBJECT_TO_RTA")]
        public bool IsSubjectToRta { get; set; }

        /// <summary>
        /// get/set - Whether this improvement contains a commercial building.
        /// </summary>
        [Column("IS_COMM_BLDG")]
        public bool IsCommBldg { get; set; }

        /// <summary>
        /// get/set - Whether this improvement is of type other.
        /// </summary>
        [Column("IS_OTHER_IMPROVEMENT")]
        public bool IsOtherImprovement { get; set; }

        /// <summary>
        /// get/set - A collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();

        /// <summary>
        /// get/set - A collection of many-to-many entities that link properties and leases..
        /// </summary>
        public ICollection<PropertyLease> PropertiesManyToMany { get; } = new List<PropertyLease>();

        /// <summary>
        /// get/set - A collection of many-to-many entities that link tenants and leases.
        /// </summary>
        public ICollection<LeaseTenant> TenantsManyToMany { get; } = new List<LeaseTenant>();

        /// <summary>
        /// get/set - A collection of Persons associated to this Lease
        /// </summary>
        public ICollection<Person> Persons { get; } = new List<Person>();

        /// <summary>
        /// get/set - A collection of Organizations associated to this Lease
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();

        /// <summary>
        /// get/set - A collection of Improvements associated to this Lease
        /// </summary>
        public ICollection<PropertyImprovement> Improvements { get; } = new List<PropertyImprovement>();

        /// <summary>
        /// get/set - A collection of Insurances associated to this Lease
        /// </summary>
        public ICollection<Insurance> Insurances { get; } = new List<Insurance>();

        /// <summary>
        /// get/set - A collection of Security Deposits associated to this Lease
        /// </summary>
        public ICollection<SecurityDeposit> SecurityDeposits { get; } = new List<SecurityDeposit>();

        /// <summary>
        /// get/set - A collection of Security Deposit Returns associated to this Lease
        /// </summary>
        public ICollection<SecurityDepositReturn> SecurityDepositReturns { get; } = new List<SecurityDepositReturn>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Lease class.
        /// </summary>
        public Lease() { }

        /// <summary>
        /// Create a new instance of a Lease class.
        /// </summary>
        /// <param name="purposeType"></param>
        /// <param name="statusType"></param>
        /// <param name="paymentFrequencyType"></param>
        public Lease(LeasePurposeType purposeType, LeaseStatusType statusType, LeasePaymentFrequencyType paymentFrequencyType)
        {
            this.PurposeTypeId = purposeType?.Id ?? throw new ArgumentNullException(nameof(purposeType));
            this.PurposeType = purposeType;
            this.StatusTypeId = statusType?.Id ?? throw new ArgumentNullException(nameof(statusType));
            this.StatusType = statusType;
            this.PaymentFrequencyTypeId = paymentFrequencyType?.Id ?? throw new ArgumentNullException(nameof(paymentFrequencyType));
            this.PaymentFrequencyType = paymentFrequencyType;
        }
        #endregion
    }
}
