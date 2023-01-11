using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY_LEASE")]
    [Index(nameof(LeaseId), Name = "PROPLS_LEASE_ID_IDX")]
    [Index(nameof(PropertyId), Name = "PROPLS_PROPERTY_ID_IDX")]
    [Index(nameof(LeaseId), nameof(PropertyId), Name = "PROPLS_PROPERTY_LEASE_TUC", IsUnique = true)]
    public partial class PimsPropertyLease
    {
        [Key]
        [Column("PROPERTY_LEASE_ID")]
        public long PropertyLeaseId { get; set; }
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
        [Column("AREA_UNIT_TYPE_CODE")]
        [StringLength(20)]
        public string AreaUnitTypeCode { get; set; }
        [Column("NAME")]
        [StringLength(250)]
        public string Name { get; set; }
        [Column("LEASE_AREA")]
        public float? LeaseArea { get; set; }
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

        [ForeignKey(nameof(AreaUnitTypeCode))]
        [InverseProperty(nameof(PimsAreaUnitType.PimsPropertyLeases))]
        public virtual PimsAreaUnitType AreaUnitTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(LeaseId))]
        [InverseProperty(nameof(PimsLease.PimsPropertyLeases))]
        public virtual PimsLease Lease { get; set; }
        [ForeignKey(nameof(PropertyId))]
        [InverseProperty(nameof(PimsProperty.PimsPropertyLeases))]
        public virtual PimsProperty Property { get; set; }
    }
}
