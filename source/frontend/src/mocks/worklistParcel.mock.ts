import * as turf from '@turf/turf';
import { LatLngLiteral } from 'leaflet';

import {
  emptyFeatureDataset,
  LocationFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import {
  emptyPmbcParcel,
  PMBC_FullyAttributed_Feature_Properties,
} from '@/models/layers/parcelMapBC';
import { exists } from '@/utils';

// Factory for ParcelFeature using
export const getMockWorklistParcel = (
  props: Partial<PMBC_FullyAttributed_Feature_Properties> = {},
  coords?: LatLngLiteral,
): LocationFeatureDataset => {
  const geometry = exists(coords) ? [coords.lng, coords.lat] : [0, 0];
  const geoFeature = turf.point<PMBC_FullyAttributed_Feature_Properties>(geometry, {
    ...emptyPmbcParcel,
    ...props,
  });

  const parcel: LocationFeatureDataset = {
    ...emptyFeatureDataset(),
    location: coords,
    parcelFeatures: [geoFeature],
  };

  if (exists(coords)) {
    parcel.location = coords;
  }

  return parcel;
};
