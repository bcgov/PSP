using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsLeaseHist
    {
        public long LeaseHistId { get; set; }
        public DateTime EffectiveDateHist { get; set; }
        public DateTime? EndDateHist { get; set; }
        public long LeaseId { get; set; }
        public long MotiNameId { get; set; }
        public string LeasePayRvblTypeCode { get; set; }
        public string LeaseLicenseTypeCode { get; set; }
        public string LeaseCategoryTypeCode { get; set; }
        public string LeasePurposeTypeCode { get; set; }
        public string LeaseProgramTypeCode { get; set; }
        public string LeaseInitiatorTypeCode { get; set; }
        public string LeaseResponsibilityTypeCode { get; set; }
        public string LeasePmtFreqTypeCode { get; set; }
        public string LeasePurposeOtherDesc { get; set; }
        public string LFileNo { get; set; }
        public int? TfaFileNo { get; set; }
        public string PsFileNo { get; set; }
        public string LeaseDescription { get; set; }
        public string LeaseNotes { get; set; }
        public DateTime OrigStartDate { get; set; }
        public DateTime? OrigExpiryDate { get; set; }
        public bool IsOrigExpiryRequired { get; set; }
        public short? IncludedRenewals { get; set; }
        public short? RenewalCount { get; set; }
        public short RenewalTermMonths { get; set; }
        public decimal? LeaseAmount { get; set; }
        public DateTime? ResponsibilityEffectiveDate { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string InspectionNotes { get; set; }
        public bool? IsSubjectToRta { get; set; }
        public bool? IsCommBldg { get; set; }
        public bool? IsOtherImprovement { get; set; }
        public bool IsExpired { get; set; }
        public bool HasPhysicalFile { get; set; }
        public bool HasDigitalFile { get; set; }
        public bool HasPhysicialLicense { get; set; }
        public bool HasDigitalLicense { get; set; }
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
    }
}
