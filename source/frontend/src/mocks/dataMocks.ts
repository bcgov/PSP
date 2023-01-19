export const createMockHeader = () => ({
  headers: {
    'Access-Control-Allow-Origin': '*',
    Authorization: 'mockToken',
  },
});

export const ERROR = { message: 'Errors', status: 400, data: { details: 'error' } };

export const mockWfsGetPropertyById = {
  type: 'FeatureCollection',
  features: [
    {
      type: 'Feature',
      id: 'PIMS_PROPERTY_LOCATION_VW.fid--458993de_17c52535f19_-6487',
      geometry: {
        type: 'Point',
        coordinates: [-124.0658, 48.8281],
      },
      geometry_name: 'GEOMETRY',
      properties: {
        PROPERTY_ID: 4481,
        PID: 6914781,
        PIN: null,
        PROPERTY_TYPE_CODE: 'LAND',
        PROPERTY_STATUS_TYPE_CODE: 'MOTIADMIN',
        PROPERTY_DATA_SOURCE_TYPE_CODE: 'PAIMS',
        PROPERTY_DATA_SOURCE_EFFECTIVE_DATE: '2021-08-31Z',
        PROPERTY_CLASSIFICATION_TYPE_CODE: 'COREOPER',
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
        NAME: null,
        DESCRIPTION: null,
        ADDRESS_ID: 0,
        REGION_CODE: 1,
        DISTRICT_CODE: 1,
        PROPERTY_AREA_UNIT_TYPE_CODE: 'HA',
        LAND_AREA: 1,
        LAND_LEGAL_DESCRIPTION: null,
        ENCUMBRANCE_REASON: null,
        IS_SENSITIVE: false,
        IS_OWNED: false,
        IS_PROPERTY_OF_INTEREST: false,
        IS_VISIBLE_TO_OTHER_AGENCIES: false,
        ZONING: null,
        ZONING_POTENTIAL: null,
        IS_PAYABLE_LEASE: false,
      },
      bbox: [-124.0658, 48.8281, -124.0658, 48.8281],
    },
  ],
  totalFeatures: 1,
  numberMatched: 1,
  numberReturned: 1,
  timeStamp: '2021-10-05T22:27:49.176Z',
  crs: {
    type: 'name',
    properties: {
      name: 'urn:ogc:def:crs:EPSG::4326',
    },
  },
  bbox: [-124.0658, 48.8281, -124.0658, 48.8281],
};
