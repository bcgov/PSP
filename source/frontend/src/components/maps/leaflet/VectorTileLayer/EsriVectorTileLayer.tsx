import {
  createElementObject,
  createTileLayerComponent,
  updateGridLayer,
} from '@react-leaflet/core';
import { vectorTileLayer } from 'esri-leaflet-vector';
import L, { TileLayer as LeafletTileLayer, TileLayerOptions } from 'leaflet';

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

// psp-10192 monkeypatch recommended by https://github.com/slutske22/react-esri-leaflet/issues/22
L.Map.include({
  removeLayer(layer: L.Layer) {
    if (!layer) return this;

    const id = L.Util.stamp(layer);

    if (!this._layers[id]) {
      return this;
    }

    if (this._loaded) {
      layer.onRemove(this);
    }

    delete this._layers[id];

    if (this._loaded) {
      this.fire('layerremove', { layer });
      layer.fire('remove');
    }

    // @ts-expect-error leaflet TS very incomplete
    layer._map = layer._mapToAdd = null;

    return this;
  },
});
