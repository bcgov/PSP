using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_FOLIO_DESCRIPTION")]
    [Index(nameof(RollNumber), Name = "BCAFDE_ROLL_NUMBER_IDX")]
    public partial class BcaFolioDescription
    {
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("NEIGHBOURHOOD_CODE")]
        [StringLength(16)]
        public string NeighbourhoodCode { get; set; }
        [Column("NEIGHBOURHOOD_DESCRIPTION")]
        [StringLength(255)]
        public string NeighbourhoodDescription { get; set; }
        [Column("ACTUAL_USE_CODE")]
        [StringLength(16)]
        public string ActualUseCode { get; set; }
        [Column("ACTUAL_USE_DESCRIPTION")]
        [StringLength(255)]
        public string ActualUseDescription { get; set; }
        [Column("VACANT_FLAG")]
        public bool? VacantFlag { get; set; }
        [Column("BCTRANSIT_FLAG")]
        public bool? BctransitFlag { get; set; }
        [Column("POLICE_TAX_FLAG")]
        public bool? PoliceTaxFlag { get; set; }
        [Column("ADD_SCHOOL_TAX_3M_TO_4M_FLAG")]
        public bool? AddSchoolTax3mTo4mFlag { get; set; }
        [Column("ADD_SCHOOL_TAX_GREATER_4M_FLAG")]
        public bool? AddSchoolTaxGreater4mFlag { get; set; }
        [Column("CANDIDATE_FOR_SPEC_TAX_FLAG")]
        public bool? CandidateForSpecTaxFlag { get; set; }
        [Column("ALR_CODE")]
        [StringLength(16)]
        public string AlrCode { get; set; }
        [Column("ALR_DESCRIPTION")]
        [StringLength(255)]
        public string AlrDescription { get; set; }
        [Column("TENURE_CODE")]
        [StringLength(16)]
        public string TenureCode { get; set; }
        [Column("TENURE_DESCRIPTION")]
        [StringLength(255)]
        public string TenureDescription { get; set; }
        [Column("PARKING_AREA")]
        [StringLength(255)]
        public string ParkingArea { get; set; }
        [Column("LAND_DIMENSION_TYPE")]
        [StringLength(255)]
        public string LandDimensionType { get; set; }
        [Column("LAND_DIMENSION_TYPE_DESCRIPTION")]
        [StringLength(255)]
        public string LandDimensionTypeDescription { get; set; }
        [Column("LAND_DIMENSION")]
        [StringLength(255)]
        public string LandDimension { get; set; }
        [Column("LAND_WIDTH")]
        [StringLength(255)]
        public string LandWidth { get; set; }
        [Column("LAND_DEPTH")]
        [StringLength(255)]
        public string LandDepth { get; set; }
        [Column("SCHOOL_DISTRICT_CODE")]
        [StringLength(16)]
        public string SchoolDistrictCode { get; set; }
        [Column("SCHOOL_DISTRICT_DESCRIPTION")]
        [StringLength(255)]
        public string SchoolDistrictDescription { get; set; }
        [Column("REGIONAL_DISTRICT_CODE")]
        [StringLength(16)]
        public string RegionalDistrictCode { get; set; }
        [Column("REGIONAL_DISTRICT_DESCRIPTION")]
        [StringLength(255)]
        public string RegionalDistrictDescription { get; set; }
        [Column("REGIONAL_HOSPITAL_DISTRICT_CODE")]
        [StringLength(16)]
        public string RegionalHospitalDistrictCode { get; set; }
        [Column("REGIONAL_HOSPITAL_DISTRICT_DESCRIPTION")]
        [StringLength(255)]
        public string RegionalHospitalDistrictDescription { get; set; }
        [Column("PREDOMINANT_MANUAL_CLASS_CODE")]
        [StringLength(16)]
        public string PredominantManualClassCode { get; set; }
        [Column("PREDOMINANT_MANUAL_CLASS_DESCRIPTION")]
        [StringLength(255)]
        public string PredominantManualClassDescription { get; set; }
        [Column("PREDOMINANT_PERCENT_DEVIATION", TypeName = "decimal(9, 3)")]
        public decimal? PredominantPercentDeviation { get; set; }
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

        [ForeignKey(nameof(RollNumber))]
        public virtual BcaFolioRecord RollNumberNavigation { get; set; }
    }
}
