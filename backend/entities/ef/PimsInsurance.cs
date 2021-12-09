using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsInsurance
    {
        public long InsuranceId { get; set; }
        public long LeaseId { get; set; }
        public string InsuranceTypeCode { get; set; }
        public long InsurerOrgId { get; set; }
        public long InsurerContactId { get; set; }
        public long MotiRiskMgmtContactId { get; set; }
        public long BctfaRiskMgmtContactId { get; set; }
        public string InsurancePayeeTypeCode { get; set; }
        public string OtherInsuranceType { get; set; }
        public string CoverageDescription { get; set; }
        public decimal CoverageLimit { get; set; }
        public decimal InsuredValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? RiskAssessmentCompletedDate { get; set; }
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

        public virtual PimsPerson BctfaRiskMgmtContact { get; set; }
        public virtual PimsInsurancePayeeType InsurancePayeeTypeCodeNavigation { get; set; }
        public virtual PimsInsuranceType InsuranceTypeCodeNavigation { get; set; }
        public virtual PimsPerson InsurerContact { get; set; }
        public virtual PimsOrganization InsurerOrg { get; set; }
        public virtual PimsLease Lease { get; set; }
        public virtual PimsPerson MotiRiskMgmtContact { get; set; }
    }
}
