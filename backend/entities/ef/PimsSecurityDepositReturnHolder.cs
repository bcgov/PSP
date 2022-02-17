using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_SECURITY_DEPOSIT_RETURN_HOLDER")]
    [Index(nameof(OrganizationId), Name = "SCDPRH_ORGANIZATION_ID_IDX")]
    [Index(nameof(PersonId), Name = "SCDPRH_PERSON_ID_IDX")]
    [Index(nameof(SecurityDepositReturnId), Name = "SCDPRH_SECURITY_DEPOSIT_RETURN_ID_IDX")]
    [Index(nameof(SecurityDepositReturnId), Name = "SCDPRH_SECURITY_DEPOSIT_RETURN_ID_TUC", IsUnique = true)]
    public partial class PimsSecurityDepositReturnHolder
    {
        [Key]
        [Column("SECURITY_DEPOSIT_RETURN_HOLDER_ID")]
        public long SecurityDepositReturnHolderId { get; set; }
        [Column("SECURITY_DEPOSIT_RETURN_ID")]
        public long SecurityDepositReturnId { get; set; }
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }
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
        [Column("APP_LAST_UPDATE_USER_GUID")]
        public Guid? AppLastUpdateUserGuid { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string AppLastUpdateUserid { get; set; }
        [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppLastUpdateTimestamp { get; set; }
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

        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsSecurityDepositReturnHolders))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsSecurityDepositReturnHolders))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(SecurityDepositReturnId))]
        [InverseProperty(nameof(PimsSecurityDepositReturn.PimsSecurityDepositReturnHolder))]
        public virtual PimsSecurityDepositReturn SecurityDepositReturn { get; set; }
    }
}
