import * as turf from '@turf/turf';
import { LatLngLiteral } from 'leaflet';

import { emptyFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { LocationDatasetWithId } from '@/features/properties/worklist/context/WorklistContext';
import {
  emptyPmbcParcel,
  PMBC_FullyAttributed_Feature_Properties,
} from '@/models/layers/parcelMapBC';
import { exists } from '@/utils';

// Factory for ParcelFeature using
export const getMockWorklistParcel = (
  customId: string,
  props: Partial<PMBC_FullyAttributed_Feature_Properties> = {},
  coords?: LatLngLiteral,
): LocationDatasetWithId => {
  const geometry = exists(coords) ? [coords.lng, coords.lat] : [0, 0];
  const geoFeature = turf.point<PMBC_FullyAttributed_Feature_Properties>(geometry, {
    ...emptyPmbcParcel,
    ...props,
  });

  const parcel: LocationDatasetWithId = {
    ...emptyFeatureDataset(),
    id: customId,
    location: coords,
    parcelFeatures: [geoFeature],
  };

  if (exists(coords)) {
    parcel.location = coords;
  }

  return parcel;
};
