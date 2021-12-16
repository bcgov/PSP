using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY_IMPROVEMENT")]
    [Index(nameof(LeaseId), nameof(PropertyImprovementTypeCode), Name = "PIMPRV_LEASE_IMPROVEMENT_TUC", IsUnique = true)]
    [Index(nameof(PropertyImprovementTypeCode), Name = "PIMPRV_PROPERTY_IMPROVEMENT_TYPE_CODE_IDX")]
    [Index(nameof(LeaseId), Name = "PIMPRV_PROPERTY_LEASE_ID_IDX")]
    public partial class PimsPropertyImprovement
    {
        [Key]
        [Column("PROPERTY_IMPROVEMENT_ID")]
        public long PropertyImprovementId { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
        [Required]
        [Column("PROPERTY_IMPROVEMENT_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyImprovementTypeCode { get; set; }
        [Column("IMPROVEMENT_DESCRIPTION")]
        [StringLength(2000)]
        public string ImprovementDescription { get; set; }
        [Column("STRUCTURE_SIZE")]
        [StringLength(2000)]
        public string StructureSize { get; set; }
        [Column("ADDRESS")]
        [StringLength(2000)]
        public string Address { get; set; }
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
        [InverseProperty(nameof(PimsLease.PimsPropertyImprovements))]
        public virtual PimsLease Lease { get; set; }
        [ForeignKey(nameof(PropertyImprovementTypeCode))]
        [InverseProperty(nameof(PimsPropertyImprovementType.PimsPropertyImprovements))]
        public virtual PimsPropertyImprovementType PropertyImprovementTypeCodeNavigation { get; set; }
    }
}
