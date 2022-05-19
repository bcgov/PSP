import axios, { AxiosError, AxiosResponse } from 'axios';
import { layerData } from 'constants/toasts';
import { Feature, FeatureCollection } from 'geojson';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { LatLngLiteral } from 'leaflet';
import { useMemo } from 'react';
import { useCallback } from 'react';
import { toast } from 'react-toastify';
import * as rax from 'retry-axios';

import { toCqlFilter } from '../../mapUtils';

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
  findByPid: (pid: string, allBy?: boolean) => Promise<FeatureCollection | undefined>;
  findByPidLoading: boolean;
  /**
   * function to find GeoJSON shape matching the passed pin.
   * @param pin
   */
  findByPin: (pin: string, allBy?: boolean) => Promise<FeatureCollection | undefined>;
  findByPinLoading: boolean;
  /**
   * function to find GeoJSON shape matching the passed planNumber.
   * @param planNumber
   */
  findByPlanNumber: (planNumber: string, allBy?: boolean) => Promise<FeatureCollection | undefined>;
  findByPlanNumberLoading: boolean;
  /**
   * function to find GeoJSON shape matching the passed administrative area.
   * @param city
   */
  findByAdministrative: (city: string) => Promise<Feature | null>;
}

const MAX_RETRIES = 2;
const wfsAxios = (timeout?: number) => {
  const instance = axios.create({ timeout: timeout ?? 5000 });
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
  const baseAllUrl = `${url}&srsName=EPSG:4326`;
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

  const { execute: findByPid, loading: findByPidLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (pid: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        //Do not make a request if we our currently cached response matches the requested pid.
        const formattedPid = pid.replace(/-/g, '');
        const data: AxiosResponse<FeatureCollection> = await wfsAxios(20000).get<FeatureCollection>(
          `${allBy ? baseAllUrl : baseUrl}&${toCqlFilter({ PID: formattedPid }, true)}`,
        );
        return data;
      },
      [baseAllUrl, baseUrl],
    ),
    requestName: 'findByPid',
  });

  const { execute: findByPin, loading: findByPinLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (pin: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        //Do not make a request if we our currently cached response matches the requested pid.
        const data: AxiosResponse<FeatureCollection> = await wfsAxios(20000).get<FeatureCollection>(
          `${allBy ? baseAllUrl : baseUrl}&${toCqlFilter({ PIN: pin })}`,
        );
        return data;
      },
      [baseAllUrl, baseUrl],
    ),
    requestName: 'findByPin',
  });

  const { execute: findByPlanNumber, loading: findByPlanNumberLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (planNumber: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        //Do not make a request if we our currently cached response matches the requested pid.
        const data: AxiosResponse<FeatureCollection> = await wfsAxios(20000).get<FeatureCollection>(
          `${allBy ? baseAllUrl : baseUrl}&${toCqlFilter({ PLAN_NUMBER: planNumber })}`,
        );
        return data;
      },
      [baseAllUrl, baseUrl],
    ),
    requestName: 'planNumber',
  });

  return useMemo(
    () => ({
      findOneWhereContains,
      findByPid,
      findByPidLoading,
      findByPin,
      findByPinLoading,
      findByPlanNumber,
      findByPlanNumberLoading,
      findByAdministrative,
    }),
    [
      findByAdministrative,
      findByPid,
      findByPidLoading,
      findByPin,
      findByPinLoading,
      findByPlanNumber,
      findByPlanNumberLoading,
      findOneWhereContains,
    ],
  );
};
