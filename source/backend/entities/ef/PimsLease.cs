﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Details of a lease that is inventoried in PIMS system.
/// </summary>
[Table("PIMS_LEASE")]
[Index("LeaseCategoryTypeCode", Name = "LEASE_LEASE_CATEGORY_TYPE_CODE_IDX")]
[Index("LeaseInitiatorTypeCode", Name = "LEASE_LEASE_INITIATOR_TYPE_CODE_IDX")]
[Index("LeaseLicenseTypeCode", Name = "LEASE_LEASE_LICENSE_TYPE_CODE_IDX")]
[Index("LeasePayRvblTypeCode", Name = "LEASE_LEASE_PAY_RVBL_TYPE_CODE_IDX")]
[Index("LeaseProgramTypeCode", Name = "LEASE_LEASE_PROGRAM_TYPE_CODE_IDX")]
[Index("LeasePurposeTypeCode", Name = "LEASE_LEASE_PURPOSE_TYPE_CODE_IDX")]
[Index("LeaseResponsibilityTypeCode", Name = "LEASE_LEASE_RESPONSIBILITY_TYPE_CODE_IDX")]
[Index("LeaseStatusTypeCode", Name = "LEASE_LEASE_STATUS_TYPE_CODE_IDX")]
[Index("LFileNo", Name = "LEASE_L_FILE_NO_IDX")]
[Index("PsFileNo", Name = "LEASE_PS_FILE_NO_IDX")]
[Index("RegionCode", Name = "LEASE_REGION_CODE_IDX")]
[Index("TfaFileNo", Name = "LEASE_TFA_FILE_NO_IDX")]
[Index("TfaFileNumber", Name = "LEASE_TFA_FILE_NUMBER_IDX")]
public partial class PimsLease
{
    [Key]
    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    [Required]
    [Column("LEASE_PAY_RVBL_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePayRvblTypeCode { get; set; }

    [Required]
    [Column("LEASE_LICENSE_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseLicenseTypeCode { get; set; }

    [Column("LEASE_CATEGORY_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseCategoryTypeCode { get; set; }

    [Required]
    [Column("LEASE_PURPOSE_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePurposeTypeCode { get; set; }

    [Required]
    [Column("LEASE_PROGRAM_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseProgramTypeCode { get; set; }

    [Column("LEASE_INITIATOR_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseInitiatorTypeCode { get; set; }

    [Column("LEASE_RESPONSIBILITY_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseResponsibilityTypeCode { get; set; }

    [Required]
    [Column("LEASE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseStatusTypeCode { get; set; }

    /// <summary>
    /// MoTI region associated with the lease
    /// </summary>
    [Column("REGION_CODE")]
    public short? RegionCode { get; set; }

    /// <summary>
    /// Project associated with this lease.
    /// </summary>
    [Column("PROJECT_ID")]
    public long? ProjectId { get; set; }

    /// <summary>
    /// Generated identifying lease/licence number
    /// </summary>
    [Column("L_FILE_NO")]
    [StringLength(50)]
    public string LFileNo { get; set; }

    /// <summary>
    /// Sourced from t_fileMain.TFA_File_Number
    /// </summary>
    [Column("TFA_FILE_NO")]
    public int? TfaFileNo { get; set; }

    /// <summary>
    /// Sourced from t_fileMain.TFA_File_Number || - || t_fileSub.Subfile_Sequence_Code
    /// </summary>
    [Column("TFA_FILE_NUMBER")]
    [StringLength(500)]
    public string TfaFileNumber { get; set; }

    /// <summary>
    /// Sourced from t_fileSubOverrideData.PSFile_No
    /// </summary>
    [Column("PS_FILE_NO")]
    [StringLength(50)]
    public string PsFileNo { get; set; }

    /// <summary>
    /// Manually etered lease description, not the legal description
    /// </summary>
    [Column("LEASE_DESCRIPTION")]
    public string LeaseDescription { get; set; }

    /// <summary>
    /// User-specified lease category description not included in standard set of lease purposes
    /// </summary>
    [Column("LEASE_CATEGORY_OTHER_DESC")]
    [StringLength(200)]
    public string LeaseCategoryOtherDesc { get; set; }

    /// <summary>
    /// User-specified lease purpose description not included in standard set of lease purposes
    /// </summary>
    [Column("LEASE_PURPOSE_OTHER_DESC")]
    [StringLength(200)]
    public string LeasePurposeOtherDesc { get; set; }

    /// <summary>
    /// Notes accompanying lease
    /// </summary>
    [Column("LEASE_NOTES")]
    public string LeaseNotes { get; set; }

    /// <summary>
    /// Contact of the MoTI person associated with the lease
    /// </summary>
    [Column("MOTI_CONTACT")]
    [StringLength(200)]
    public string MotiContact { get; set; }

    /// <summary>
    /// Location of documents pertianing to the lease/license
    /// </summary>
    [Column("DOCUMENTATION_REFERENCE")]
    [StringLength(500)]
    public string DocumentationReference { get; set; }

    /// <summary>
    /// Notes accompanying lease
    /// </summary>
    [Column("RETURN_NOTES")]
    public string ReturnNotes { get; set; }

    /// <summary>
    /// Description of a non-standard lease program type
    /// </summary>
    [Column("OTHER_LEASE_PROGRAM_TYPE")]
    [StringLength(200)]
    public string OtherLeaseProgramType { get; set; }

    /// <summary>
    /// Description of a non-standard lease/license type
    /// </summary>
    [Column("OTHER_LEASE_LICENSE_TYPE")]
    [StringLength(200)]
    public string OtherLeaseLicenseType { get; set; }

    /// <summary>
    /// Description of a non-standard lease purpose type
    /// </summary>
    [Column("OTHER_LEASE_PURPOSE_TYPE")]
    [StringLength(200)]
    public string OtherLeasePurposeType { get; set; }

    /// <summary>
    /// Original start date of the lease/license
    /// </summary>
    [Column("ORIG_START_DATE", TypeName = "datetime")]
    public DateTime OrigStartDate { get; set; }

    /// <summary>
    /// Original expiry date of the lease/license
    /// </summary>
    [Column("ORIG_EXPIRY_DATE", TypeName = "datetime")]
    public DateTime? OrigExpiryDate { get; set; }

    /// <summary>
    /// Lease/licence amount
    /// </summary>
    [Column("LEASE_AMOUNT", TypeName = "money")]
    public decimal? LeaseAmount { get; set; }

    /// <summary>
    /// Date current responsibility came into effect for this lease
    /// </summary>
    [Column("RESPONSIBILITY_EFFECTIVE_DATE", TypeName = "datetime")]
    public DateTime? ResponsibilityEffectiveDate { get; set; }

    /// <summary>
    /// Inspection date
    /// </summary>
    [Column("INSPECTION_DATE", TypeName = "datetime")]
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// Notes accompanying inspection
    /// </summary>
    [Column("INSPECTION_NOTES")]
    public string InspectionNotes { get; set; }

    /// <summary>
    /// Is subject the Residential Tenancy Act
    /// </summary>
    [Column("IS_SUBJECT_TO_RTA")]
    public bool? IsSubjectToRta { get; set; }

    /// <summary>
    /// Is a commercial building
    /// </summary>
    [Column("IS_COMM_BLDG")]
    public bool? IsCommBldg { get; set; }

    /// <summary>
    /// Is improvement of another description
    /// </summary>
    [Column("IS_OTHER_IMPROVEMENT")]
    public bool? IsOtherImprovement { get; set; }

    /// <summary>
    /// Incidcator that lease/license has expired
    /// </summary>
    [Column("IS_EXPIRED")]
    public bool IsExpired { get; set; }

    /// <summary>
    /// Indicator that phyical file exists
    /// </summary>
    [Column("HAS_PHYSICAL_FILE")]
    public bool? HasPhysicalFile { get; set; }

    /// <summary>
    /// Indicator that digital file exists
    /// </summary>
    [Column("HAS_DIGITAL_FILE")]
    public bool? HasDigitalFile { get; set; }

    /// <summary>
    /// Indicator that physical license exists
    /// </summary>
    [Column("HAS_PHYSICIAL_LICENSE")]
    public bool? HasPhysicialLicense { get; set; }

    /// <summary>
    /// Indicator that digital license exists
    /// </summary>
    [Column("HAS_DIGITAL_LICENSE")]
    public bool? HasDigitalLicense { get; set; }

    /// <summary>
    /// Reason for the cancellation of the lease.  For example, &quot;The request for leasing the space was withdrawn.&quot;
    /// </summary>
    [Column("CANCELLATION_REASON")]
    [StringLength(500)]
    public string CancellationReason { get; set; }

    /// <summary>
    /// Reason for the termination of the lease.  For example, &quot;The tenant is in violation of the terms of the agreement.&quot;
    /// </summary>
    [Column("TERMINATION_REASON")]
    [StringLength(500)]
    public string TerminationReason { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("LeaseCategoryTypeCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsLeaseCategoryType LeaseCategoryTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseInitiatorTypeCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsLeaseInitiatorType LeaseInitiatorTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseLicenseTypeCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsLeaseLicenseType LeaseLicenseTypeCodeNavigation { get; set; }

    [ForeignKey("LeasePayRvblTypeCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsLeasePayRvblType LeasePayRvblTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseProgramTypeCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsLeaseProgramType LeaseProgramTypeCodeNavigation { get; set; }

    [ForeignKey("LeasePurposeTypeCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsLeasePurposeType LeasePurposeTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseResponsibilityTypeCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsLeaseResponsibilityType LeaseResponsibilityTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseStatusTypeCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsLeaseStatusType LeaseStatusTypeCodeNavigation { get; set; }

    [InverseProperty("Lease")]
    public virtual ICollection<PimsInsurance> PimsInsurances { get; set; } = new List<PimsInsurance>();

    [InverseProperty("Lease")]
    public virtual ICollection<PimsLeaseConsultation> PimsLeaseConsultations { get; set; } = new List<PimsLeaseConsultation>();

    [InverseProperty("Lease")]
    public virtual ICollection<PimsLeaseDocument> PimsLeaseDocuments { get; set; } = new List<PimsLeaseDocument>();

    [InverseProperty("Lease")]
    public virtual ICollection<PimsLeaseNote> PimsLeaseNotes { get; set; } = new List<PimsLeaseNote>();

    [InverseProperty("Lease")]
    public virtual ICollection<PimsLeaseTenant> PimsLeaseTenants { get; set; } = new List<PimsLeaseTenant>();

    [InverseProperty("Lease")]
    public virtual ICollection<PimsLeaseTerm> PimsLeaseTerms { get; set; } = new List<PimsLeaseTerm>();

    [InverseProperty("Lease")]
    public virtual ICollection<PimsPropertyImprovement> PimsPropertyImprovements { get; set; } = new List<PimsPropertyImprovement>();

    [InverseProperty("Lease")]
    public virtual ICollection<PimsPropertyLease> PimsPropertyLeases { get; set; } = new List<PimsPropertyLease>();

    [InverseProperty("Lease")]
    public virtual ICollection<PimsSecurityDeposit> PimsSecurityDeposits { get; set; } = new List<PimsSecurityDeposit>();

    [ForeignKey("ProjectId")]
    [InverseProperty("PimsLeases")]
    public virtual PimsProject Project { get; set; }

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsLeases")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }
}
