import axios, { AxiosError, AxiosResponse } from 'axios';
import { layerData } from 'constants/toasts';
import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { LatLngLiteral } from 'leaflet';
import { useMemo } from 'react';
import { useCallback } from 'react';
import { toast } from 'react-toastify';
import * as rax from 'retry-axios';
import { store } from 'store/store';

import { toCqlFilter } from '../../mapUtils';

export interface IUserLayerQuery {
  /**
   * function to find GeoJSON shape containing a point (x, y)
   * @param latlng = {lat, lng}
   * @param geometryName the name of the geometry field for this layer; can be 'GEOMETRY' or 'SHAPE'
   * @param spatialReferenceId the spatial reference of the location argument; common values are: 4326 (WGS84 - lat/lng) and 3005 (BC Albers)
   */
  findOneWhereContains: (
    latlng: LatLngLiteral,
    geometryName?: string,
    spatialReferenceId?: number,
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

  /**
   * Function to query spatial layers and return layer metadata for the supplied location (x, y)
   * @param latlng = {lat, lng}
   * @param geometryName the name of the geometry field for this layer; can be 'GEOMETRY' or 'SHAPE'
   * @param spatialReferenceId the spatial reference of the location argument; common values are: 4326 (WGS84 - lat/lng) and 3005 (BC Albers)
   */
  findMetadataByLocation: (
    latlng: LatLngLiteral,
    geometryName?: string,
    spatialReferenceId?: number,
  ) => Promise<Record<string, any>>;

  findOneWhereContainsWrapped: (
    latlng: LatLngLiteral,
    geometryName?: string | undefined,
    spatialReferenceId?: number | undefined,
  ) => Promise<FeatureCollection<Geometry, GeoJsonProperties> | undefined>;
  findOneWhereContainsLoading: boolean;
}

const MAX_RETRIES = 2;
const wfsAxios = (axiosProps?: { timeout?: number; authenticated?: boolean }) => {
  const instance = axios.create({
    timeout: axiosProps?.timeout ?? 5000,
  });
  if (axiosProps?.authenticated) {
    instance.defaults.headers.common['Authorization'] = `Bearer ${store.getState().jwt}`;
  }
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
 * // TODO: PSP-4393 This should be deprecated
 * Custom hook to fetch layer feature collection from wfs url
 * @param url wfs request url
 * @param geometry the name of the geometry in the feature collection
 */
export const useLayerQuery = (url: string, authenticated?: boolean): IUserLayerQuery => {
  const baseAllUrl = `${url}&srsName=EPSG:4326`;
  const baseUrl = `${url}&srsName=EPSG:4326&count=1`;

  const findOneWhereContains = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName: string = 'SHAPE',
      spatialReferenceId: number = 4326,
    ): Promise<FeatureCollection> => {
      const data: FeatureCollection = (
        await wfsAxios({ authenticated }).get<FeatureCollection>(
          `${baseUrl}&cql_filter=CONTAINS(${geometryName},SRID=${spatialReferenceId};POINT ( ${latlng.lng} ${latlng.lat}))`,
        )
      )?.data;
      return data;
    },
    [baseUrl, authenticated],
  );

  const {
    execute: findOneWhereContainsWrapped,
    loading: findOneWhereContainsLoading,
  } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (
        latlng: LatLngLiteral,
        geometryName: string = 'SHAPE',
        spatialReferenceId: number = 4326,
      ): Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>>> => {
        const data = await wfsAxios({ authenticated }).get<
          FeatureCollection<Geometry, GeoJsonProperties>
        >(
          `${baseUrl}&cql_filter=CONTAINS(${geometryName},SRID=${spatialReferenceId};POINT ( ${latlng.lng} ${latlng.lat}))`,
        );
        return data;
      },
      [baseUrl, authenticated],
    ),
    requestName: 'findByPid',
  });

  const findByAdministrative = useCallback(
    async (city: string): Promise<Feature | null> => {
      try {
        const data: any = (
          await wfsAxios({ authenticated }).get(
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
    [baseUrl, authenticated],
  );

  const { execute: findByPid, loading: findByPidLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (pid: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        //Do not make a request if we our currently cached response matches the requested pid.
        const formattedPid = pid.replace(/-/g, '');
        const data: AxiosResponse<FeatureCollection> = await wfsAxios({
          timeout: 20000,
          authenticated,
        }).get<FeatureCollection>(
          `${allBy ? baseAllUrl : baseUrl}&${toCqlFilter({ PID: formattedPid }, true)}`,
        );
        return data;
      },
      [baseAllUrl, baseUrl, authenticated],
    ),
    requestName: 'findByPid',
  });

  const { execute: findByPin, loading: findByPinLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (pin: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        //Do not make a request if we our currently cached response matches the requested pid.
        const data: AxiosResponse<FeatureCollection> = await wfsAxios({
          timeout: 20000,
          authenticated,
        }).get<FeatureCollection>(`${allBy ? baseAllUrl : baseUrl}&${toCqlFilter({ PIN: pin })}`);
        return data;
      },
      [baseAllUrl, baseUrl, authenticated],
    ),
    requestName: 'findByPin',
  });

  const { execute: findByPlanNumber, loading: findByPlanNumberLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (planNumber: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        //Do not make a request if we our currently cached response matches the requested pid.
        const data: AxiosResponse<FeatureCollection> = await wfsAxios({
          timeout: 20000,
          authenticated,
        }).get<FeatureCollection>(
          `${allBy ? baseAllUrl : baseUrl}&${toCqlFilter({ PLAN_NUMBER: planNumber })}`,
        );
        return data;
      },
      [baseAllUrl, baseUrl, authenticated],
    ),
    requestName: 'planNumber',
  });

  const findMetadataByLocation = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName: string = 'SHAPE',
      spatialReferenceId: number = 4326,
    ): Promise<Record<string, any>> => {
      try {
        const collection = await findOneWhereContains(latlng, geometryName, spatialReferenceId);
        if (collection?.features?.length > 0) {
          return collection.features[0].properties || {};
        }
        return {};
      } catch (error) {
        // return empty object if map layer is not available
        return {};
      }
    },
    [findOneWhereContains],
  );

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
      findMetadataByLocation,
      findOneWhereContainsWrapped,
      findOneWhereContainsLoading,
    }),
    [
      findMetadataByLocation,
      findByAdministrative,
      findByPid,
      findByPidLoading,
      findByPin,
      findByPinLoading,
      findByPlanNumber,
      findByPlanNumberLoading,
      findOneWhereContains,
      findOneWhereContainsWrapped,
      findOneWhereContainsLoading,
    ],
  );
};
