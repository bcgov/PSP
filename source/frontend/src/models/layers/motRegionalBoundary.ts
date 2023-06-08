import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';

// Source : https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric-fully-attributed/resource/59d9964f-bc93-496f-8039-b83ab8f24a41
export interface MOT_RegionalBoundary_Feature_Properties2 {
  readonly REGION_NUMBER: number | null;
  readonly FEATURE_CODE: string | null;
  readonly REGION_NAME: string | null;
  readonly OBJECTID: number | null;
  readonly SE_ANNO_CAD_DATA: Blob | null;
  readonly FEATURE_AREA_SQM: number | null;
  readonly FEATURE_LENGTH_M: number | null;
}

interface MOT_RegionalBoundary_Feature_Properties {
  readonly regionNumber: number | null;
  readonly featureCode: string | null;
  readonly regionName: string | null;
  readonly objectid: number | null;
  readonly seAnnoCadData: Blob | null;
  readonly featureAreaSqm: number | null;
  readonly featureLengthM: number | null;
}

const regionFromFeatureCollection = (
  values: FeatureCollection<Geometry, MOT_RegionalBoundary_Feature_Properties2> | undefined,
): MOT_RegionalBoundary_Feature_Properties[] | undefined =>
  values?.features
    .map(feature => fromProperties(feature.properties))
    .filter((x): x is MOT_RegionalBoundary_Feature_Properties => !!x);

function fromProperties(
  properties: MOT_RegionalBoundary_Feature_Properties2,
): MOT_RegionalBoundary_Feature_Properties | undefined {
  if (properties === null || properties === undefined) {
    return undefined;
  }

  return {
    regionNumber: properties.REGION_NUMBER,
    featureCode: properties.FEATURE_CODE,
    regionName: properties.REGION_NAME,
    objectid: properties.OBJECTID,
    seAnnoCadData: properties.SE_ANNO_CAD_DATA,
    featureAreaSqm: properties.FEATURE_AREA_SQM,
    featureLengthM: properties.FEATURE_LENGTH_M,
  };
}
