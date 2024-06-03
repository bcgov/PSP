export const getMockMotiHwy = () => ({
  type: 'FeatureCollection',
  features: [
    {
      type: 'Feature',
      id: 'DSA_REGION_BOUNDARY.1',
      geometry: {
        type: 'Polygon',
        coordinates: [[]],
      },
      geometry_name: 'SHAPE',
      properties: {
        OBJECTID: 1,
        REGION_NUMBER: 1,
        REGION_NAME: 'South Coast',
      },
      bbox: [-129.43013603, 48.22452676, -120.49999404, 52.00000245],
    },
  ],
  totalFeatures: 1,
  numberMatched: 1,
  numberReturned: 1,
  timeStamp: '2024-05-31T23:46:24.229Z',
  crs: {
    type: 'name',
    properties: {
      name: 'urn:ogc:def:crs:EPSG::4326',
    },
  },
  bbox: [-129.43013603, 48.22452676, -120.49999404, 52.00000245],
});
