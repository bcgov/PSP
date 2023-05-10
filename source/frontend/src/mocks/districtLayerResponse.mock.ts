export const mockDistrictLayerResponse = {
  type: 'FeatureCollection',
  features: [
    {
      type: 'Feature',
      id: 'WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY.2',
      geometry: {
        type: 'Polygon',
        coordinates: [
          [
            [-122.64209928, 49.10436194],
            [-122.64211348, 49.10396936],
            [-122.64085839, 49.10394959],
            [-122.64083912, 49.10447851],
            [-122.64201075, 49.10449689],
            [-122.64201384, 49.10441544],
            [-122.64209928, 49.10436194],
          ],
        ],
      },
      geometry_name: 'GEOMETRY',
      properties: {
        DISTRICT_NUMBER: 2,
        DISTRICT_NAME: 'Vancouver Island',
        FEATURE_CODE: null,
        OBJECTID: 30215,
        SE_ANNO_CAD_DATA: null,
        FEATURE_AREA_SQM: 65831400828.9239,
        FEATURE_LENGTH_M: 1340187.9529,
      },
    },
  ],
  totalFeatures: 1,
  numberMatched: 1,
  numberReturned: 1,
  timeStamp: '2022-06-16T22:38:59.099Z',
  crs: {
    type: 'name',
    properties: {
      name: 'urn:ogc:def:crs:EPSG::4326',
    },
  },
};
