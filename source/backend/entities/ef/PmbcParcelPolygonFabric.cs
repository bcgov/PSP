using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

/// <summary>
/// The ParcelMap BC (PMBC) parcel fabric contains all active titled parcels and surveyed provincial Crown land parcels in BC. For building strata parcels, there is a record, with PID value, for each parcel within the strata parcel; the geometry for those records is the geometry for the overall strata. This dataset is polygonal and contains all parcel attributes.
/// </summary>
[Table("PMBC_PARCEL_POLYGON_FABRIC", Schema = "pmbc")]
[Index("PidNumber", Name = "PLYFAB_PID_NUMBER_IDX")]
[Index("PlanNumber", Name = "PLYFAB_PLAN_NUMBER_IDX")]
[Index("Shape", Name = "PLYFAB_SHAPE_IDX")]
public partial class PmbcParcelPolygonFabric
{
    /// <summary>
    /// PARCEL_FABRIC_POLY_ID is a system generated unique identification number.
    /// </summary>
    [Key]
    [Column("PARCEL_FABRIC_POLY_ID")]
    public int ParcelFabricPolyId { get; set; }

    /// <summary>
    /// GLOBAL_UID is a unique global identifier (GUID) for the parcel.
    /// </summary>
    [Column("GLOBAL_UID")]
    [StringLength(254)]
    public string GlobalUid { get; set; }

    /// <summary>
    /// PARCEL_NAME is the same as the PID, if there is one. If there is a PIN but no PID, then PARCEL_NAME is the PIN. If there is no PID nor PIN, then PARCEL_NAME is the parcel class value, e.g., COMMON OWNERSHIP, BUILDING STRATA, AIR SPACE, ROAD, PARK.
    /// </summary>
    [Column("PARCEL_NAME")]
    [StringLength(50)]
    public string ParcelName { get; set; }

    /// <summary>
    /// PLAN_ID is the unique identifier of the land survey plan that corresponds to this parcel.
    /// </summary>
    [Column("PLAN_ID")]
    public int? PlanId { get; set; }

    /// <summary>
    /// PLAN_NUMBER is the Land Act, Land Title Act, or Strata Property Act Plan Number for the land survey plan that corresponds to this parcel, e.g., VIP1632, NO_PLAN.
    /// </summary>
    [Column("PLAN_NUMBER")]
    [StringLength(128)]
    public string PlanNumber { get; set; }

    /// <summary>
    /// PIN is the Crown Land Registry Parcel Identifier, if applicable.
    /// </summary>
    [Column("PIN")]
    public int? Pin { get; set; }

    /// <summary>
    /// PID is the Land Title Register parcel identifier, a left-zero-padded nine-digit number that uniquely identifies a parcel in the land title register of in British Columbia. The registrar assigns PID numbers to parcels for which a title is being entered as a registered title. The Land Title Act refers to the PID as the permanent parcel identifier.
    /// </summary>
    [Column("PID")]
    [StringLength(9)]
    public string Pid { get; set; }

    /// <summary>
    /// PID_NUMBER is the PID, without leading zeroes. PID is the Land Title Register parcel identifier, a nine-digit number that uniquely identifies a parcel in the land title register of in British Columbia. The registrar assigns PID numbers to parcels for which a title is being entered as a registered title. The Land Title Act refers to the PID as the permanent parcel identifier.
    /// </summary>
    [Column("PID_NUMBER")]
    public int? PidNumber { get; set; }

    /// <summary>
    /// SOURCE_PARCEL_ID is the unique parcel identifier supplied by the source data provider. The value is intended to assist local governments by providing traceability back to the source data provider&apos;s parcel ID. It will not be populated during on-going operations, e.g., DATA_NEW, CPG000050, 999, 01762.146.
    /// </summary>
    [Column("SOURCE_PARCEL_ID")]
    [StringLength(50)]
    public string SourceParcelId { get; set; }

