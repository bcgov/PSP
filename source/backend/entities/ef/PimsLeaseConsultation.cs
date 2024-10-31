using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_LEASE_CONSULTATION")]
[Index("ConsultationOutcomeTypeCode", Name = "LESCON_CONSULTATION_OUTCOME_TYPE_CODE_IDX")]
[Index("ConsultationStatusTypeCode", Name = "LESCON_CONSULTATION_STATUS_TYPE_CODE_IDX")]
[Index("ConsultationTypeCode", Name = "LESCON_CONSULTATION_TYPE_CODE_IDX")]
[Index("LeaseId", Name = "LESCON_LEASE_ID_IDX")]
[Index("OrganizationId", Name = "LESCON_ORGANIZATION_ID_IDX")]
[Index("PersonId", Name = "LESCON_PERSON_ID_IDX")]
[Index("PrimaryContactId", Name = "LESCON_PRIMARY_CONTACT_ID_IDX")]
public partial class PimsLeaseConsultation
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("LEASE_CONSULTATION_ID")]
    public long LeaseConsultationId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    /// <summary>
    /// Consultation contact person exclusive of an organization.
    /// </summary>
    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// Consultation contact organization.
    /// </summary>
    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    /// <summary>
    /// Consultation contact person within the organization.
    /// </summary>
    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_CONSULTATION_TYPE table.
    /// </summary>
    [Required]
    [Column("CONSULTATION_TYPE_CODE")]
    [StringLength(20)]
    public string ConsultationTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_CONSULTATION_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("CONSULTATION_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ConsultationStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_CONSULTATION_OUTCOME_TYPE table.
    /// </summary>
    [Required]
    [Column("CONSULTATION_OUTCOME_TYPE_CODE")]
    [StringLength(20)]
    public string ConsultationOutcomeTypeCode { get; set; }

    /// <summary>
    /// Description for the approval / consultation when &quot;Other&quot; consultation type is selected.
    /// </summary>
    [Column("OTHER_DESCRIPTION")]
    [StringLength(2000)]
    public string OtherDescription { get; set; }

    /// <summary>
    /// Date that the approval / consultation request was sent.
    /// </summary>
    [Column("REQUESTED_ON", TypeName = "datetime")]
    public DateTime? RequestedOn { get; set; }

    /// <summary>
    /// Has the consultation request response been received?
    /// </summary>
    [Column("IS_RESPONSE_RECEIVED")]
    public bool? IsResponseReceived { get; set; }

    /// <summary>
    /// Date that the consultation request response was received.
    /// </summary>
    [Column("RESPONSE_RECEIVED_DATE", TypeName = "datetime")]
    public DateTime? ResponseReceivedDate { get; set; }

    /// <summary>
    /// Remarks / summary on the process or its results.
    /// </summary>
    [Column("COMMENT")]
    [StringLength(500)]
    public string Comment { get; set; }

    /// <summary>
    /// if the record is disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

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

    [ForeignKey("ConsultationOutcomeTypeCode")]
    [InverseProperty("PimsLeaseConsultations")]
    public virtual PimsConsultationOutcomeType ConsultationOutcomeTypeCodeNavigation { get; set; }

    [ForeignKey("ConsultationStatusTypeCode")]
    [InverseProperty("PimsLeaseConsultations")]
    public virtual PimsConsultationStatusType ConsultationStatusTypeCodeNavigation { get; set; }

    [ForeignKey("ConsultationTypeCode")]
    [InverseProperty("PimsLeaseConsultations")]
    public virtual PimsConsultationType ConsultationTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsLeaseConsultations")]
    public virtual PimsLease Lease { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsLeaseConsultations")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsLeaseConsultationPeople")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsLeaseConsultationPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }
}
