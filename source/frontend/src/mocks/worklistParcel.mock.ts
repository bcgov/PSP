import * as turf from '@turf/turf';
import { LatLngLiteral } from 'leaflet';

import { ParcelDataset } from '@/features/properties/parcelList/models';
import {
  emptyPmbcParcel,
  PMBC_FullyAttributed_Feature_Properties,
} from '@/models/layers/parcelMapBC';
import { exists } from '@/utils';

// Factory for ParcelFeature using fromFullyAttributedFeature
export const getMockWorklistParcel = (
  id: string,
  props: Partial<PMBC_FullyAttributed_Feature_Properties> = {},
  coords?: LatLngLiteral,
): ParcelDataset => {
  const geometry = exists(coords) ? [coords.lng, coords.lat] : [0, 0];
  const geoFeature = turf.point<PMBC_FullyAttributed_Feature_Properties>(geometry, {
    ...emptyPmbcParcel,
    ...props,
  });

  const parcel = ParcelDataset.fromFullyAttributedFeature(geoFeature);
  parcel.id = id;

  if (exists(coords)) {
    parcel.location = coords;
  }

  return parcel;
};
