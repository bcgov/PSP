import { FeatureCollection, Geometry } from 'geojson';
import { LatLngBounds, LatLngLiteral } from 'leaflet';

import { TANTALIS_CrownSurveyParcels_Feature_Properties } from '@/models/layers/crownLand';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import { PIMS_Property_Lite_View, PIMS_Property_View } from '@/models/layers/pimsPropertyView';

export interface MarkerSelected {
  readonly clusterId: string;
  readonly pimsFeature: PIMS_Property_View | PIMS_Property_Lite_View | null;
  readonly fullyAttributedFeature: PMBC_FullyAttributed_Feature_Properties | null;
  readonly latlng: LatLngLiteral;
}

export interface MapFeatureData {
  readonly pimsFeatures: FeatureCollection<Geometry, PIMS_Property_View>;
  readonly pimsLiteFeatures: FeatureCollection<Geometry, PIMS_Property_Lite_View>;
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
  readonly fileBoundary: Geometry | null;
  readonly isActive?: boolean;
}

export const emptyPimsFeatureCollection: FeatureCollection<Geometry, PIMS_Property_View> = {
  type: 'FeatureCollection',
  features: [],
};

export const emptyPimsLiteFeatureCollection: FeatureCollection<Geometry, PIMS_Property_Lite_View> =
  {
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
  pimsFeatures: emptyPimsFeatureCollection,
  pimsLiteFeatures: emptyPimsLiteFeatureCollection,
  fullyAttributedFeatures: emptyPmbcFeatureCollection,
  surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
  highwayPlanFeatures: emptyHighwayFeatures,
};
