import { FeatureCollection, Geometry } from 'geojson';
import { LatLngBounds, LatLngLiteral } from 'leaflet';

import { TANTALIS_CrownSurveyParcels_Feature_Properties } from '@/models/layers/crownLand';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import {
  PIMS_Property_Boundary_View,
  PIMS_Property_Location_Lite_View,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';

export interface MarkerSelected {
  readonly clusterId: string;
  readonly pimsLocationFeature:
    | PIMS_Property_Location_View
    | PIMS_Property_Location_Lite_View
    | null;
  readonly pimsBoundaryFeature: PIMS_Property_Boundary_View | null;
  readonly fullyAttributedFeature: PMBC_FullyAttributed_Feature_Properties | null;
  readonly latlng: LatLngLiteral;
}

export interface MapFeatureData {
  readonly pimsLocationFeatures: FeatureCollection<Geometry, PIMS_Property_Location_View>;
  readonly pimsLocationLiteFeatures: FeatureCollection<Geometry, PIMS_Property_Location_Lite_View>;
  readonly pimsBoundaryFeatures: FeatureCollection<Geometry, PIMS_Property_Boundary_View>;
  readonly fullyAttributedFeatures: FeatureCollection<
    Geometry,
    PMBC_FullyAttributed_Feature_Properties
  >;
  readonly surveyedParcelsFeatures: FeatureCollection<
    Geometry,
    TANTALIS_CrownSurveyParcels_Feature_Properties
  >;
  readonly highwayPlanFeatures: FeatureCollection<Geometry, ISS_ProvincialPublicHighway>;
}

export interface RequestedFlyTo {
  readonly location: LatLngLiteral | null;
  readonly bounds: LatLngBounds | null;
}

export interface RequestedCenterTo {
  readonly location: LatLngLiteral | null;
}

export interface LocationBoundaryDataset {
  readonly location: LatLngLiteral;
  readonly boundary: Geometry | null;
  readonly isActive?: boolean;
}

export const emptyPimsLocationFeatureCollection: FeatureCollection<
  Geometry,
  PIMS_Property_Location_View
> = {
  type: 'FeatureCollection',
  features: [],
};

export const emptyPimsLocationLiteFeatureCollection: FeatureCollection<
  Geometry,
  PIMS_Property_Location_Lite_View
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

export const emptyPmbcFeatureCollection: FeatureCollection<
  Geometry,
  PMBC_FullyAttributed_Feature_Properties
> = {
  type: 'FeatureCollection',
  features: [],
};

export const emptySurveyedParcelsFeatures: FeatureCollection<
  Geometry,
  TANTALIS_CrownSurveyParcels_Feature_Properties
> = {
  type: 'FeatureCollection',
  features: [],
};
export const emptyHighwayFeatures: FeatureCollection<Geometry, ISS_ProvincialPublicHighway> = {
  type: 'FeatureCollection',
  features: [],
};

export const emptyFeatureData: MapFeatureData = {
  pimsLocationFeatures: emptyPimsLocationFeatureCollection,
  pimsLocationLiteFeatures: emptyPimsLocationLiteFeatureCollection,
  pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
  fullyAttributedFeatures: emptyPmbcFeatureCollection,
  surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
  highwayPlanFeatures: emptyHighwayFeatures,
};
