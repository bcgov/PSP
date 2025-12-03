import { feature, featureCollection } from '@turf/turf';
import { FeatureCollection, Geometry } from 'geojson';
import L, { LatLng, LatLngLiteral } from 'leaflet';
import find from 'lodash/find';
import { useEffect, useMemo, useRef } from 'react';
import { FeatureGroup, GeoJSON, Marker, Pane } from 'react-leaflet';
import { useTheme } from 'styled-components';
import { v4 as uuidv4 } from 'uuid';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { exists, getLatLng, locationFromFileProperty } from '@/utils';

import { useFilterContext } from '../../providers/FilterProvider';
import { getDisabledDraftIcon, getDraftIcon } from './util';
interface FileLocationBoundaryDataset {
  readonly location: LatLngLiteral;
  readonly propertyBoundary: Geometry | null;
  readonly fileBoundary: Geometry | null;
  readonly isActive?: boolean;
}

function filePropertyToFileLocationBoundaryDataset(
  fileProperty: ApiGen_Concepts_FileProperty | undefined | null,
): FileLocationBoundaryDataset | null {
  const geom = locationFromFileProperty(fileProperty);
  const location = getLatLng(geom);
  return exists(location)
    ? {
        location,
        propertyBoundary: fileProperty?.property?.boundary ?? null,
        fileBoundary: fileProperty?.boundary ?? null,
        isActive: fileProperty.isActive,
      }
    : null;
}

export const FilePropertiesLayer: React.FunctionComponent = () => {
  const draftFeatureGroupRef = useRef<L.FeatureGroup>(null);
  const filterState = useFilterContext();
  const theme = useTheme();

  const mapMachine = useMapStateMachine();
  const mapMarkerClickFn = mapMachine.mapMarkerClick;

  const filePropertyLocations = useMemo(
    () => mapMachine.filePropertyLocations.map(x => filePropertyToFileLocationBoundaryDataset(x)),
    [mapMachine.filePropertyLocations],
  );

  const draftPoints = useMemo<FileLocationBoundaryDataset[]>(() => {
    return (filePropertyLocations ?? []).filter(
      dp => exists(dp?.location?.lat) && exists(dp?.location?.lng),
    );
  }, [filePropertyLocations]);

  // These are the boundaries for the properties
  const propertyBoundaryFeatures = useMemo<FeatureCollection>(() => {
    // ignore properties without a valid boundary
    const validBoundaries = (filePropertyLocations ?? [])
      .map(pl => pl.propertyBoundary)
      .filter(exists)
      .map(boundary => feature(boundary));

    return featureCollection(validBoundaries);
  }, [filePropertyLocations]);

  // These are the user-uploaded shapes in the context of the file (can be different than the property boundaries that mirror PMBC)
  const fileBoundaryFeatures = useMemo<FeatureCollection>(() => {
    const validBoundaries = (filePropertyLocations ?? [])
      .map(pl => pl.fileBoundary)
      .filter(exists)
      .map(boundary => feature(boundary));

    return featureCollection(validBoundaries);
  }, [filePropertyLocations]);

  const boundaryLayerKeyRef = useRef<string>(uuidv4());
  const fileBoundaryLayerKeyRef = useRef<string>(uuidv4());

  const previousPropertyBoundaries = usePrevious(propertyBoundaryFeatures);
  const previousFileBoundaries = usePrevious(fileBoundaryFeatures);

  // We need to regenerate an unique `key` on the `<GeoJSON>` element when the underlying data changes.
  // This is to force React to re-render the GeoJSON component with the updated property boundaries.
  // https://github.com/PaulLeCam/react-leaflet/issues/332
  useEffect(() => {
    if (previousPropertyBoundaries !== propertyBoundaryFeatures) {
      boundaryLayerKeyRef.current = uuidv4();
    }
  }, [propertyBoundaryFeatures, previousPropertyBoundaries]);

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
        </Pane>

        {propertyBoundaryFeatures?.features?.length > 0 && (
          <Pane name="property-boundaries-pane" style={{ zIndex: 200 }}>
            <GeoJSON
              key={boundaryLayerKeyRef.current}
              data={propertyBoundaryFeatures}
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
      propertyBoundaryFeatures,
      fileBoundaryFeatures,
      theme.css.pimsRed80,
      mapMarkerClickFn,
    ],
  );
};
