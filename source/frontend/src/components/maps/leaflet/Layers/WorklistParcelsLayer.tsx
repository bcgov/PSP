import React, { useMemo } from 'react';
import { GeoJSON } from 'react-leaflet';
import { v4 as uuidv4 } from 'uuid';

import { useWorklistContext } from '@/features/properties/worklist/context/WorklistContext';
import { ParcelFeature } from '@/features/properties/worklist/models';
import { exists } from '@/utils';

export const WorklistParcelsLayer: React.FunctionComponent = () => {
  const { parcels } = useWorklistContext();

  // For now, lat/long properties in the worklist will not display on the map
  // Ignore properties without a valid boundary
  const validParcels = useMemo<ParcelFeature[]>(
    () => (parcels ?? []).filter(p => exists(p?.pmbcFeature?.geometry)),
    [parcels],
  );

  return (
    <React.Fragment>
      {validParcels.map(vp => (
        <GeoJSON
          key={vp.id ?? uuidv4()}
          data={vp.pmbcFeature}
          style={{ stroke: true, fill: true, color: '#4CCBEA', fillOpacity: 0.2 }}
        />
      ))}
    </React.Fragment>
  );
};

export default WorklistParcelsLayer;
