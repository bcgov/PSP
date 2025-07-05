import { BBox } from 'geojson';
import { LatLngBounds, LeafletMouseEvent } from 'leaflet';
import React, { useMemo } from 'react';
import { FeatureGroup, Marker, useMap } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { exists } from '@/utils';

import PointClusterer from './PointClusterer';
import { getNotOwnerMarkerIcon } from './util';

export type InventoryLayerProps = {
  /** Latitude and Longitude boundary of the layer. */
  bounds: LatLngBounds;
  /** Zoom level of the map. */
  zoom: number;
  /** Minimum zoom level allowed. */
  minZoom?: number;
  /** Maximum zoom level allowed. */
  maxZoom?: number;
};

/**
 * Get a new instance of a BBox from the specified 'bounds'.
 * @param bounds The latitude longitude boundary.
 */
const getBbox = (bounds: LatLngBounds): BBox => {
  return [
    bounds.getSouthWest().lng,
    bounds.getSouthWest().lat,
    bounds.getNorthEast().lng,
    bounds.getNorthEast().lat,
  ];
};

/**
 * Displays the search results onto a layer with clustering.
 * This component makes a request to the PIMS API properties search WFS endpoint.
 */
export const MarkerLayer: React.FC<React.PropsWithChildren<InventoryLayerProps>> = ({
  bounds,
  zoom,
  minZoom,
  maxZoom,
}) => {
  const mapInstance = useMap();
  const mapMachine = useMapStateMachine();

  if (!mapInstance) {
    throw new Error('<InventoryLayer /> must be used under a <Map> leaflet component');
  }

  const bbox = useMemo(() => getBbox(bounds), [bounds]);

  const markedLocation = useMemo(
    () => mapMachine.mapMarkedLocation,
    [mapMachine.mapMarkedLocation],
  );

  return (
    <>
      <PointClusterer
        zoom={zoom}
        minZoom={minZoom}
        maxZoom={maxZoom}
        bounds={bbox}
        zoomToBoundsOnClick={true}
        spiderfyOnMaxZoom={true}
        tilesLoaded={!mapMachine.isLoading}
      />

      {exists(markedLocation) && (
        <FeatureGroup>
          <Marker
            position={markedLocation}
            icon={getNotOwnerMarkerIcon(true)}
            eventHandlers={{
              click: (e: LeafletMouseEvent) => {
                e.originalEvent.stopPropagation();
                mapMachine.mapClick(markedLocation);
              },
            }}
          />
        </FeatureGroup>
      )}
    </>
  );
};
