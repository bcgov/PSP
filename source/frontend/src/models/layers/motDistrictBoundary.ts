import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';

// Source : https://catalogue.data.gov.bc.ca/dataset/ministry-of-transportation-mot-district-boundary/resource/ae458593-3860-4552-9bcc-9f016478f782
export interface MOT_DistrictBoundary_Feature_Properties2 {
  readonly DISTRICT_NUMBER: number | null;
  readonly DISTRICT_NAME: string | null;
  readonly FEATURE_CODE: string | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
}

interface MOT_DistrictBoundary_Feature_Properties {
  readonly districtNumber: number | null;
  readonly districtName: string | null;
  readonly featureCode: string | null;
  readonly objectid: number | null;
  readonly seAnnoCadData: Blob | null;
  readonly featureAreaSqm: number | null;
  readonly featureLengthM: number | null;
}

const districtFromFeatureCollection = (
  values: FeatureCollection<Geometry, MOT_DistrictBoundary_Feature_Properties2> | undefined,
): MOT_DistrictBoundary_Feature_Properties[] | undefined =>
  values?.features
    .map(feature => fromProperties(feature.properties))
    .filter((x): x is MOT_DistrictBoundary_Feature_Properties => !!x);

function fromProperties(
  properties: MOT_DistrictBoundary_Feature_Properties2,
): MOT_DistrictBoundary_Feature_Properties | undefined {
  if (properties === null || properties === undefined) {
    return undefined;
  }

  return {
    districtNumber: properties.DISTRICT_NUMBER,
    districtName: properties.DISTRICT_NAME,
    featureCode: properties.FEATURE_CODE,
    objectid: properties.OBJECTID,
    seAnnoCadData: properties.SE_ANNO_CAD_DATA,
    featureAreaSqm: properties.FEATURE_AREA_SQM,
    featureLengthM: properties.FEATURE_LENGTH_M,
  };
}
