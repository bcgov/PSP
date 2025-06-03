import L, { LatLng, LatLngLiteral } from 'leaflet';
import find from 'lodash/find';
import { useMemo, useRef } from 'react';
import { FeatureGroup, Marker } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';

import { useFilterContext } from '../../providers/FilterProvider';
import { getDraftIcon } from './util';

export const FilePropertiesLayer: React.FunctionComponent = () => {
  const draftFeatureGroupRef = useRef<L.FeatureGroup>(null);
  const filterState = useFilterContext();

  const mapMachine = useMapStateMachine();
  const mapMarkerClickFn = mapMachine.mapMarkerClick;
  const filePropertyLocations = mapMachine.filePropertyLocations;

  const draftPoints = useMemo<LatLngLiteral[]>(() => {
    return filePropertyLocations;
  }, [filePropertyLocations]);

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
        if (!find(draftPoints, vl => (l._latlng as LatLng).equals(vl))) {
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
      <FeatureGroup ref={draftFeatureGroupRef}>
        {draftPoints.map((draftPoint, index) => {
          console.log(draftPoint);
          return (
            <Marker
              key={index}
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
    ),
    [draftPoints, mapMarkerClickFn],
  );
};
