import axios, { AxiosError } from 'axios';
import { layerData } from 'constants/toasts';
import { Feature, FeatureCollection } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useMemo } from 'react';
import { useCallback } from 'react';
import { toast } from 'react-toastify';
import * as rax from 'retry-axios';

export interface IUserLayerQuery {
  /**
   * function to find GeoJSON shape containing a point (x, y)
   * @param latlng = {lat, lng}
   */
  findOneWhereContains: (
    latlng: LatLngLiteral,
    geometryName?: string,
  ) => Promise<FeatureCollection>;
  /**
   * function to find GeoJSON shape matching the passed non-zero padded pid.
   * @param pid
   */
  findByPid: (pid: string) => Promise<FeatureCollection>;
  /**
   * function to find GeoJSON shape matching the passed pin.
   * @param pin
   */
  findByPin: (pin: string) => Promise<FeatureCollection>;
  /**
   * function to find GeoJSON shape matching the passed administrative area.
   * @param city
   */
  findByAdministrative: (city: string) => Promise<Feature | null>;
}

const MAX_RETRIES = 2;
const wfsAxios = () => {
  const instance = axios.create({ timeout: 5000 });
  instance.defaults.raxConfig = {
    retry: MAX_RETRIES,
    instance: instance,
    shouldRetry: (error: AxiosError) => {
      const cfg = rax.getConfig(error);
      if (cfg?.currentRetryAttempt === MAX_RETRIES) {
        toast.dismiss(layerData.LAYER_DATA_LOADING_ID);
        layerData.LAYER_DATA_ERROR();
      }
      return rax.shouldRetryRequest(error);
    },
  };
  rax.attach(instance);

  instance.interceptors.request.use(config => {
    layerData.LAYER_DATA_LOADING();
    return config;
  });
  return instance;
};

/**
 * Custom hook to fetch layer feature collection from wfs url
 * @param url wfs request url
 * @param geometry the name of the geometry in the feature collection
 */
export const useLayerQuery = (url: string): IUserLayerQuery => {
  const baseUrl = `${url}&srsName=EPSG:4326&count=1`;

  const findOneWhereContains = useCallback(
    async (latlng: LatLngLiteral, geometryName: string = 'SHAPE'): Promise<FeatureCollection> => {
      const data: FeatureCollection = (
        await wfsAxios().get<FeatureCollection>(
          `${baseUrl}&cql_filter=CONTAINS(${geometryName},SRID=4326;POINT ( ${latlng.lng} ${latlng.lat}))`,
        )
      )?.data;
      return data;
    },
    [baseUrl],
  );

  const findByAdministrative = useCallback(
    async (city: string): Promise<Feature | null> => {
      try {
        const data: any = (
          await wfsAxios().get(
            `${baseUrl}&cql_filter=ADMIN_AREA_NAME='${city}' OR ADMIN_AREA_ABBREVIATION='${city}'&outputformat=json`,
          )
        )?.data;

        if (data.totalFeatures === 0) {
          return null;
        }
        return data.features[0];
      } catch (error) {
        console.log('Failed to find municipality feature', error);
        return null;
      }
    },
    [baseUrl],
  );

  const findByPid = useCallback(
    async (pid: string): Promise<FeatureCollection> => {
      //Do not make a request if we our currently cached response matches the requested pid.
      const formattedPid = pid.replace(/-/g, '');
      const data: FeatureCollection = (
        await wfsAxios().get<FeatureCollection>(`${baseUrl}&CQL_FILTER=PID_NUMBER=${+formattedPid}`)
      ).data;
      return data;
    },
    [baseUrl],
  );

  const findByPin = useCallback(
    async (pin: string): Promise<FeatureCollection> => {
      //Do not make a request if we our currently cached response matches the requested pid.
      const data: FeatureCollection = (
        await wfsAxios().get<FeatureCollection>(`${baseUrl}&CQL_FILTER=PIN=${pin}`)
      ).data;
      return data;
    },
    [baseUrl],
  );

  return useMemo(
    () => ({
      findOneWhereContains,
      findByPid,
      findByPin,
      findByAdministrative,
    }),
    [findByAdministrative, findByPid, findByPin, findOneWhereContains],
  );
};
