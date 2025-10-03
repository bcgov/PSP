using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Associates a tenant with a lease
/// </summary>
[Table("PIMS_LEASE_STAKEHOLDER")]
[Index("LeaseId", Name = "LSTKHL_LEASE_ID_IDX")]
[Index("OrganizationId", "PersonId", "LeaseId", Name = "LSTKHL_LEASE_PERSON_ORG_TUC", IsUnique = true)]
[Index("LeaseStakeholderTypeCode", Name = "LSTKHL_LEASE_STAKEHOLDER_TYPE_CODE_IDX")]
[Index("LessorTypeCode", Name = "LSTKHL_LESSOR_TYPE_CODE_IDX")]
[Index("OrganizationId", Name = "LSTKHL_ORGANIZATION_ID_IDX")]
[Index("PersonId", Name = "LSTKHL_PERSON_ID_IDX")]
[Index("PrimaryContactId", Name = "LSTKHL_PRIMARY_CONTACT_ID_IDX")]
public partial class PimsLeaseStakeholder
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("LEASE_STAKEHOLDER_ID")]
    public long LeaseStakeholderId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table.
    /// </summary>
    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ORGANIZATION table.
    /// </summary>
    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table for the organization&apos;s primary contact.
    /// </summary>
    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LESSOR_TYPE table.
    /// </summary>
    [Required]
    [Column("LESSOR_TYPE_CODE")]
    [StringLength(20)]
    public string LessorTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE_STAKEHOLDER_TYPE table.
    /// </summary>
    [Required]
    [Column("LEASE_STAKEHOLDER_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseStakeholderTypeCode { get; set; }

    /// <summary>
    /// Notes associated with the lease/tenant relationship.
    /// </summary>
    [Column("NOTE")]
    [StringLength(2000)]
    public string Note { get; set; }

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
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
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

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsLeaseStakeholders")]
    public virtual PimsLease Lease { get; set; }

    [ForeignKey("LeaseStakeholderTypeCode")]
    [InverseProperty("PimsLeaseStakeholders")]
    public virtual PimsLeaseStakeholderType LeaseStakeholderTypeCodeNavigation { get; set; }

    [ForeignKey("LessorTypeCode")]
    [InverseProperty("PimsLeaseStakeholders")]
    public virtual PimsLessorType LessorTypeCodeNavigation { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsLeaseStakeholders")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsLeaseStakeholderPeople")]
    public virtual PimsPerson Person { get; set; }

    [InverseProperty("LeaseStakeholder")]
    public virtual ICollection<PimsCompReqLeasePayee> PimsCompReqLeasePayees { get; set; } = new List<PimsCompReqLeasePayee>();

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsLeaseStakeholderPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }
}
