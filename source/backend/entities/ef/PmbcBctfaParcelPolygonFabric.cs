using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

[Keyless]
public partial class PmbcBctfaParcelPolygonFabric
{
    [Column("PID")]
    public int Pid { get; set; }

    [Column("IS_BCTFA_OWNED")]
    public bool? IsBctfaOwned { get; set; }

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

    [Column("PARCEL_FABRIC_POLY_ID")]
    public int ParcelFabricPolyId { get; set; }

    [Column("GLOBAL_UID")]
    [StringLength(254)]
    public string GlobalUid { get; set; }

    [Column("PARCEL_NAME")]
    [StringLength(50)]
    public string ParcelName { get; set; }

    [Column("PLAN_ID")]
    public int? PlanId { get; set; }

    [Column("PLAN_NUMBER")]
    [StringLength(128)]
    public string PlanNumber { get; set; }

    [Column("PIN")]
    public int? Pin { get; set; }

    [Column("PID_NUMBER")]
    public int? PidNumber { get; set; }

    [Column("SOURCE_PARCEL_ID")]
    [StringLength(50)]
    public string SourceParcelId { get; set; }

    [Column("PARCEL_STATUS")]
    [StringLength(20)]
    public string ParcelStatus { get; set; }

    [Column("PARCEL_CLASS")]
    [StringLength(50)]
    public string ParcelClass { get; set; }

    [Column("OWNER_TYPE")]
    [StringLength(50)]
    public string OwnerType { get; set; }

    [Column("PARCEL_START_DATE", TypeName = "datetime")]
    public DateTime? ParcelStartDate { get; set; }

    [Column("SURVEY_DESIGNATION_1")]
    [StringLength(30)]
    public string SurveyDesignation1 { get; set; }

    [Column("SURVEY_DESIGNATION_2")]
    [StringLength(30)]
    public string SurveyDesignation2 { get; set; }

    [Column("SURVEY_DESIGNATION_3")]
    [StringLength(30)]
    public string SurveyDesignation3 { get; set; }

    [Column("LEGAL_DESCRIPTION")]
    [StringLength(2000)]
    public string LegalDescription { get; set; }

    [Column("MUNICIPALITY")]
    [StringLength(254)]
    public string Municipality { get; set; }

    [Column("REGIONAL_DISTRICT")]
    [StringLength(50)]
    public string RegionalDistrict { get; set; }

    [Column("IS_REMAINDER_IND")]
    [StringLength(3)]
    public string IsRemainderInd { get; set; }

    [Column("GEOMETRY_SOURCE")]
    [StringLength(50)]
    public string GeometrySource { get; set; }

    [Column("POSITIONAL_ERROR")]
    public long? PositionalError { get; set; }

    [Column("ERROR_REPORTED_BY")]
    [StringLength(50)]
    public string ErrorReportedBy { get; set; }

    [Column("CAPTURE_METHOD")]
    [StringLength(50)]
    public string CaptureMethod { get; set; }

    [Column("COMPILED_IND")]
    [StringLength(5)]
    public string CompiledInd { get; set; }

    [Column("STATED_AREA")]
    [StringLength(50)]
    public string StatedArea { get; set; }

    [Column("WHEN_CREATED", TypeName = "datetime")]
    public DateTime? WhenCreated { get; set; }

    [Column("WHEN_UPDATED", TypeName = "datetime")]
    public DateTime? WhenUpdated { get; set; }

    [Column("FEATURE_AREA_SQM")]
    public long? FeatureAreaSqm { get; set; }

    [Column("FEATURE_LENGTH_M")]
    public long? FeatureLengthM { get; set; }

    [Column("SHAPE", TypeName = "geometry")]
    public Geometry Shape { get; set; }

    [Column("OBJECTID", TypeName = "numeric(38, 0)")]
    public decimal? Objectid { get; set; }

    [Column("SE_ANNO_CAD_DATA")]
    public byte[] SeAnnoCadData { get; set; }
}
