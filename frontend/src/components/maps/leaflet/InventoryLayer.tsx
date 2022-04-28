import { IGeoSearchParams } from 'constants/API';
import { BBox, Feature } from 'geojson';
import { useApi } from 'hooks/useApi';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import { IProperty } from 'interfaces';
import { geoJSON, LatLngBounds } from 'leaflet';
import { uniqBy } from 'lodash';
import React, { useEffect, useMemo, useState } from 'react';
import { useContext } from 'react';
import { useMap } from 'react-leaflet';
import { toast } from 'react-toastify';
import { tilesInBbox } from 'tiles-in-bbox';

import { useMapRefreshEvent } from '../hooks/useMapRefreshEvent';
import { useFilterContext } from '../providers/FIlterProvider';
import { SelectedPropertyContext } from '../providers/SelectedPropertyContext';
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
  /** Callback function to display/hide backdrop*/
  onRequestData: (showBackdrop: boolean) => void;
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
  onRequestData,
}) => {
  const mapInstance = useMap();
  const [features, setFeatures] = useState<PointFeature[]>([]);
  const [loadingTiles, setLoadingTiles] = useState(false);
  const { loadProperties } = useApi();
  const { changed: filterChanged } = useFilterContext();
  const { draftProperties } = useContext(SelectedPropertyContext);

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

  const loadTile = async (mapFilter: IGeoSearchParams) => {
    return loadProperties(mapFilter);
  };

  /**
   * TODO: convert polygon features into lat/lng coordinates, remove this when new Point Geoserver layer available.
   * @param feature the feature to obtain lat/lng coordinates for.
   * @returns [lat, lng]
   */
  const getLatLng = (feature: any) => {
    if (feature.geometry.type === 'Polygon' || feature.geometry.type === 'MultiPolygon') {
      const latLng = geoJSON(feature.geometry)
        .getBounds()
        .getCenter();
      return [latLng.lng, latLng.lat];
    }
    return feature.geometry.coordinates;
  };

  const search = async (filters: IGeoSearchParams[]) => {
    //TODO: currently this loads all matching properties, this should be rewritten to use the bbox and make one request per tile.
    try {
      onRequestData(true);
      const tileData = await loadTile(filters[0]);
      const validFeatures = tileData.features.filter(feature => !!feature?.geometry);
      const data = validFeatures.map((feature: Feature) => {
        //TODO: this converts all polygons to points, this should be changed to a View that returns the POINT instead of the POLYGON (psp-1859)
        return {
          ...feature,
          geometry: { type: 'Point', coordinates: getLatLng(feature) },
          properties: {
            ...feature.properties,
          },
        } as Feature;
      });

      const results = uniqBy(data, (point: Feature) => `${point?.properties?.PROPERTY_ID}`);

      setFeatures(results as any);
      setLoadingTiles(false);
      if (results.length === 0) {
        toast.info('No search results found');
      } else {
        toast.info(`${results.length} properties found`);
      }
    } catch (error) {
      toast.error((error as Error).message, { autoClose: 7000 });
    } finally {
      onRequestData(false);
    }
  };

  useMapRefreshEvent(() => search(params));
  useDeepCompareEffect(() => {
    setLoadingTiles(true);
    search(params);
  }, [params]);

  return (
    <PointClusterer
      draftPoints={draftProperties}
      points={features}
      zoom={zoom}
      bounds={bbox}
      onMarkerClick={onMarkerClick}
      zoomToBoundsOnClick={true}
      spiderfyOnMaxZoom={true}
      tilesLoaded={!loadingTiles}
    />
  );
};
