import {
  createElementObject,
  createTileLayerComponent,
  updateGridLayer,
} from '@react-leaflet/core';
import { vectorTileLayer } from 'esri-leaflet-vector';
import { TileLayer as LeafletTileLayer, TileLayerOptions } from 'leaflet';

export interface EsriVectorTileLayerProps extends TileLayerOptions {
  itemId: string;
  opacity?: number;
}

export const EsriVectorTileLayer = createTileLayerComponent<
  LeafletTileLayer,
  EsriVectorTileLayerProps
>(
  function createTileLayer({ itemId, ...options }, context) {
    const layer = vectorTileLayer(itemId, options);
    return createElementObject(layer, context);
  },
  function updateTileLayer(layer, props, prevProps) {
    updateGridLayer(layer, props, prevProps);
  },
);
