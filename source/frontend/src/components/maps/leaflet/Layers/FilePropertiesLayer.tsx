import { feature, featureCollection } from '@turf/turf';
import { FeatureCollection } from 'geojson';
import L, { LatLng } from 'leaflet';
import find from 'lodash/find';
import { useEffect, useMemo, useRef } from 'react';
import { FeatureGroup, GeoJSON, Marker, Pane } from 'react-leaflet';
import { useTheme } from 'styled-components';
import { v4 as uuidv4 } from 'uuid';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationBoundaryDataset } from '@/components/common/mapFSM/models';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists } from '@/utils';

import { useFilterContext } from '../../providers/FilterProvider';
import { getDisabledDraftIcon, getDraftIcon } from './util';

export const FilePropertiesLayer: React.FunctionComponent = () => {
  const draftFeatureGroupRef = useRef<L.FeatureGroup>(null);
  const filterState = useFilterContext();
  const theme = useTheme();

  const mapMachine = useMapStateMachine();
  const mapMarkerClickFn = mapMachine.mapMarkerClick;
  const filePropertyLocations = mapMachine.filePropertyLocations;

  const draftPoints = useMemo<LocationBoundaryDataset[]>(() => {
    return (filePropertyLocations ?? []).filter(
      dp => exists(dp?.location?.lat) && exists(dp?.location?.lng),
    );
  }, [filePropertyLocations]);

  // These are the boundaries for the properties
  const draftBoundaryFeatures = useMemo<FeatureCollection>(() => {
    // ignore properties without a valid boundary
    const validBoundaries = (filePropertyLocations ?? [])
      .map(pl => pl?.boundary)
      .filter(exists)
      .map(boundary => feature(boundary));

    return featureCollection(validBoundaries);
  }, [filePropertyLocations]);

  // These are the user-uploaded shapes in the context of the file (can be different than the property boundaries that mirror PMBC)
  const fileBoundaryFeatures = useMemo<FeatureCollection>(() => {
    const validBoundaries = (filePropertyLocations ?? [])
      .map(pl => pl?.fileBoundary)
      .filter(exists)
      .map(boundary => feature(boundary));

    return featureCollection(validBoundaries);
  }, [filePropertyLocations]);

  const boundaryLayerKeyRef = useRef<string>(uuidv4());
  const fileBoundaryLayerKeyRef = useRef<string>(uuidv4());

  const previousBoundaries = usePrevious(draftBoundaryFeatures);
  const previousFileBoundaries = usePrevious(fileBoundaryFeatures);

  // We need to regenerate an unique `key` on the `<GeoJSON>` element when the underlying data changes.
  // This is to force React to re-render the GeoJSON component with the updated property boundaries.
  // https://github.com/PaulLeCam/react-leaflet/issues/332
  useEffect(() => {
    if (previousBoundaries !== draftBoundaryFeatures) {
      boundaryLayerKeyRef.current = uuidv4();
    }
  }, [draftBoundaryFeatures, previousBoundaries]);

  useEffect(() => {
    if (previousFileBoundaries !== fileBoundaryFeatures) {
      fileBoundaryLayerKeyRef.current = uuidv4();
    }
  }, [fileBoundaryFeatures, previousFileBoundaries]);

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
        if (!find(draftPoints, vl => (l._latlng as LatLng)?.equals(vl.location))) {
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
        <Pane name="file-markers-pane" style={{ zIndex: 650 }}>
          <FeatureGroup ref={draftFeatureGroupRef}>
            {draftPoints.map((draftPoint, index) => {
              return (
                <Marker
                  key={uuidv4()}
                  position={draftPoint.location}
                  icon={
                    draftPoint.isActive !== false
                      ? getDraftIcon((index + 1).toString())
                      : getDisabledDraftIcon((index + 1).toString())
                  }
                  zIndexOffset={500}
                  eventHandlers={{
                    click: e => {
                      // stop propagation of 'click' event to the underlying leaflet map
                      e.originalEvent.preventDefault();
                      e.originalEvent.stopPropagation();

                      mapMarkerClickFn({
                        clusterId: 'NO_ID',
                        latlng: draftPoint.location,
                        pimsFeature: null,
                        fullyAttributedFeature: null,
                      });
                    },
                  }}
                ></Marker>
              );
            })}
          </FeatureGroup>
        </Pane>

        {draftBoundaryFeatures?.features?.length > 0 && (
          <Pane name="property-boundaries-pane" style={{ zIndex: 200 }}>
            <GeoJSON
              key={boundaryLayerKeyRef.current}
              data={draftBoundaryFeatures}
              pathOptions={{ color: '#2A81CB', fill: false, dashArray: [12] }}
            ></GeoJSON>
          </Pane>
        )}

        {fileBoundaryFeatures?.features?.length > 0 && (
          <Pane name="file-boundaries-pane" style={{ zIndex: 450 }}>
            <GeoJSON
              key={fileBoundaryLayerKeyRef.current}
              data={fileBoundaryFeatures}
              pathOptions={{ color: theme.css.pimsRed80, fill: true, fillOpacity: 0.2 }}
            ></GeoJSON>
          </Pane>
        )}
      </>
    ),
    [
      draftPoints,
      draftBoundaryFeatures,
      fileBoundaryFeatures,
      theme.css.pimsRed80,
      mapMarkerClickFn,
    ],
  );
};
