import React, { useMemo } from 'react';
import { GeoJSON } from 'react-leaflet';

import { useWorklistContext } from '@/features/properties/worklist/context/WorklistContext';
import { exists, firstValidOrNull } from '@/utils';

export const WorklistParcelsLayer: React.FunctionComponent = () => {
  const { parcels } = useWorklistContext();

  // For now, lat/long properties in the worklist will not display on the map
  // Ignore properties without a valid boundary
  const validParcels = useMemo(
    () =>
      (parcels ?? []).filter(p => exists(firstValidOrNull(p?.parcelFeatures, exists)?.geometry)),
    [parcels],
  );

  return (
    <React.Fragment>
      {validParcels.map(vp => (
        <GeoJSON
          key={vp.id}
          data={firstValidOrNull(vp.parcelFeatures, exists)}
          style={{ stroke: true, fill: true, color: '#4CCBEA', fillOpacity: 0.2 }}
        />
      ))}
    </React.Fragment>
  );
};

export default WorklistParcelsLayer;
