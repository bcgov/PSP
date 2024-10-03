import { MultiPolygon, Polygon } from 'geojson';

import { ApiGen_Concepts_Geometry } from '@/models/api/generated/ApiGen_Concepts_Geometry';

export function getMockLocation(lat = 48, lng = -123): ApiGen_Concepts_Geometry {
  return {
    coordinate: { x: lng, y: lat },
  };
}

export function getMockPolygon(): Polygon {
  return {
    type: 'Polygon',
    coordinates: [
      [
        [-123.46, 48.767],
        [-123.4601, 48.7668],
        [-123.461, 48.7654],
        [-123.4623, 48.7652],
        [-123.4627, 48.7669],
        [-123.4602, 48.7672],
        [-123.4601, 48.7672],
        [-123.4601, 48.7672],
        [-123.46, 48.767],
      ],
    ],
  };
}

export function getMockMultiPolygon(): MultiPolygon {
  return {
    type: 'MultiPolygon',
    coordinates: [
      [
        [
          [-123.46, 48.767],
          [-123.4601, 48.7668],
          [-123.461, 48.7654],
          [-123.4623, 48.7652],
          [-123.4627, 48.7669],
          [-123.4602, 48.7672],
          [-123.4601, 48.7672],
          [-123.4601, 48.7672],
          [-123.46, 48.767],
        ],
      ],
    ],
  };
}
