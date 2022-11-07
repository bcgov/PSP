import { IGeoSearchParams } from 'constants/API';
import { BBox } from 'geojson';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import { IProperty } from 'interfaces';
import { LatLngBounds } from 'leaflet';
import React, { useEffect, useMemo } from 'react';
import { useContext } from 'react';
import { useMap } from 'react-leaflet';
import { tilesInBbox } from 'tiles-in-bbox';

import { useMapRefreshEvent } from '../hooks/useMapRefreshEvent';
import { useMapSearch } from '../hooks/useMapSearch';
import { useFilterContext } from '../providers/FIlterProvider';
import { MapStateContext } from '../providers/MapStateContext';
import { PointFeature } from '../types';
import PointClusterer from './PointClusterer';

export type InventoryLayerProps = {
  /** Latitude and Longitude boundary of the layer. */
  bounds: LatLngBounds;
  /** Zoom level of the map. */
  zoom: number;
  /** Minimum zoom level allowed. */
  minZoom?: number;
  /** Maximum zoom level allowed. */
  maxZoom?: number;
  /** Search filter to apply to properties. */
  filter?: IGeoSearchParams;
  /** What to do when the marker is clicked. */
  onMarkerClick: (property: IProperty) => void;
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
  ] as BBox;
};

interface ITilePoint {
  // x axis of the tile
  x: number;
  // y axis of the tile
  y: number;
  // zoom state of the tile
  z: number;
}

interface ITile {
  // Tile point {x, y, z}
  point: ITilePoint;
  // unique id of the file
  key: string;
  // bbox of the tile
  bbox: string;
  // tile data status
  processed?: boolean;
  // tile data, a list of properties in the tile
  datum?: PointFeature[];
  // tile bounds
  latlngBounds: LatLngBounds;
}

/**
 * Generate tiles for current bounds and zoom
 * @param bounds
 * @param zoom
 */
export const getTiles = (bounds: LatLngBounds, zoom: number): ITile[] => {
  const bbox = {
    bottom: bounds.getSouth(),
    left: bounds.getWest(),
    top: bounds.getNorth(),
    right: bounds.getEast(),
  };

  const tiles = tilesInBbox(bbox, zoom);

  // convert tile x axis to longitude
  const tileToLong = (x: number, z: number) => {
    return (x / Math.pow(2, z)) * 360 - 180;
  };

  // convert tile y axis to longitude
  const tileToLat = (y: number, z: number) => {
    const n = Math.PI - (2 * Math.PI * y) / Math.pow(2, z);

    return (180 / Math.PI) * Math.atan(0.5 * (Math.exp(n) - Math.exp(-n)));
  };

  return tiles.map(({ x, y, z }) => {
    const SW_long = tileToLong(x, z);

    const SW_lat = tileToLat(y + 1, z);

    const NE_long = tileToLong(x + 1, z);

    const NE_lat = tileToLat(y, z);

    return {
      key: `${x}:${y}:${z}`,
      bbox: SW_long + ',' + SW_lat + ',' + NE_long + ',' + NE_lat + ',EPSG:4326',
      point: { x, y, z },
      datum: [],
      latlngBounds: new LatLngBounds({ lat: SW_lat, lng: SW_long }, { lat: NE_lat, lng: NE_long }),
    };
  });
};

// default BC map bounds
export const defaultBounds = new LatLngBounds(
  [60.09114547, -119.49609429],
  [48.78370426, -139.35937554],
);

/**
 * Displays the search results onto a layer with clustering.
 * This component makes a request to the PIMS API properties search WFS endpoint.
 */
export const InventoryLayer: React.FC<InventoryLayerProps> = ({
  bounds,
  zoom,
  filter,
  onMarkerClick,
}) => {
  const mapInstance = useMap();
  const { search, properties, loading } = useMapSearch();
  const { changed: filterChanged } = useFilterContext();
  const { draftProperties } = useContext(MapStateContext);

  if (!mapInstance) {
    throw new Error('<InventoryLayer /> must be used under a <Map> leaflet component');
  }

  const bbox = useMemo(() => getBbox(bounds), [bounds]);
  useEffect(() => {
    const fit = async () => {
      if (filterChanged) {
        mapInstance.fitBounds(defaultBounds, { maxZoom: 5 });
      }
    };

    fit();
  }, [mapInstance, filter, filterChanged]);

  const params = useMemo((): any => {
    const tiles = getTiles(defaultBounds, 5);

    return tiles.map(tile => ({
      STREET_ADDRESS_1: filter?.STREET_ADDRESS_1,
      PID: filter?.PID,
      PIN: filter?.PIN,
      BBOX: tile.bbox,
    }));
  }, [filter]);
  useMapRefreshEvent(() => search(params));
  useDeepCompareEffect(() => {
    if (filterChanged || !properties?.length) {
      search(params);
    }
  }, [params, filterChanged]);

  return (
    <PointClusterer
      draftPoints={draftProperties}
      points={properties}
      zoom={zoom}
      bounds={bbox}
      onMarkerClick={onMarkerClick}
      zoomToBoundsOnClick={true}
      spiderfyOnMaxZoom={true}
      tilesLoaded={!loading}
    />
  );
};
