import React, { useMemo } from 'react';
import { Marker } from 'react-leaflet';

import { ParcelDataset } from '@/features/properties/parcelList/models';
import { useWorklistContext } from '@/features/properties/worklist/context/WorklistContext';
import { exists } from '@/utils';

import { getNotOwnerMarkerIcon } from './util';

export const WorklistMarkersLayer: React.FunctionComponent = () => {
  const { parcels } = useWorklistContext();

  // Now, lat/long properties in the worklist will display on the map as markers.
  // But must not have a Feature.
  const validLocations = useMemo<ParcelDataset[]>(
    () => (parcels ?? []).filter(p => !exists(p?.pmbcFeature) && exists(p?.location)),
    [parcels],
  );

  return (
    <React.Fragment>
      {validLocations.map(vp => (
        <Marker key={vp.id} position={vp.location} icon={getNotOwnerMarkerIcon(true)} />
      ))}
    </React.Fragment>
  );
};

export default WorklistMarkersLayer;
