import { Feature, Geometry } from 'geojson';
import React, { useMemo } from 'react';
import { GeoJSON } from 'react-leaflet';
import { v4 as uuidv4 } from 'uuid';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import { exists } from '@/utils';

export const HighwayParcelsLayer: React.FunctionComponent = () => {
  const { mapFeatureData } = useMapStateMachine();

  // For now, lat/long properties in the highway list will not display on the map
  // Ignore properties without a valid boundary
  const validParcels = useMemo<Feature<Geometry, ISS_ProvincialPublicHighway>[]>(
    () => (mapFeatureData?.highwayPlanFeatures?.features ?? []).filter(p => exists(p?.geometry)),
    [mapFeatureData?.highwayPlanFeatures?.features],
  );

  return (
    <React.Fragment>
      {validParcels.map(vp => (
        <GeoJSON
          key={vp.id ?? uuidv4()}
          data={vp}
          style={{ stroke: true, fill: true, color: '#9925be', fillOpacity: 0.2 }}
        />
      ))}
    </React.Fragment>
  );
};

export default HighwayParcelsLayer;
