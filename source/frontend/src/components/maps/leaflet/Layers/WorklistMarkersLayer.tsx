import React, { useMemo } from 'react';
import { Marker } from 'react-leaflet';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { useWorklistContext } from '@/features/properties/worklist/context/WorklistContext';
import { exists, firstOrNull, latLngToKey } from '@/utils';

import { getNotOwnerMarkerIcon } from './util';

export const WorklistMarkersLayer: React.FunctionComponent = () => {
  const { parcels } = useWorklistContext();

  // Now, lat/long properties in the worklist will display on the map as markers.
  // But must not have a Feature.
  const validLocations = useMemo<LocationFeatureDataset[]>(
    () =>
      (parcels ?? []).filter(p => !exists(firstOrNull(p?.parcelFeatures)) && exists(p?.location)),
    [parcels],
  );

  return (
    <React.Fragment>
      {validLocations.map(vp => (
        <Marker
          key={latLngToKey(vp.location)}
          position={vp.location}
          icon={getNotOwnerMarkerIcon(true)}
        />
      ))}
    </React.Fragment>
  );
};

export default WorklistMarkersLayer;
