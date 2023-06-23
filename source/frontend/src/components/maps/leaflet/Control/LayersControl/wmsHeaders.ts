import L from 'leaflet';

import CustomAxios from '@/customAxios';

import { ILayerItem } from './types';

/**
 * Override the base leaflet WMS functionality to handle adding an auth token to the requests.
 */
class WmsHeaders extends L.TileLayer.WMS {
  createTile(coords: any, done: any) {
    const url = this.getTileUrl(coords);
    const img = document.createElement('img');
    const axios = CustomAxios();
    axios.get<Blob>(url, { responseType: 'blob' }).then(response => {
      if (response.headers['content-type'] === 'image/png') {
        img.src = URL.createObjectURL(response.data);
      }
      done(null, img);
    });
    return img;
  }
}
/**
 * Only override the base WMS functionality if the url targets the PSP backend.
 * @param url the target WMS url
 * @param options {L.TileLayerOptions}
 */
export const wmsHeaders = (url: string, options: ILayerItem) =>
  options.authenticated ? new WmsHeaders(url, options) : new L.TileLayer.WMS(url, options);
