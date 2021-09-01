import { IGeoSearchParams } from 'constants/API';
import { BBox } from 'geojson';
import { useApiProperties } from 'hooks/pims-api';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { GeoJSON, LatLngBounds } from 'leaflet';
import { flatten, uniqBy } from 'lodash';
import React, { useEffect, useMemo, useState } from 'react';
import { useMap } from 'react-leaflet';
import { toast } from 'react-toastify';
import { useAppSelector } from 'store/hooks';
import { IPropertyDetail } from 'store/slices/properties';
import { tilesInBbox } from 'tiles-in-bbox';

import { useMapRefreshEvent } from '../hooks/useMapRefreshEvent';
import { useFilterContext } from '../providers/FIlterProvider';
import { PointFeature } from '../types';
import { MUNICIPALITY_LAYER_URL, useLayerQuery } from './LayerPopup';
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
  onMarkerClick: () => void;

  selected?: IPropertyDetail | null;
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
      bbox: SW_long + ',' + NE_long + ',' + SW_lat + ',' + NE_lat,
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
  minZoom,
  maxZoom,
  filter,
  onMarkerClick,
  selected,
  onRequestData,
}) => {
  const keycloak = useKeycloakWrapper();
  const mapInstance = useMap();
  const [features, setFeatures] = useState<PointFeature[]>([]);
  const [loadingTiles, setLoadingTiles] = useState(false);
  const { getPropertiesWfs } = useApiProperties();
  const { changed: filterChanged } = useFilterContext();
  const municipalitiesService = useLayerQuery(MUNICIPALITY_LAYER_URL);

  const draftProperties: PointFeature[] = useAppSelector(state => state.properties.draftProperties);

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

  minZoom = minZoom ?? 0;
  maxZoom = maxZoom ?? 18;

  const params = useMemo((): any => {
    const tiles = getTiles(defaultBounds, 5);

    return tiles.map(tile => ({
      bbox: tile.bbox,
      address: filter?.address,
      municipality: filter?.municipality,
      pid: filter?.pid,
      organizations: filter?.organizations,
      classificationId: filter?.classificationId,
      minLandArea: filter?.minLandArea,
      maxLandArea: filter?.maxLandArea,
      name: filter?.name,
    }));
  }, [filter]);

  const loadTile = async (filter: IGeoSearchParams) => {
    return getPropertiesWfs(filter);
  };

  const search = async (filters: IGeoSearchParams[]) => {
    try {
      onRequestData(true);
      const data = flatten(await Promise.all(filters.map(x => loadTile(x)))).map(f => {
        return {
          ...f,
        } as PointFeature;
      });

      const items = uniqBy(
        data,
        point => `${point?.properties?.id}-${point?.properties?.propertyTypeId}`,
      );

      let results = items.filter(({ properties }: any) => {
        return keycloak.canUserEditProperty(properties);
      }) as any;

      // Fit to municipality bounds
      const municipality = filter?.municipality;
      if (results.length === 0 && !!municipality) {
        const value = await municipalitiesService.findByAdministrative(municipality);
        if (value) {
          const bounds = (GeoJSON.geometryToLayer(value) as any)._bounds;
          mapInstance.fitBounds(bounds, { maxZoom: 11 });
        }
      }
      setFeatures(results);
      setLoadingTiles(false);
      if (results.length === 0) {
        toast.info('No search results found');
      } else {
        toast.info(`${results.length} properties found`);
      }
    } catch (error) {
      toast.error((error as Error).message, { autoClose: 7000 });
      console.error(error);
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
      points={features}
      draftPoints={draftProperties}
      zoom={zoom}
      bounds={bbox}
      onMarkerClick={onMarkerClick}
      zoomToBoundsOnClick={true}
      spiderfyOnMaxZoom={true}
      selected={selected}
      tilesLoaded={!loadingTiles}
    />
  );
};
