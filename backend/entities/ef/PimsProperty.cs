﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY")]
    [Index(nameof(AddressId), Name = "PRPRTY_ADDRESS_ID_IDX")]
    [Index(nameof(DistrictCode), Name = "PRPRTY_DISTRICT_CODE_IDX")]
    [Index(nameof(PropertyAreaUnitTypeCode), Name = "PRPRTY_PROPERTY_AREA_UNIT_TYPE_CODE_IDX")]
    [Index(nameof(PropertyClassificationTypeCode), Name = "PRPRTY_PROPERTY_CLASSIFICATION_TYPE_CODE_IDX")]
    [Index(nameof(PropertyDataSourceTypeCode), Name = "PRPRTY_PROPERTY_DATA_SOURCE_TYPE_CODE_IDX")]
    [Index(nameof(PropertyManagerId), Name = "PRPRTY_PROPERTY_MANAGER_ID_IDX")]
    [Index(nameof(PropertyStatusTypeCode), Name = "PRPRTY_PROPERTY_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(PropertyTenureTypeCode), Name = "PRPRTY_PROPERTY_TENURE_TYPE_CODE_IDX")]
    [Index(nameof(PropertyTypeCode), Name = "PRPRTY_PROPERTY_TYPE_CODE_IDX")]
    [Index(nameof(PropMgmtOrgId), Name = "PRPRTY_PROP_MGMT_ORG_ID_IDX")]
    [Index(nameof(RegionCode), Name = "PRPRTY_REGION_CODE_IDX")]
    [Index(nameof(SurplusDeclarationTypeCode), Name = "PRPRTY_SURPLUS_DECLARATION_TYPE_CODE_IDX")]
    public partial class PimsProperty
    {
        public PimsProperty()
        {
            PimsProjectProperties = new HashSet<PimsProjectProperty>();
            PimsPropertyActivities = new HashSet<PimsPropertyActivity>();
            PimsPropertyEvaluations = new HashSet<PimsPropertyEvaluation>();
            PimsPropertyLeases = new HashSet<PimsPropertyLease>();
            PimsPropertyOrganizations = new HashSet<PimsPropertyOrganization>();
            PimsPropertyPropertyServiceFiles = new HashSet<PimsPropertyPropertyServiceFile>();
            PimsPropertyTaxes = new HashSet<PimsPropertyTax>();
        }

        [Key]
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
        [Column("LAND_LEGAL_DESCRIPTION")]
        public string LandLegalDescription { get; set; }
        [Column("BOUNDARY", TypeName = "geometry")]
        public Geometry Boundary { get; set; }
        [Column("LOCATION", TypeName = "geometry")]
        public Geometry Location { get; set; }
        [Column("ENCUMBRANCE_REASON")]
        [StringLength(500)]
        public string EncumbranceReason { get; set; }
        [Column("SURPLUS_DECLARATION_COMMENT")]
        [StringLength(2000)]
        public string SurplusDeclarationComment { get; set; }
        [Column("SURPLUS_DECLARATION_DATE", TypeName = "datetime")]
        public DateTime? SurplusDeclarationDate { get; set; }
        [Required]
        [Column("IS_OWNED")]
        public bool? IsOwned { get; set; }
        [Required]
        [Column("IS_PROPERTY_OF_INTEREST")]
        public bool? IsPropertyOfInterest { get; set; }
        [Required]
        [Column("IS_VISIBLE_TO_OTHER_AGENCIES")]
        public bool? IsVisibleToOtherAgencies { get; set; }
        [Required]
        [Column("IS_SENSITIVE")]
        public bool? IsSensitive { get; set; }
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

        [ForeignKey(nameof(AddressId))]
        [InverseProperty(nameof(PimsAddress.PimsProperties))]
        public virtual PimsAddress Address { get; set; }
        [ForeignKey(nameof(DistrictCode))]
        [InverseProperty(nameof(PimsDistrict.PimsProperties))]
        public virtual PimsDistrict DistrictCodeNavigation { get; set; }
        [ForeignKey(nameof(PropMgmtOrgId))]
        [InverseProperty(nameof(PimsOrganization.PimsProperties))]
        public virtual PimsOrganization PropMgmtOrg { get; set; }
        [ForeignKey(nameof(PropertyAreaUnitTypeCode))]
        [InverseProperty(nameof(PimsAreaUnitType.PimsProperties))]
        public virtual PimsAreaUnitType PropertyAreaUnitTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropertyClassificationTypeCode))]
        [InverseProperty(nameof(PimsPropertyClassificationType.PimsProperties))]
        public virtual PimsPropertyClassificationType PropertyClassificationTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropertyDataSourceTypeCode))]
        [InverseProperty(nameof(PimsDataSourceType.PimsProperties))]
        public virtual PimsDataSourceType PropertyDataSourceTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropertyManagerId))]
        [InverseProperty(nameof(PimsPerson.PimsProperties))]
        public virtual PimsPerson PropertyManager { get; set; }
        [ForeignKey(nameof(PropertyStatusTypeCode))]
        [InverseProperty(nameof(PimsPropertyStatusType.PimsProperties))]
        public virtual PimsPropertyStatusType PropertyStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropertyTenureTypeCode))]
        [InverseProperty(nameof(PimsPropertyTenureType.PimsProperties))]
        public virtual PimsPropertyTenureType PropertyTenureTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(PropertyTypeCode))]
        [InverseProperty(nameof(PimsPropertyType.PimsProperties))]
        public virtual PimsPropertyType PropertyTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(RegionCode))]
        [InverseProperty(nameof(PimsRegion.PimsProperties))]
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        [ForeignKey(nameof(SurplusDeclarationTypeCode))]
        [InverseProperty(nameof(PimsSurplusDeclarationType.PimsProperties))]
        public virtual PimsSurplusDeclarationType SurplusDeclarationTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsProjectProperty.Property))]
        public virtual ICollection<PimsProjectProperty> PimsProjectProperties { get; set; }
        [InverseProperty(nameof(PimsPropertyActivity.Property))]
        public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; }
        [InverseProperty(nameof(PimsPropertyEvaluation.Property))]
        public virtual ICollection<PimsPropertyEvaluation> PimsPropertyEvaluations { get; set; }
        [InverseProperty(nameof(PimsPropertyLease.Property))]
        public virtual ICollection<PimsPropertyLease> PimsPropertyLeases { get; set; }
        [InverseProperty(nameof(PimsPropertyOrganization.Property))]
        public virtual ICollection<PimsPropertyOrganization> PimsPropertyOrganizations { get; set; }
        [InverseProperty(nameof(PimsPropertyPropertyServiceFile.Property))]
        public virtual ICollection<PimsPropertyPropertyServiceFile> PimsPropertyPropertyServiceFiles { get; set; }
        [InverseProperty(nameof(PimsPropertyTax.Property))]
        public virtual ICollection<PimsPropertyTax> PimsPropertyTaxes { get; set; }
    }
}
