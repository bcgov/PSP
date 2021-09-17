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
        /// get/set - Foreign key to the property management organization.
        /// </summary>
        [Column("PROP_MGMT_ORG_ID")]
        public long? PropertyManagementOrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization that manages this property.
        /// </summary>
        public Organization PropertyManagementOrganization { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease purpose type.
        /// </summary>
        [Column("LEASE_PURPOSE_TYPE_CODE")]
        public int PurposeTypeId { get; set; }

        /// <summary>
        /// get/set - The lease purpose type.
        /// </summary>
        public LeasePurposeType PurposeType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease purpose subtype.
        /// </summary>
        [Column("LEASE_PURPOSE_SUBTYPE_CODE")]
        public int PurposeSubtypeId { get; set; }

        /// <summary>
        /// get/set - The lease purpose subtype.
        /// </summary>
        public LeasePurposeSubtype PurposeSubtype { get; set; }

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
        /// get/set - Foreign key to the lease program type.
        /// </summary>
        [Column("LEASE_PROGRAM_TYPE_CODE")]
        public string ProgramTypeId { get; set; }

        /// <summary>
        /// get/set - The lease program type.
        /// </summary>
        public LeaseProgramType ProgramType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease property manager.
        /// </summary>
        [Column("PROPERTY_MANAGER_ID")]
        public long? PropertyManagerId { get; set; }

        /// <summary>
        /// get/set - The lease property manager.
        /// </summary>
        public Person PropertyManager { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease tenant.
        /// </summary>
        [Column("TENANT_ID")]
        public long? TenantId { get; set; }

        /// <summary>
        /// get/set - The lease tenant.
        /// </summary>
        public Person Tenant { get; set; }

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
        [Column("START_DATE")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// get/set - The lease renewal date.
        /// </summary>
        [Column("RENEWAL_DATE")]
        public DateTime? RenewalDate { get; set; }

        /// <summary>
        /// get/set - The lease expiry date.
        /// </summary>
        [Column("EXPIRY_DATE")]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The lease amount.
        /// </summary>
        [Column("LEASE_AMOUNT")]
        public string Amount { get; set; }

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
        /// get/set - The lease unit.
        /// </summary>
        [Column("UNIT")]
        public string Unit { get; set; }

        /// <summary>
        /// get/set - Whether the lease has expired.
        /// </summary>
        [Column("EXPIRED")]
        public bool IsExpired { get; set; }

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
        /// get/set - A collection of lease expected amounts.
        /// </summary>
        public ICollection<LeaseExpectedAmount> ExpectedAmounts { get; } = new List<LeaseExpectedAmount>();

        /// <summary>
        /// get/set - A collection of lease activities.
        /// </summary>
        public ICollection<LeaseActivity> Activities { get; } = new List<LeaseActivity>();

        /// <summary>
        /// get/set - A collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();

        /// <summary>
        /// get/set - A collection of many-to-many entities that link properties and leases..
        /// </summary>
        public ICollection<PropertyLease> PropertiesManyToMany { get; } = new List<PropertyLease>();
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
        /// <param name="purposeSubtype"></param>
        /// <param name="statusType"></param>
        /// <param name="paymentFrequencyType"></param>
        public Lease(LeasePurposeType purposeType, LeasePurposeSubtype purposeSubtype, LeaseStatusType statusType, LeasePaymentFrequencyType paymentFrequencyType)
        {
            this.PurposeTypeId = purposeType?.Id ?? throw new ArgumentNullException(nameof(purposeType));
            this.PurposeType = purposeType;
            this.PurposeSubtypeId = purposeSubtype?.Id ?? throw new ArgumentNullException(nameof(purposeSubtype));
            this.PurposeSubtype = purposeSubtype;
            this.StatusTypeId = statusType?.Id ?? throw new ArgumentNullException(nameof(statusType));
            this.StatusType = statusType;
            this.PaymentFrequencyTypeId = paymentFrequencyType?.Id ?? throw new ArgumentNullException(nameof(paymentFrequencyType));
            this.PaymentFrequencyType = paymentFrequencyType;
        }
        #endregion
    }
}
