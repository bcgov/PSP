import React, { useMemo } from 'react';
import { GeoJSON } from 'react-leaflet';
import { v4 as uuidv4 } from 'uuid';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { useWorklistContext } from '@/features/properties/worklist/context/WorklistContext';
import { exists, firstValidOrNull, latLngToKey } from '@/utils';

export const WorklistParcelsLayer: React.FunctionComponent = () => {
  const { parcels } = useWorklistContext();

  // For now, lat/long properties in the worklist will not display on the map
  // Ignore properties without a valid boundary
  const validParcels = useMemo<LocationFeatureDataset[]>(
    () =>
      (parcels ?? []).filter(p => exists(firstValidOrNull(p?.parcelFeatures, exists)?.geometry)),
    [parcels],
  );

  return (
    <React.Fragment>
      {validParcels.map(vp => (
        <GeoJSON
          key={latLngToKey(vp.location) ?? uuidv4()}
          data={firstValidOrNull(vp.parcelFeatures, exists)}
          style={{ stroke: true, fill: true, color: '#4CCBEA', fillOpacity: 0.2 }}
        />
      ))}
    </React.Fragment>
  );
};

export default WorklistParcelsLayer;
