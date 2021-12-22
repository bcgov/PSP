﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY_HIST")]
    [Index(nameof(PropertyHistId), nameof(EndDateHist), Name = "PIMS_PRPRTY_H_UK", IsUnique = true)]
    public partial class PimsPropertyHist
    {
        [Key]
        [Column("_PROPERTY_HIST_ID")]
        public long PropertyHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }
        [Column("PROPERTY_MANAGER_ID")]
        public long? PropertyManagerId { get; set; }
        [Column("PROP_MGMT_ORG_ID")]
        public long? PropMgmtOrgId { get; set; }
        [Required]
        [Column("PROPERTY_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyTypeCode { get; set; }
        [Required]
        [Column("PROPERTY_CLASSIFICATION_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyClassificationTypeCode { get; set; }
        [Column("ADDRESS_ID")]
        public long AddressId { get; set; }
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Column("DISTRICT_CODE")]
        public short DistrictCode { get; set; }
        [Required]
        [Column("PROPERTY_TENURE_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyTenureTypeCode { get; set; }
        [Required]
        [Column("PROPERTY_AREA_UNIT_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyAreaUnitTypeCode { get; set; }
        [Required]
        [Column("PROPERTY_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyStatusTypeCode { get; set; }
        [Required]
        [Column("SURPLUS_DECLARATION_TYPE_CODE")]
        [StringLength(20)]
        public string SurplusDeclarationTypeCode { get; set; }
        [Required]
        [Column("PROPERTY_DATA_SOURCE_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyDataSourceTypeCode { get; set; }
        [Column("PROPERTY_DATA_SOURCE_EFFECTIVE_DATE", TypeName = "date")]
        public DateTime PropertyDataSourceEffectiveDate { get; set; }
        [Column("NAME")]
        [StringLength(250)]
        public string Name { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(2000)]
        public string Description { get; set; }
        [Column("PID")]
        public int? Pid { get; set; }
        [Column("PIN")]
        public int? Pin { get; set; }
        [Column("LAND_AREA")]
        public float? LandArea { get; set; }
        [Column("ENCUMBRANCE_REASON")]
        [StringLength(500)]
        public string EncumbranceReason { get; set; }
        [Column("SURPLUS_DECLARATION_COMMENT")]
        [StringLength(2000)]
        public string SurplusDeclarationComment { get; set; }
        [Column("SURPLUS_DECLARATION_DATE", TypeName = "datetime")]
        public DateTime? SurplusDeclarationDate { get; set; }
        [Column("IS_OWNED")]
        public bool IsOwned { get; set; }
        [Column("IS_PROPERTY_OF_INTEREST")]
        public bool IsPropertyOfInterest { get; set; }
        [Column("IS_VISIBLE_TO_OTHER_AGENCIES")]
        public bool IsVisibleToOtherAgencies { get; set; }
        [Column("IS_SENSITIVE")]
        public bool IsSensitive { get; set; }
        [Column("ZONING")]
        [StringLength(50)]
        public string Zoning { get; set; }
        [Column("ZONING_POTENTIAL")]
        [StringLength(50)]
        public string ZoningPotential { get; set; }
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
