import { feature, featureCollection } from '@turf/turf';
import { FeatureCollection } from 'geojson';
import L, { LatLng, LatLngLiteral } from 'leaflet';
import find from 'lodash/find';
import { useEffect, useMemo, useRef } from 'react';
import { FeatureGroup, GeoJSON, Marker } from 'react-leaflet';
import { v4 as uuidv4 } from 'uuid';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists } from '@/utils';

import { useFilterContext } from '../../providers/FilterProvider';
import { getDraftIcon } from './util';

export const FilePropertiesLayer: React.FunctionComponent = () => {
  const draftFeatureGroupRef = useRef<L.FeatureGroup>(null);
  const filterState = useFilterContext();

  const mapMachine = useMapStateMachine();
  const mapMarkerClickFn = mapMachine.mapMarkerClick;
  const filePropertyLocations = mapMachine.filePropertyLocations;

  const draftPoints = useMemo<LatLngLiteral[]>(() => {
    return (filePropertyLocations ?? []).map(pl => pl.location).filter(exists);
  }, [filePropertyLocations]);

  const draftBoundaryFeatures = useMemo<FeatureCollection>(() => {
    // ignore properties without a valid boundary
    const validBoundaries = (filePropertyLocations ?? [])
      .map(pl => pl.boundary)
      .filter(exists)
      .map(boundary => feature(boundary));

    return featureCollection(validBoundaries);
  }, [filePropertyLocations]);

  const geojsonKeyRef = useRef<string>(uuidv4());
  const previousBoundaries = usePrevious(draftBoundaryFeatures);

  // We need to regenerate an unique `key` on the `<GeoJSON>` element when the underlying data changes.
  // This is to force React to re-render the GeoJSON component with the updated property boundaries.
  // https://github.com/PaulLeCam/react-leaflet/issues/332
  useEffect(() => {
    if (previousBoundaries !== draftBoundaryFeatures) {
      geojsonKeyRef.current = uuidv4();
    }
  }, [draftBoundaryFeatures, previousBoundaries]);

  /**
   * Cleanup draft layers.
   * TODO: Figure out if this is still necessary now that this does not fit the map bounds
   */
  useDeepCompareEffect(() => {
    const hasDraftPoints = draftPoints.length > 0;
    if (draftFeatureGroupRef.current && hasDraftPoints) {
      const group: L.FeatureGroup = draftFeatureGroupRef.current;

      //react-leaflet is not displaying removed drafts but the layer is still present, this
      //causes the fitbounds calculation to be off. Fixed by manually cleaning up layers referencing removed drafts.
      group.getLayers().forEach((l: any) => {
        if (!find(draftPoints, vl => (l._latlng as LatLng)?.equals(vl))) {
          group.removeLayer(l);
        }
      });

      const groupBounds = group.getBounds();

      if (groupBounds.isValid()) {
        filterState.setChanged(false);
      }
    }
  }, [draftFeatureGroupRef, draftPoints]);

  /**
   * Render all of the unclustered DRAFT MARKERS.
   **/
  return useMemo(
    () => (
      <>
        <FeatureGroup ref={draftFeatureGroupRef}>
          {draftPoints.map((draftPoint, index) => {
            return (
              <Marker
                key={uuidv4()}
                position={draftPoint}
                icon={getDraftIcon((index + 1).toString())}
                zIndexOffset={500}
                eventHandlers={{
                  click: e => {
                    // stop propagation of 'click' event to the underlying leaflet map
                    e.originalEvent.preventDefault();
                    e.originalEvent.stopPropagation();

                    mapMarkerClickFn({
                      clusterId: 'NO_ID',
                      latlng: draftPoint,
                      pimsLocationFeature: null,
                      pimsBoundaryFeature: null,
                      fullyAttributedFeature: null,
                    });
                  },
                }}
              ></Marker>
            );
          })}
        </FeatureGroup>
        {draftBoundaryFeatures?.features?.length > 0 && (
          <GeoJSON
            key={geojsonKeyRef.current}
            data={draftBoundaryFeatures}
            pathOptions={{ color: '#2A81CB', fill: false, dashArray: [12] }}
          ></GeoJSON>
        )}
      </>
    ),
    [draftBoundaryFeatures, draftPoints, geojsonKeyRef, mapMarkerClickFn],
  );
};
