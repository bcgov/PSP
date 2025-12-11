import {
  LocationFeatureDataset,
  SelectedFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';

import getMockISSResult from './mockISSResult';

export const getMockLocationFeatureDataset = (): LocationFeatureDataset => ({
  location: {
    lat: 48.432802005,
    lng: -123.310041775,
  },
  fileLocation: {
    lat: 48.432802005,
    lng: -123.310041775,
  },
  pimsFeatures: [
    {
      properties: null,
      type: 'Feature',
      geometry: {
        type: 'Point',
        coordinates: [-123.310041775, 48.432802005],
      },
    },
  ],
  parcelFeatures: [
    {
      type: 'Feature',
      id: 'WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW.fid--55c55c24_18d9ff61aac_-5aaa',
      geometry: {
        type: 'Polygon',
        coordinates: [[[-123.31014591, 48.43274258]]],
      },
      //geometry_name: 'SHAPE',
      properties: {
        PARCEL_FABRIC_POLY_ID: 6103848,
        PARCEL_NAME: '000002500',
        PLAN_NUMBER: 'VIP3881',
        PIN: null,
        PID: '000002500',
        PID_FORMATTED: '000-002-500',
        PID_NUMBER: 2500,
        PARCEL_STATUS: 'Active',
        PARCEL_CLASS: 'Subdivision',
        OWNER_TYPE: 'Private',
        PARCEL_START_DATE: null,
        MUNICIPALITY: 'Oak Bay, The Corporation of the District of',
        REGIONAL_DISTRICT: 'Capital Regional District',
        WHEN_UPDATED: '2022-03-24Z',
        FEATURE_AREA_SQM: 647.4646,
        FEATURE_LENGTH_M: 109.0764,
        OBJECTID: 848479361,
        SE_ANNO_CAD_DATA: null,
        GLOBAL_UID: '',
        PLAN_ID: 0,
        SOURCE_PARCEL_ID: null,
        SURVEY_DESIGNATION_1: null,
        SURVEY_DESIGNATION_2: null,
        SURVEY_DESIGNATION_3: null,
        LEGAL_DESCRIPTION: null,
        IS_REMAINDER_IND: null,
        GEOMETRY_SOURCE: null,
        POSITIONAL_ERROR: null,
        ERROR_REPORTED_BY: null,
        CAPTURE_METHOD: null,
        COMPILED_IND: null,
        STATED_AREA: null,
        WHEN_CREATED: null,
        SHAPE: null,
      },
    },
  ],
  regionFeature: {
    type: 'Feature',
    id: 'WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY.1',
    geometry: {
      type: 'Polygon',
      coordinates: [[[-126.05000031, 51.9999999]]],
    },
    //geometry_name: 'GEOMETRY',
    properties: {
      REGION_NUMBER: 1,
      FEATURE_CODE: null,
      REGION_NAME: 'South Coast',
      OBJECTID: 20273,
      SE_ANNO_CAD_DATA: null,
      FEATURE_AREA_SQM: 151439012177.005,
      FEATURE_LENGTH_M: 2098978.5042,
    },
  },
  districtFeature: {
    type: 'Feature',
    id: 'WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY.2',
    geometry: {
      type: 'Polygon',
      coordinates: [[[-125.86634507, 50.38795075]]],
    },
    //geometry_name: 'GEOMETRY',
    properties: {
      DISTRICT_NUMBER: 2,
      DISTRICT_NAME: 'Vancouver Island',
      FEATURE_CODE: null,
      OBJECTID: 52285,
      SE_ANNO_CAD_DATA: null,
      FEATURE_AREA_SQM: 65831400828.9239,
      FEATURE_LENGTH_M: 1340187.9529,
    },
  },
  municipalityFeatures: [
    {
      type: 'Feature',
      id: 'WHSE_LEGAL_ADMIN_BOUNDARIES.ABMS_MUNICIPALITIES_SP.265',
      geometry: {
        type: 'Polygon',
        coordinates: [[[-123.29632516, 48.42581565]]],
      },
      //geometry_name: 'SHAPE',
      properties: {
        LGL_ADMIN_AREA_ID: 265,
        ADMIN_AREA_NAME: 'The Corporation of the District of Oak Bay',
        ADMIN_AREA_ABBREVIATION: 'Oak Bay',
        ADMIN_AREA_BOUNDARY_TYPE: 'Legal',
        ADMIN_AREA_GROUP_NAME: 'Capital Regional District',
        CHANGE_REQUESTED_ORG: 'MUNI',
        UPDATE_TYPE: 'E',
        WHEN_UPDATED: null,
        MAP_STATUS: 'Not Appended',
        OIC_MO_NUMBER: '665',
        OIC_MO_YEAR: '1981',
        OIC_MO_TYPE: 'OIC',
        WEBSITE_URL: null,
        IMAGE_URL: null,
        AFFECTED_ADMIN_AREA_ABRVN: null,
        FEATURE_AREA_SQM: 15719635.1334,
        FEATURE_LENGTH_M: 20852.8728,
        OBJECTID: 13892,
        SE_ANNO_CAD_DATA: null,
      },
    },
  ],
  highwayFeatures: getMockISSResult().features,
  selectingComponentId: '',
  crownLandLeasesFeatures: [],
  crownLandLicensesFeatures: [],
  crownLandTenuresFeatures: [],
  crownLandInventoryFeatures: [],
  crownLandInclusionsFeatures: [],
});

export const getMockSelectedFeatureDataset = (): SelectedFeatureDataset => {
  const locationFeatureDataset = getMockLocationFeatureDataset();
  return {
    selectingComponentId: locationFeatureDataset.selectingComponentId,
    location: locationFeatureDataset.location,
    fileLocation: locationFeatureDataset.fileLocation,
    parcelFeature: locationFeatureDataset.parcelFeatures[0],
    pimsFeature: locationFeatureDataset.pimsFeatures[0],
    regionFeature: locationFeatureDataset.regionFeature,
    districtFeature: locationFeatureDataset.districtFeature,
    municipalityFeature: locationFeatureDataset.municipalityFeatures[0],
  };
};
