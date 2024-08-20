import { FeatureCollection, Geometry } from 'geojson';

import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

export const createMockHeader = () => ({
  headers: {
    'Access-Control-Allow-Origin': '*',
    Authorization: 'mockToken',
  },
});

export const ERROR = { message: 'Errors', status: 400, data: { details: 'error' } };

export const mockWfsGetPropertyById: FeatureCollection<Geometry, PIMS_Property_Location_View> = {
  type: 'FeatureCollection',
  features: [
    {
      type: 'Feature',
      id: 'PIMS_PROPERTY_LOCATION_VW.fid--458993de_17c52535f19_-6487',
      geometry: {
        type: 'Point',
        coordinates: [-124.0658, 48.8281],
      },
      properties: {
        PROPERTY_ID: 4481,
        PID: 6914781,
        PIN: null,
        PROPERTY_TYPE_CODE: 'LAND',
        PROPERTY_STATUS_TYPE_CODE: 'MOTIADMIN',
        PROPERTY_DATA_SOURCE_TYPE_CODE: 'PAIMS',
        PROPERTY_DATA_SOURCE_EFFECTIVE_DATE: '2021-08-31Z',
        PROPERTY_TENURE_TYPE_CODE: 'TM',
        STREET_ADDRESS_1: '940 Blanshard Street',
        STREET_ADDRESS_2: null,
        STREET_ADDRESS_3: null,
        MUNICIPALITY_NAME: 'Victoria',
        POSTAL_CODE: 'V8W 3E6',
        PROVINCE_STATE_CODE: 'BC',
        PROVINCE_NAME: 'British Columbia',
        COUNTRY_CODE: 'CA',
        COUNTRY_NAME: 'Canada',
        ADDRESS_ID: 0,
        REGION_CODE: 1,
        DISTRICT_CODE: 1,
        PROPERTY_AREA_UNIT_TYPE_CODE: 'HA',
        LAND_AREA: 1,
        LAND_LEGAL_DESCRIPTION: null,
        IS_OWNED: false,
        HAS_ACTIVE_RESEARCH_FILE: false,
        HAS_ACTIVE_ACQUISITION_FILE: false,
        IS_PAYABLE_LEASE: false,
        PID_PADDED: '',
        SURVEY_PLAN_NUMBER: '',
        IS_RETIRED: false,
        IS_DISPOSED: false,
        IS_OTHER_INTEREST: false,
        IS_ACTIVE_PAYABLE_LEASE: false,
        IS_RECEIVABLE_LEASE: false,
        IS_ACTIVE_RECEIVABLE_LEASE: false,
        HISTORICAL_FILE_NUMBER_STR: '',
      },
      bbox: [-124.0658, 48.8281, -124.0658, 48.8281],
    },
  ],
  bbox: [-124.0658, 48.8281, -124.0658, 48.8281],
};
