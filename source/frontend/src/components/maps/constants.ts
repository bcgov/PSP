import { LatLngBounds } from 'leaflet';

export const DEFAULT_MAP_ZOOM = 6;
/** rough center of bc Itcha Ilgachuz Provincial Park */
export const defaultLatLng = {
  lat: 52.81604319154934,
  lng: -124.67285156250001,
};

// default BC map bounds
export const defaultBounds = new LatLngBounds(
  [60.09114547, -119.49609429],
  [48.78370426, -139.35937554],
);
