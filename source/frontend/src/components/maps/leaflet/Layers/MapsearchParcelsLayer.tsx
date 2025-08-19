import { Feature, Geometry } from 'geojson';
import React, { useMemo } from 'react';
import { GeoJSON } from 'react-leaflet';
import { v4 as uuidv4 } from 'uuid';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { exists } from '@/utils';

export const MapsearchParcelsLayer: React.FunctionComponent = () => {
  const { mapFeatureData } = useMapStateMachine();

  // For now, lat/long properties in the worklist will not display on the map
  // Ignore properties without a valid boundary
  const validParcels = useMemo<Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>[]>(
    () => (mapFeatureData?.fullyAttributedFeatures.features ?? []).filter(p => exists(p?.geometry)),
    [mapFeatureData?.fullyAttributedFeatures.features],
  );

  return (
    <React.Fragment>
      {validParcels.map(vp => (
        <GeoJSON
          key={vp.id ?? uuidv4()}
          data={vp}
          style={{ stroke: true, fill: true, color: '#873e23', fillOpacity: 0.2 }}
        />
      ))}
    </React.Fragment>
  );
};

export default MapsearchParcelsLayer;
