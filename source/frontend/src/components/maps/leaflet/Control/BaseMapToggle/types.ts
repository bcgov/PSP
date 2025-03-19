import { exists } from '@/utils';

type RasterBaseLayer = {
  name: string;
  kind: 'raster';
  urls: string[];
  attribution: string;
  thumbnail: string;
};

type VectorBaseLayer = {
  name: string;
  kind: 'esri-vector';
  itemId: string;
  attribution: string;
  thumbnail: string;
};

export type BaseLayer = RasterBaseLayer | VectorBaseLayer;

export function isVectorBasemap(baseLayer: BaseLayer): baseLayer is VectorBaseLayer {
  return baseLayer.kind === 'esri-vector' && exists(baseLayer.itemId);
}

export function isRasterBasemap(baseLayer: BaseLayer): baseLayer is RasterBaseLayer {
  return baseLayer.kind === 'raster' && exists(baseLayer.urls);
}