    /// <summary>
    /// PARCEL_STATUS is the status of the parcel, according to the Land Title Register or Crown Land Registry, as appropriate, i.e., ACTIVE, CANCELLED, INACTIVE, PENDING.
    /// </summary>
    [Column("PARCEL_STATUS")]
    [StringLength(20)]
    public string ParcelStatus { get; set; }

    /// <summary>
    /// PARCEL_CLASS is the Parcel classification for maintenance, mapping, publishing and analysis, i.e., PRIMARY, SUBDIVISION, PART OF PRIMARY, BUILDING STRATA, BARE LAND STRATA, AIR SPACE, ROAD, HIGHWAY, PARK, INTEREST, COMMON OWNERSHIP, ABSOLUTE FEE BOOK, CROWN SUBDIVISION, RETURN TO CROWN.
    /// </summary>
    [Column("PARCEL_CLASS")]
    [StringLength(50)]
    public string ParcelClass { get; set; }

    /// <summary>
    /// OWNER_TYPE is the general ownership category, e.g., PRIVATE, CROWN PROVINCIAL, MUNICIPAL.
    /// </summary>
    [Column("OWNER_TYPE")]
    [StringLength(50)]
    public string OwnerType { get; set; }

    /// <summary>
    /// PARCEL_START_DATE is the date of the legal event that created the parcel, i.e., the date the plan was filed.
    /// </summary>
    [Column("PARCEL_START_DATE", TypeName = "datetime")]
    public DateTime? ParcelStartDate { get; set; }

    /// <summary>
    /// SURVEY_DESIGNATION_1 is, typically, the smallest division of lands in a survey. If available, this is generally the Parcel from a Subdivided short legal description or the Quadrant or Block from an Unsubdivided short legal description, e.g., PARCEL A, SW4.
    /// </summary>
    [Column("SURVEY_DESIGNATION_1")]
    [StringLength(30)]
    public string SurveyDesignation1 { get; set; }

    /// <summary>
    /// SURVEY_DESIGNATION_2 is, typically, the second smallest division of lands in a survey. If available, this is generally the Lot from a Subdivided short legal description or the District Lot, Lot or Section from an Unsubdivided short legal description, e.g., LOT 2.
    /// </summary>
    [Column("SURVEY_DESIGNATION_2")]
    [StringLength(30)]
    public string SurveyDesignation2 { get; set; }

    /// <summary>
    /// SURVEY_DESIGNATION_3 is, typically, the third smallest division of lands in a survey. If available, this is generally the Block from a Subdivided short legal description or the Range from an Unsubdivided short legal description, e.g., BLOCK H.
    /// </summary>
    [Column("SURVEY_DESIGNATION_3")]
    [StringLength(30)]
    public string SurveyDesignation3 { get; set; }

    /// <summary>
    /// LEGAL_DESCRIPTION is the full legal description of the parcel and is primarily recorded from the Land Title Register. Where recorded only in the Crown Land Registry, this attribute is to be populated from Tantalis for the fabric compilation, but maintained by PMBC during on-going operations.
    /// </summary>
    [Column("LEGAL_DESCRIPTION")]
    [StringLength(2000)]
    public string LegalDescription { get; set; }

    /// <summary>
    /// MUNICIPALITY is the municipal area within which the parcel is located. The value is either RURAL (for parcels in unincorporated regions) or the name of a BC municipality.
    /// </summary>
    [Column("MUNICIPALITY")]
    [StringLength(254)]
    public string Municipality { get; set; }

    /// <summary>
    /// REGIONAL_DISTRICT is the name of the regional district in which the parcel is located, e.g., CAPITAL REGIONAL DISTRICT.
    /// </summary>
    [Column("REGIONAL_DISTRICT")]
    [StringLength(50)]
    public string RegionalDistrict { get; set; }

