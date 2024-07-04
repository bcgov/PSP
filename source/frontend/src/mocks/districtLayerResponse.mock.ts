export const mockDistrictLayerResponse = {
  type: 'FeatureCollection',
  features: [
    {
      type: 'Feature',
      id: 'DSA_DISTRICT_BOUNDARY.2',
      geometry: {
        type: 'Polygon',
        coordinates: [],
      },
      geometry_name: 'SHAPE',
      properties: {
        OBJECTID: 2,
        DISTRICT_NUMBER: 2,
        DISTRICT_NAME: 'Vancouver Island',
      },
      bbox: [-129.43013603, 48.22452676, -123.00728678, 51.00005479],
    },
  ],
  totalFeatures: 1,
  numberMatched: 1,
  numberReturned: 1,
  timeStamp: '2024-05-31T23:46:24.270Z',
  crs: {
    type: 'name',
    properties: {
      name: 'urn:ogc:def:crs:EPSG::4326',
    },
  },
  bbox: [-129.43013603, 48.22452676, -123.00728678, 51.00005479],
};
