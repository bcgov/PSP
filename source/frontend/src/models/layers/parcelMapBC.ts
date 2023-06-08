import { FeatureCollection, GeoJsonProperties, Geometry, Polygon } from 'geojson';
import polylabel from 'polylabel';

export type stringDate = string;

// ParcelMap BC Parcel Fabric - Fully Attributed
// Source : https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric-fully-attributed
// Api: WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW

export interface PMBC_FullyAttributed_Feature_Properties2 {
  readonly PARCEL_FABRIC_POLY_ID: number | null;
  readonly GLOBAL_UID: string | null;
  readonly PARCEL_NAME: string | null;
  readonly PLAN_ID: number | null;
  readonly PLAN_NUMBER: string | null;
  readonly PIN: number | null;
  readonly PID: string | null;
  readonly PID_FORMATTED: string | null;
  readonly PID_NUMBER: number | null;
  readonly SOURCE_PARCEL_ID: string | null;
  readonly PARCEL_STATUS: string | null;
  readonly PARCEL_CLASS: string | null;
  readonly OWNER_TYPE: string | null;
  readonly PARCEL_START_DATE: stringDate | null;
  readonly SURVEY_DESIGNATION_1: string | null;
  readonly SURVEY_DESIGNATION_2: string | null;
  readonly SURVEY_DESIGNATION_3: string | null;
  readonly LEGAL_DESCRIPTION: string | null;
  readonly MUNICIPALITY: string | null;
  readonly REGIONAL_DISTRICT: string | null;
  readonly IS_REMAINDER_IND: string | null;
  readonly GEOMETRY_SOURCE: string | null;
  readonly POSITIONAL_ERROR: number | null;
  readonly ERROR_REPORTED_BY: string | null;
  readonly CAPTURE_METHOD: string | null;
  readonly COMPILED_IND: string | null;
  readonly STATED_AREA: string | null;
  readonly WHEN_CREATED: stringDate | null;
  readonly WHEN_UPDATED: stringDate | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
  //readonly SHAPE: sdo_geometry | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
}

interface PMBC_FullyAttributed_Feature_Properties {
  readonly parcelFabricPolyID: number | null;
  readonly globalUid: string | null;
  readonly parcelName: string | null;
  readonly planID: number | null;
  readonly planNumber: string | null;
  readonly pin: number | null;
  readonly pid: string | null;
  readonly pidFormatted: string | null;
  readonly pidNumber: number | null;
  readonly sourceParcelID: string | null;
  readonly parcelStatus: string | null;
  readonly parcelClass: string | null;
  readonly ownerType: string | null;
  readonly parcelStartDate: stringDate | null;
  readonly surveyDesignation1: string | null;
  readonly surveyDesignation2: string | null;
  readonly surveyDesignation3: string | null;
  readonly legalDescription: string | null;
  readonly municipality: string | null;
  readonly regionalDistrict: string | null;
  readonly isRemainderInd: string | null;
  readonly geometrySource: string | null;
  readonly positionalError: number | null;
  readonly errorReportedBy: string | null;
  readonly captureMethod: string | null;
  readonly compiledInd: string | null;
  readonly statedArea: string | null;
  readonly whenCreated: stringDate | null;
  readonly whenUpdated: stringDate | null;
  readonly featureAreaSqm: number | null;
  readonly featureLengthM: number | null;
  //readonly shape: sdo_geometry | null;
  readonly objectid: number | null;
  readonly seAnnoCadData: Blob | null;
}

// Extension to contain latitude and longitude
/*export interface PMBC_FullyAttributed_Property extends PMBC_FullyAttributed_Feature_Properties {
  readonly latitude: number;
  readonly longitude: number;
}*/

const propertyFromFeatureCollection = (
  values: FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties2> | undefined,
): PMBC_FullyAttributed_Feature_Properties[] | undefined =>
  values?.features
    ?.filter(feature => feature?.geometry?.type === 'Polygon')
    .map((feature): PMBC_FullyAttributed_Feature_Properties | undefined => {
      const boundedCenter = polylabel((feature.geometry as Polygon).coordinates);
      return fromProperties(feature.properties);
    })
    .filter((x): x is PMBC_FullyAttributed_Feature_Properties => !!x);

function fromProperties(
  properties: PMBC_FullyAttributed_Feature_Properties2,
): PMBC_FullyAttributed_Feature_Properties | undefined {
  if (properties === null || properties === undefined) {
    return undefined;
  }

  return {
    parcelFabricPolyID: properties.PARCEL_FABRIC_POLY_ID,
    globalUid: properties.GLOBAL_UID,
    parcelName: properties.PARCEL_NAME,
    planID: properties.PLAN_ID,
    planNumber: properties.PLAN_NUMBER,
    pin: properties.PIN,
    pid: properties.PID,
    pidFormatted: properties.PID_FORMATTED,
    pidNumber: properties.PID_NUMBER,
    sourceParcelID: properties.SOURCE_PARCEL_ID,
    parcelStatus: properties.PARCEL_STATUS,
    parcelClass: properties.PARCEL_CLASS,
    ownerType: properties.OWNER_TYPE,
    parcelStartDate: properties.PARCEL_START_DATE,
    surveyDesignation1: properties.SURVEY_DESIGNATION_1,
    surveyDesignation2: properties.SURVEY_DESIGNATION_2,
    surveyDesignation3: properties.SURVEY_DESIGNATION_3,
    legalDescription: properties.LEGAL_DESCRIPTION,
    municipality: properties.MUNICIPALITY,
    regionalDistrict: properties.REGIONAL_DISTRICT,
    isRemainderInd: properties.IS_REMAINDER_IND,
    geometrySource: properties.GEOMETRY_SOURCE,
    positionalError: properties.POSITIONAL_ERROR,
    errorReportedBy: properties.ERROR_REPORTED_BY,
    captureMethod: properties.CAPTURE_METHOD,
    compiledInd: properties.COMPILED_IND,
    statedArea: properties.STATED_AREA,
    whenCreated: properties.WHEN_CREATED,
    whenUpdated: properties.WHEN_UPDATED,
    featureAreaSqm: properties.FEATURE_AREA_SQM,
    featureLengthM: properties.FEATURE_LENGTH_M,
    objectid: properties.OBJECTID,
    seAnnoCadData: properties.SE_ANNO_CAD_DATA,
  };
}
