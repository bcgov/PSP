using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsLease
    {
        public PimsLease()
        {
            PimsInsurances = new HashSet<PimsInsurance>();
            PimsLeaseTenants = new HashSet<PimsLeaseTenant>();
            PimsLeaseTerms = new HashSet<PimsLeaseTerm>();
            PimsPropertyImprovements = new HashSet<PimsPropertyImprovement>();
            PimsPropertyLeases = new HashSet<PimsPropertyLease>();
            PimsSecurityDepositReturns = new HashSet<PimsSecurityDepositReturn>();
            PimsSecurityDeposits = new HashSet<PimsSecurityDeposit>();
        }

        public long LeaseId { get; set; }
        public string LeasePayRvblTypeCode { get; set; }
        public string LeaseLicenseTypeCode { get; set; }
        public string LeaseCategoryTypeCode { get; set; }
        public string LeasePurposeTypeCode { get; set; }
        public string LeaseProgramTypeCode { get; set; }
        public string LeaseInitiatorTypeCode { get; set; }
        public string LeaseResponsibilityTypeCode { get; set; }
        public string LeasePmtFreqTypeCode { get; set; }
        public string LeaseStatusTypeCode { get; set; }
        public string LFileNo { get; set; }
        public int? TfaFileNo { get; set; }
        public string PsFileNo { get; set; }
        public string LeaseDescription { get; set; }
        public string LeaseCategoryOtherDesc { get; set; }
        public string LeasePurposeOtherDesc { get; set; }
        public string LeaseNotes { get; set; }
        public string MotiContact { get; set; }
        public string MotiRegion { get; set; }
        public string DocumentationReference { get; set; }
        public string ReturnNotes { get; set; }
        public string OtherLeaseProgramType { get; set; }
        public string OtherLeaseLicenseType { get; set; }
        public string OtherLeasePurposeType { get; set; }
        public DateTime OrigStartDate { get; set; }
        public DateTime? OrigExpiryDate { get; set; }
        public decimal? LeaseAmount { get; set; }
        public DateTime? ResponsibilityEffectiveDate { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string InspectionNotes { get; set; }
        public bool? IsSubjectToRta { get; set; }
        public bool? IsCommBldg { get; set; }
        public bool? IsOtherImprovement { get; set; }
        public bool? IsExpired { get; set; }
        public bool? HasPhysicalFile { get; set; }
        public bool? HasDigitalFile { get; set; }
        public bool? HasPhysicialLicense { get; set; }
        public bool? HasDigitalLicense { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserid { get; set; }
        public Guid? AppCreateUserGuid { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public Guid? AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual PimsLeaseCategoryType LeaseCategoryTypeCodeNavigation { get; set; }
        public virtual PimsLeaseInitiatorType LeaseInitiatorTypeCodeNavigation { get; set; }
        public virtual PimsLeaseLicenseType LeaseLicenseTypeCodeNavigation { get; set; }
        public virtual PimsLeasePayRvblType LeasePayRvblTypeCodeNavigation { get; set; }
        public virtual PimsLeasePmtFreqType LeasePmtFreqTypeCodeNavigation { get; set; }
        public virtual PimsLeaseProgramType LeaseProgramTypeCodeNavigation { get; set; }
        public virtual PimsLeasePurposeType LeasePurposeTypeCodeNavigation { get; set; }
        public virtual PimsLeaseResponsibilityType LeaseResponsibilityTypeCodeNavigation { get; set; }
        public virtual PimsLeaseStatusType LeaseStatusTypeCodeNavigation { get; set; }
        public virtual ICollection<PimsInsurance> PimsInsurances { get; set; }
        public virtual ICollection<PimsLeaseTenant> PimsLeaseTenants { get; set; }
        public virtual ICollection<PimsLeaseTerm> PimsLeaseTerms { get; set; }
        public virtual ICollection<PimsPropertyImprovement> PimsPropertyImprovements { get; set; }
        public virtual ICollection<PimsPropertyLease> PimsPropertyLeases { get; set; }
        public virtual ICollection<PimsSecurityDepositReturn> PimsSecurityDepositReturns { get; set; }
        public virtual ICollection<PimsSecurityDeposit> PimsSecurityDeposits { get; set; }
    }
}
