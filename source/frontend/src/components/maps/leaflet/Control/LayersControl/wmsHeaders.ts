import L from 'leaflet';

import CustomAxios from '@/customAxios';

import { LayerDefinition } from './types';

/**
 * Override the base leaflet WMS functionality to handle adding an auth token to the requests.
 */
class WmsHeaders extends L.TileLayer.WMS {
  createTile(coords: any, done: any) {
    const url = this.getTileUrl(coords);
    const img = document.createElement('img');
    const axios = CustomAxios();
    axios.get<Blob>(url, { responseType: 'blob', withCredentials: true }).then(response => {
      if (response.headers['content-type'] === 'image/png') {
        img.src = URL.createObjectURL(response.data);
      }
      done(null, img);
    });
    return img;
  }
}
/**
 * only override the base wms functionality if the url targets the PSP backend.
 * @param url the target WMS url
 * @param options {L.TileLayerOptions}
 */
export const wmsHeaders = (url: string, options: LayerDefinition): L.TileLayer.WMS =>
  options.authenticated ? new WmsHeaders(url, options) : new L.TileLayer.WMS(url, options);
