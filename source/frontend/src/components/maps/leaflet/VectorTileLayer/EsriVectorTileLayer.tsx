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

    let id: number;
    try {
      id = L.Util.stamp(layer);
    } catch {
      // If Leaflet cannot stamp the layer, abort safely.
      return this;
    }

    if (!this._layers[id]) {
      return this;
    }

    if (this._loaded) {
      try {
        layer.onRemove(this);
      } catch {
        // Swallow errors from custom layer teardown to avoid breaking map unmount.
      }
    }

    delete this._layers[id];

    if (this._loaded) {
      this.fire('layerremove', { layer });
      // Some layers may not implement events in a standard way; guard the call.
      try {
        layer.fire('remove');
      } catch {
        // No-op if the layer cannot fire events.
      }
    }

    // @ts-expect-error leaflet TS very incomplete
    layer._map = layer._mapToAdd = null;

    return this;
  },
});

// Guard Map.hasLayer against null/undefined layers that can be produced in
// edge cases by React-Leaflet/Leaflet integration in tests.
L.Map.include({
  hasLayer(layer: L.Layer) {
    if (!layer) {
      return false;
    }

    const id = L.Util.stamp(layer);
    return id in this._layers;
  },
});

// Guard Leaflet's global stamp utility against null/undefined inputs that can
// occur in tests or edge cases, to prevent runtime TypeErrors.
const originalStamp = L.Util.stamp;
// eslint-disable-next-line @typescript-eslint/no-explicit-any
L.Util.stamp = function (obj: any): number {
  if (obj == null) {
    // Return a sentinel ID that will never match a real layer.
    return -1;
  }

  return originalStamp(obj);
};
