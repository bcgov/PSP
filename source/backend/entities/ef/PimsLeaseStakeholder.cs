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
    [Key]
    [Column("LEASE_STAKEHOLDER_ID")]
    public long LeaseStakeholderId { get; set; }

    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    [Required]
    [Column("LESSOR_TYPE_CODE")]
    [StringLength(20)]
    public string LessorTypeCode { get; set; }

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

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

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
    public virtual ICollection<PimsLeaseStakeholderCompReq> PimsLeaseStakeholderCompReqs { get; set; } = new List<PimsLeaseStakeholderCompReq>();

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsLeaseStakeholderPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }
}
