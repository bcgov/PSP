import { FeatureCollection, Geometry } from 'geojson';
import { LatLngBounds, LatLngLiteral } from 'leaflet';

import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

export interface FeatureSelected {
  readonly clusterId: string;
  readonly pimsFeature: PIMS_Property_Location_View | null;
  readonly fullyAttributedFeature: PMBC_FullyAttributed_Feature_Properties | null;
  readonly latlng: LatLngLiteral;
}

export interface MapFeatureData {
  pimsFeatures: FeatureCollection<Geometry, PIMS_Property_Location_View>;
  fullyAttributedFeatures: FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>;
}

export interface RequestedFlyTo {
  location: LatLngLiteral | null;
  bounds: LatLngBounds | null;
}

export const emptyPimsFeatureCollection: FeatureCollection<Geometry, PIMS_Property_Location_View> =
  {
    type: 'FeatureCollection',
    features: [],
  };

export const emptyFullyFeaturedFeatureCollection: FeatureCollection<
  Geometry,
  PMBC_FullyAttributed_Feature_Properties
> = {
  type: 'FeatureCollection',
  features: [],
};

export const emptyFeatureData: MapFeatureData = {
  pimsFeatures: emptyPimsFeatureCollection,
  fullyAttributedFeatures: emptyFullyFeaturedFeatureCollection,
};