    /// <summary>
    /// IS_REMAINDER_IND indicates if the parcel is a remainder of the original, i.e., YES, NO.
    /// </summary>
    [Column("IS_REMAINDER_IND")]
    [StringLength(3)]
    public string IsRemainderInd { get; set; }

    /// <summary>
    /// GEOMETRY_SOURCE is the source of the parcel geometry data, e.g., ICIS CADASTRE, ICF, PMBC OPERATIONS.
    /// </summary>
    [Column("GEOMETRY_SOURCE")]
    [StringLength(50)]
    public string GeometrySource { get; set; }

    /// <summary>
    /// POSITIONAL_ERROR is the semi-major axis at the 95% confidence level of the least accurate point of the parcel.
    /// </summary>
    [Column("POSITIONAL_ERROR")]
    public long? PositionalError { get; set; }

    /// <summary>
    /// ERROR_REPORTED_BY is the organization or process reporting the error, i.e., LSA, DATA COMPILATION.
    /// </summary>
    [Column("ERROR_REPORTED_BY")]
    [StringLength(50)]
    public string ErrorReportedBy { get; set; }

    /// <summary>
    /// CAPTURE_METHOD is an indicator of relative accuracy, i.e., UNKNOWN, COGO, SURVEY PLAN DATASET.
    /// </summary>
    [Column("CAPTURE_METHOD")]
    [StringLength(50)]
    public string CaptureMethod { get; set; }

    /// <summary>
    /// COMPILED_IND indicates if the parcel polygon was generated from inverted dimensions, i.e., True, False. &quot;True&quot; means that the parcel geometry was from a previous source cadastre and not precision input based on plan dimensions.
    /// </summary>
    [Column("COMPILED_IND")]
    [StringLength(5)]
    public string CompiledInd { get; set; }

    /// <summary>
    /// STATED_AREA is the area of the parcel, in square metres. It is automatically calculated if misclose is small; it can be edited to reflect the recorded plan value.
    /// </summary>
    [Column("STATED_AREA")]
    [StringLength(50)]
    public string StatedArea { get; set; }

    /// <summary>
    /// WHEN_CREATED is the date and time the source record was created (not the time when it was loaded into the BC Geographic Warehouse).
    /// </summary>
    [Column("WHEN_CREATED", TypeName = "datetime")]
    public DateTime? WhenCreated { get; set; }

    /// <summary>
    /// WHEN_UPDATED is the date and time the source record was last modified (not the time when it was loaded into, or modified in, the BC Geographic Warehouse).
    /// </summary>
    [Column("WHEN_UPDATED", TypeName = "datetime")]
    public DateTime? WhenUpdated { get; set; }

    /// <summary>
    /// FEATURE_AREA_SQM is the system calculated area of a two-dimensional polygon in square meters.
    /// </summary>
    [Column("FEATURE_AREA_SQM")]
    public long? FeatureAreaSqm { get; set; }

    /// <summary>
    /// FEATURE_LENGTH_M is the system calculated length or perimeter of a geometry in meters.
    /// </summary>
    [Column("FEATURE_LENGTH_M")]
    public long? FeatureLengthM { get; set; }

    /// <summary>
    /// SHAPE is the column used to reference the spatial coordinates defining the feature.
    /// </summary>
    [Column("SHAPE", TypeName = "geometry")]
    public Geometry Shape { get; set; }

    /// <summary>
    /// OBJECTID is a column required by spatial layers that interact with ESRI ArcSDE. It is populated with unique values automatically by SDE.
    /// </summary>
    [Column("OBJECTID", TypeName = "numeric(38, 0)")]
    public decimal? Objectid { get; set; }

    /// <summary>
    /// SE_ANNO_CAD_DATA is a binary column used by spatial tools to store annotation, curve features and CAD data when using the SDO_GEOMETRY storage data type.
    /// </summary>
    [Column("SE_ANNO_CAD_DATA")]
    public byte[] SeAnnoCadData { get; set; }
}
