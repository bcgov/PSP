using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_TENANT")]
    [Index(nameof(LeaseId), Name = "TENANT_LEASE_ID_IDX")]
    [Index(nameof(OrganizationId), nameof(PersonId), nameof(LeaseId), Name = "TENANT_LEASE_PERSON_ORG_TUC", IsUnique = true)]
    [Index(nameof(LessorTypeCode), Name = "TENANT_LESSOR_TYPE_CODE_IDX")]
    [Index(nameof(OrganizationId), Name = "TENANT_ORGANIZATION_ID_IDX")]
    [Index(nameof(PersonId), Name = "TENANT_PERSON_ID_IDX")]
    [Index(nameof(PrimaryContactId), Name = "TENANT_PRIMARY_CONTACT_ID_IDX")]
    [Index(nameof(TenantTypeCode), Name = "TENANT_TENANT_TYPE_CODE_IDX")]
    public partial class PimsLeaseTenant
    {
        [Key]
        [Column("LEASE_TENANT_ID")]
        public long LeaseTenantId { get; set; }
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
        [Column("TENANT_TYPE_CODE")]
        [StringLength(20)]
        public string TenantTypeCode { get; set; }
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

        [ForeignKey(nameof(LeaseId))]
        [InverseProperty(nameof(PimsLease.PimsLeaseTenants))]
        public virtual PimsLease Lease { get; set; }
        [ForeignKey(nameof(LessorTypeCode))]
        [InverseProperty(nameof(PimsLessorType.PimsLeaseTenants))]
        public virtual PimsLessorType LessorTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsLeaseTenants))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsLeaseTenantPeople))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(PrimaryContactId))]
        [InverseProperty(nameof(PimsPerson.PimsLeaseTenantPrimaryContacts))]
        public virtual PimsPerson PrimaryContact { get; set; }
        [ForeignKey(nameof(TenantTypeCode))]
        [InverseProperty(nameof(PimsTenantType.PimsLeaseTenants))]
        public virtual PimsTenantType TenantTypeCodeNavigation { get; set; }
    }
}
