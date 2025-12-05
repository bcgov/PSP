import { feature, featureCollection } from '@turf/turf';
import { Feature, FeatureCollection, Geometry } from 'geojson';
import React, { useEffect, useMemo, useRef } from 'react';
import { GeoJSON } from 'react-leaflet';
import { v4 as uuidv4 } from 'uuid';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePrevious } from '@/hooks/usePrevious';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Boundary_View } from '@/models/layers/pimsPropertyLocationView';
import { exists } from '@/utils';

export const MapsearchParcelsLayer: React.FunctionComponent = () => {
  const { mapFeatureData } = useMapStateMachine();

  // For now, lat/long properties in the search list result will not display on the map
  // Ignore properties without a valid boundary
  const validParcels = useMemo<Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>[]>(
    () => (mapFeatureData?.fullyAttributedFeatures.features ?? []).filter(p => exists(p?.geometry)),
    [mapFeatureData?.fullyAttributedFeatures.features],
  );

  const searchValidBoundaries = useMemo<Feature<Geometry, PIMS_Property_Boundary_View>[]>(
    () => (mapFeatureData?.pimsBoundaryFeatures.features ?? []).filter(p => exists(p?.geometry)),
    [mapFeatureData?.pimsBoundaryFeatures.features],
  );

  const draftBoundaryFeatures = useMemo<FeatureCollection>(() => {
    // ignore properties without a valid boundary
    const validBoundaries = (searchValidBoundaries ?? [])
      .map(pl => pl.geometry)
      .filter(exists)
      .map(boundary => feature(boundary));

    return featureCollection(validBoundaries);
  }, [searchValidBoundaries]);

  const geojsonKeyRef = useRef<string>(uuidv4());
  const previousBoundaries = usePrevious(draftBoundaryFeatures);

  useEffect(() => {
    if (previousBoundaries !== draftBoundaryFeatures) {
      geojsonKeyRef.current = uuidv4();
    }
  }, [draftBoundaryFeatures, previousBoundaries]);

  return (
    <React.Fragment>
      {draftBoundaryFeatures?.features?.length > 0 && (
        <GeoJSON
          key={geojsonKeyRef.current}
          data={draftBoundaryFeatures}
          pathOptions={{ color: '#2A81CB', fill: false, dashArray: [12] }}
        ></GeoJSON>
      )}
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
