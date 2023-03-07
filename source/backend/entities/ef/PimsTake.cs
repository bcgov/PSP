﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_TAKE")]
    [Index(nameof(PropertyAcquisitionFileId), Name = "TAKE_PROPERTY_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(TakeSiteContamTypeCode), Name = "TAKE_TAKE_SITE_CONTAM_TYPE_CODE_IDX")]
    [Index(nameof(TakeStatusTypeCode), Name = "TAKE_TAKE_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(TakeTypeCode), Name = "TAKE_TAKE_TYPE_CODE_IDX")]
    public partial class PimsTake
    {
        [Key]
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
        [Column("DESCRIPTION")]
        [StringLength(4000)]
        public string Description { get; set; }
        [Required]
        [Column("IS_NEW_RIGHT_OF_WAY")]
        public bool? IsNewRightOfWay { get; set; }
        [Column("NEW_RIGHT_OF_WAY_AREA")]
        public float? NewRightOfWayArea { get; set; }
        [Required]
        [Column("IS_STATUTORY_RIGHT_OF_WAY")]
        public bool? IsStatutoryRightOfWay { get; set; }
        [Column("STATUTORY_RIGHT_OF_WAY_AREA")]
        public float? StatutoryRightOfWayArea { get; set; }
        [Column("SRW_END_DT", TypeName = "date")]
        public DateTime? SrwEndDt { get; set; }
        [Required]
        [Column("IS_LICENSE_TO_CONSTRUCT")]
        public bool? IsLicenseToConstruct { get; set; }
        [Column("LICENSE_TO_CONSTRUCT_AREA")]
        public float? LicenseToConstructArea { get; set; }
        [Column("LTC_END_DT", TypeName = "date")]
        public DateTime? LtcEndDt { get; set; }
        [Required]
        [Column("IS_SECTION_16")]
        public bool? IsSection16 { get; set; }
        [Column("SECTION_16_AREA")]
        public float? Section16Area { get; set; }
        [Column("SECTION_16_END_DT", TypeName = "date")]
        public DateTime? Section16EndDt { get; set; }
        [Required]
        [Column("IS_SURPLUS_SEVERANCE")]
        public bool? IsSurplusSeverance { get; set; }
        [Column("SURPLUS_SEVERANCE_AREA")]
        public float? SurplusSeveranceArea { get; set; }
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
        [InverseProperty(nameof(PimsAreaUnitType.PimsTakes))]
        public virtual PimsAreaUnitType AreaUnitTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropertyAcquisitionFileId))]
        [InverseProperty(nameof(PimsPropertyAcquisitionFile.PimsTakes))]
        public virtual PimsPropertyAcquisitionFile PropertyAcquisitionFile { get; set; }
        [ForeignKey(nameof(TakeSiteContamTypeCode))]
        [InverseProperty(nameof(PimsTakeSiteContamType.PimsTakes))]
        public virtual PimsTakeSiteContamType TakeSiteContamTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(TakeStatusTypeCode))]
        [InverseProperty(nameof(PimsTakeStatusType.PimsTakes))]
        public virtual PimsTakeStatusType TakeStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(TakeTypeCode))]
        [InverseProperty(nameof(PimsTakeType.PimsTakes))]
        public virtual PimsTakeType TakeTypeCodeNavigation { get; set; }
    }
}
