using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    public partial class PimsPropertyBoundaryVw
    {
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }
        [Column("PID")]
        public int? Pid { get; set; }
        [Column("PID_PADDED")]
        [StringLength(9)]
        public string PidPadded { get; set; }
        [Column("PIN")]
        public int? Pin { get; set; }
        [Required]
        [Column("PROPERTY_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyTypeCode { get; set; }
        [Required]
        [Column("PROPERTY_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyStatusTypeCode { get; set; }
        [Required]
        [Column("PROPERTY_DATA_SOURCE_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyDataSourceTypeCode { get; set; }
        [Column("PROPERTY_DATA_SOURCE_EFFECTIVE_DATE", TypeName = "date")]
        public DateTime PropertyDataSourceEffectiveDate { get; set; }
        [Required]
        [Column("PROPERTY_CLASSIFICATION_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyClassificationTypeCode { get; set; }
        [Column("PROPERTY_TENURE_TYPE_CODE")]
        [StringLength(4000)]
        public string PropertyTenureTypeCode { get; set; }
        [Column("STREET_ADDRESS_1")]
        [StringLength(200)]
        public string StreetAddress1 { get; set; }
        [Column("STREET_ADDRESS_2")]
        [StringLength(200)]
        public string StreetAddress2 { get; set; }
        [Column("STREET_ADDRESS_3")]
        [StringLength(200)]
        public string StreetAddress3 { get; set; }
        [Column("MUNICIPALITY_NAME")]
        [StringLength(200)]
        public string MunicipalityName { get; set; }
        [Column("POSTAL_CODE")]
        [StringLength(20)]
        public string PostalCode { get; set; }
        [Column("PROVINCE_STATE_CODE")]
        [StringLength(20)]
        public string ProvinceStateCode { get; set; }
        [Column("PROVINCE_NAME")]
        [StringLength(200)]
        public string ProvinceName { get; set; }
        [Column("COUNTRY_CODE")]
        [StringLength(20)]
        public string CountryCode { get; set; }
        [Column("COUNTRY_NAME")]
        [StringLength(200)]
        public string CountryName { get; set; }
        [Column("NAME")]
        [StringLength(250)]
        public string Name { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(2000)]
        public string Description { get; set; }
        [Column("ADDRESS_ID")]
        public long? AddressId { get; set; }
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Column("DISTRICT_CODE")]
        public short DistrictCode { get; set; }
        [Column("GEOMETRY", TypeName = "geometry")]
        public Geometry Geometry { get; set; }
        [Column("PROPERTY_AREA_UNIT_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyAreaUnitTypeCode { get; set; }
        [Column("LAND_AREA")]
        public float? LandArea { get; set; }
        [Column("LAND_LEGAL_DESCRIPTION")]
        public string LandLegalDescription { get; set; }
        [Column("SURVEY_PLAN_NUMBER")]
        [StringLength(250)]
        public string SurveyPlanNumber { get; set; }
        [Column("ENCUMBRANCE_REASON")]
        [StringLength(500)]
        public string EncumbranceReason { get; set; }
        [Column("IS_SENSITIVE")]
        public bool IsSensitive { get; set; }
        [Column("IS_OWNED")]
        public bool IsOwned { get; set; }
        [Column("IS_PROPERTY_OF_INTEREST")]
        public bool IsPropertyOfInterest { get; set; }
        [Column("IS_VISIBLE_TO_OTHER_AGENCIES")]
        public bool IsVisibleToOtherAgencies { get; set; }
        [Column("ZONING")]
        [StringLength(50)]
        public string Zoning { get; set; }
        [Column("ZONING_POTENTIAL")]
        [StringLength(50)]
        public string ZoningPotential { get; set; }
        [Column("IS_PAYABLE_LEASE")]
        public bool? IsPayableLease { get; set; }
    }
}
