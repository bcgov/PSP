import { FeatureCollection, Geometry } from 'geojson';
import { LatLngBounds, LatLngLiteral } from 'leaflet';

import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import {
  PIMS_Property_Boundary_View,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';

export interface FeatureSelected {
  readonly clusterId: string;
  readonly pimsLocationFeature: PIMS_Property_Location_View | null;
  readonly pimsBoundaryFeature: PIMS_Property_Boundary_View | null;
  readonly fullyAttributedFeature: PMBC_FullyAttributed_Feature_Properties | null;
  readonly latlng: LatLngLiteral;
}

export interface MapFeatureData {
  readonly pimsLocationFeatures: FeatureCollection<Geometry, PIMS_Property_Location_View>;
  readonly pimsBoundaryFeatures: FeatureCollection<Geometry, PIMS_Property_Boundary_View>;
  readonly fullyAttributedFeatures: FeatureCollection<
    Geometry,
    PMBC_FullyAttributed_Feature_Properties
  >;
}

export interface RequestedFlyTo {
  readonly location: LatLngLiteral | null;
  readonly bounds: LatLngBounds | null;
}

export const emptyPimsLocationFeatureCollection: FeatureCollection<
  Geometry,
  PIMS_Property_Location_View
> = {
  type: 'FeatureCollection',
  features: [],
};

export const emptyPimsBoundaryFeatureCollection: FeatureCollection<
  Geometry,
  PIMS_Property_Boundary_View
> = {
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
  pimsLocationFeatures: emptyPimsLocationFeatureCollection,
  pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
  fullyAttributedFeatures: emptyFullyFeaturedFeatureCollection,
};
