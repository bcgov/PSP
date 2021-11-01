using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Insurance class, provides an entity for the datamodel to manage insurance.
    /// </summary>
    [MotiTable("PIMS_INSURANCE", "INSRNC")]
    public class Insurance : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify insurance.
        /// </summary>
        [Column("INSURANCE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease.
        /// </summary>
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The lease.
        /// </summary>
        public Lease Lease { get; set; }

        [Column("INSURANCE_TYPE_CODE")]
        public string InsuranceTypeId { get; set; }

        public InsuranceType InsuranceType { get; set; }

        [Column("INSURER_ORG_ID")]
        public long InsurerOrganizationId { get; set; }

        public Organization InsurerOrganization { get; set; }

        [Column("INSURER_CONTACT_ID")]
        public long InsurerContactId { get; set; }

        public Person InsurerContact { get; set; }

        [Column("MOTI_RISK_MGMT_CONTACT_ID")]
        public long MotiRiskManagementContactId { get; set; }

        public Person MotiRiskManagementContact { get; set; }

        [Column("BCTFA_RISK_MGMT_CONTACT_ID")]
        public long BctfaRiskManagementContactId { get; set; }

        public Person BctfaRiskManagementContact { get; set; }

        [Column("INSURANCE_PAYEE_TYPE_CODE")]
        public string InsurancePayeeTypeId { get; set; }

        public InsurancePayeeType InsurancePayeeType { get; set; }

        /// <summary>
        /// get/set - The text description if the insurance type is other.
        /// </summary>
        [Column("OTHER_INSURANCE_TYPE")]
        public string OtherInsuranceType { get; set; }

        /// <summary>
        /// get/set - 2000 character description of coverage.
        /// </summary>
        [Column("COVERAGE_DESCRIPTION")]
        public string CoverageDescription { get; set; }

        [Column("COVERAGE_LIMIT")]
        public decimal CoverageLimit { get; set; }

        [Column("INSURED_VALUE")]
        public decimal InsuredValue { get; set; }

        [Column("START_DATE")]
        public DateTime StartDate { get; set; }

        [Column("EXPIRY_DATE")]
        public DateTime ExpiryDate { get; set; }

        [Column("RISK_ASSESSMENT_COMPLETED_DATE")]
        public DateTime? RiskAssessmentCompletedDate { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a insurance class.
        /// </summary>
        public Insurance() { }

        /// <summary>
        /// Create a new instance of a insurance class.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="insuranceType"></param>
        /// <param name="insurerOrganization"></param>
        /// <param name="insurerContact"></param>
        /// <param name="motiRiskManagementContact"></param>
        /// <param name="bctfaRiskManagementContact"></param>
        /// <param name="insurancePayeeType"></param>
        /// <param name="otherInsuranceType"></param>
        /// <param name="coverageLimit"></param>
        /// <param name="insuredValue"></param>
        /// <param name="startDate"></param>
        /// <param name="expiryDate"></param>
        public Insurance(Lease lease, InsuranceType insuranceType, Organization insurerOrganization, Person insurerContact, Person motiRiskManagementContact, Person bctfaRiskManagementContact,
            InsurancePayeeType insurancePayeeType, string otherInsuranceType, string coverageDescription, decimal coverageLimit, decimal insuredValue, DateTime startDate, DateTime expiryDate)
        {
            this.Lease = lease ?? throw new ArgumentNullException(nameof(lease));
            this.LeaseId = lease.Id;
            this.InsuranceType = insuranceType ?? throw new ArgumentNullException(nameof(insuranceType));
            this.InsuranceTypeId = InsuranceType.Id;
            this.InsurerOrganization = insurerOrganization ?? throw new ArgumentNullException(nameof(insurerOrganization));
            this.InsurerOrganizationId = insurerOrganization.Id;
            this.InsurerContact = insurerContact ?? throw new ArgumentNullException(nameof(insurerContact));
            this.InsurerContactId = insurerOrganization.Id;
            this.MotiRiskManagementContact = motiRiskManagementContact ?? throw new ArgumentNullException(nameof(motiRiskManagementContact));
            this.MotiRiskManagementContactId = motiRiskManagementContact.Id;
            this.BctfaRiskManagementContact = bctfaRiskManagementContact ?? throw new ArgumentNullException(nameof(bctfaRiskManagementContact));
            this.BctfaRiskManagementContactId = bctfaRiskManagementContact.Id;
            this.InsurancePayeeType = insurancePayeeType ?? throw new ArgumentNullException(nameof(insurancePayeeType));
            this.InsurancePayeeTypeId = insurancePayeeType.Id;
            this.OtherInsuranceType = otherInsuranceType;
            this.CoverageDescription = coverageDescription;
            this.CoverageLimit = coverageLimit;
            this.InsuredValue = insuredValue;
            this.StartDate = startDate;
            this.ExpiryDate = expiryDate;
        }
        #endregion
    }
}
