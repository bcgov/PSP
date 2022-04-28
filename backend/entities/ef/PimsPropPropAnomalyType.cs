using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROP_PROP_ANOMALY_TYPE")]
    [Index(nameof(PropertyAnomalyTypeCode), Name = "PRPRAT_PROPERTY_ANOMALY_TYPE_CODE_IDX")]
    [Index(nameof(PropertyId), Name = "PRPRAT_PROPERTY_ID_IDX")]
    [Index(nameof(PropertyAnomalyTypeCode), nameof(PropertyId), Name = "PRPRAT_PROP_ANOMALY_TYPE_TUC", IsUnique = true)]
    public partial class PimsPropPropAnomalyType
    {
        [Key]
        [Column("PROP_PROP_ANOMALY_TYPE_ID")]
        public long PropPropAnomalyTypeId { get; set; }
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }
        [Required]
        [Column("PROPERTY_ANOMALY_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyAnomalyTypeCode { get; set; }
        [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppCreateTimestamp { get; set; }
        [Required]
        [Column("APP_CREATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppCreateUserDirectory { get; set; }
        [Column("APP_CREATE_USER_GUID")]
        public Guid? AppCreateUserGuid { get; set; }
        [Required]
        [Column("APP_CREATE_USERID")]
        [StringLength(30)]
        public string AppCreateUserid { get; set; }
        [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppLastUpdateTimestamp { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppLastUpdateUserDirectory { get; set; }
        [Column("APP_LAST_UPDATE_USER_GUID")]
        public Guid? AppLastUpdateUserGuid { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string AppLastUpdateUserid { get; set; }
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long ConcurrencyControlNumber { get; set; }
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

        [ForeignKey(nameof(PropertyId))]
        [InverseProperty(nameof(PimsProperty.PimsPropPropAnomalyTypes))]
        public virtual PimsProperty Property { get; set; }
        [ForeignKey(nameof(PropertyAnomalyTypeCode))]
        [InverseProperty(nameof(PimsPropertyAnomalyType.PimsPropPropAnomalyTypes))]
        public virtual PimsPropertyAnomalyType PropertyAnomalyTypeCodeNavigation { get; set; }
    }
}
