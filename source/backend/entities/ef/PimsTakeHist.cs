using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_TAKE_HIST")]
    [Index(nameof(TakeHistId), nameof(EndDateHist), Name = "PIMS_TAKE_H_UK", IsUnique = true)]
    public partial class PimsTakeHist
    {
        [Key]
        [Column("_TAKE_HIST_ID")]
        public long TakeHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("TAKE_ID")]
        public long TakeId { get; set; }
        [Column("PROPERTY_ACQUISITION_FILE_ID")]
        public long PropertyAcquisitionFileId { get; set; }
        [Required]
        [Column("TAKE_TYPE_CODE")]
        [StringLength(20)]
        public string TakeTypeCode { get; set; }
        [Required]
        [Column("TAKE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string TakeStatusTypeCode { get; set; }
        [Column("TAKE_SITE_CONTAM_TYPE_CODE")]
        [StringLength(20)]
        public string TakeSiteContamTypeCode { get; set; }
        [Column("AREA_UNIT_TYPE_CODE")]
        [StringLength(20)]
        public string AreaUnitTypeCode { get; set; }
        [Column("LAND_ACT_TYPE_CODE")]
        [StringLength(20)]
        public string LandActTypeCode { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(4000)]
        public string Description { get; set; }
        [Column("IS_NEW_RIGHT_OF_WAY")]
        public bool IsNewRightOfWay { get; set; }
        [Column("NEW_RIGHT_OF_WAY_AREA")]
        public float? NewRightOfWayArea { get; set; }
        [Column("IS_STATUTORY_RIGHT_OF_WAY")]
        public bool IsStatutoryRightOfWay { get; set; }
        [Column("STATUTORY_RIGHT_OF_WAY_AREA")]
        public float? StatutoryRightOfWayArea { get; set; }
        [Column("IS_LICENSE_TO_CONSTRUCT")]
        public bool IsLicenseToConstruct { get; set; }
        [Column("LICENSE_TO_CONSTRUCT_AREA")]
        public float? LicenseToConstructArea { get; set; }
        [Column("LTC_END_DT", TypeName = "date")]
        public DateTime? LtcEndDt { get; set; }
        [Column("IS_LAND_ACT")]
        public bool IsLandAct { get; set; }
        [Column("LAND_ACT_AREA")]
        public float? LandActArea { get; set; }
        [Column("LAND_ACT_END_DT", TypeName = "date")]
        public DateTime? LandActEndDt { get; set; }
        [Column("IS_SURPLUS")]
        public bool IsSurplus { get; set; }
        [Column("SURPLUS_AREA")]
        public float? SurplusArea { get; set; }
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
    }
}
