import { AxiosResponse } from 'axios';
import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useMemo } from 'react';
import { useCallback } from 'react';

import { toCqlFilter } from './layerUtils';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';
import { wfsAxios2 } from './wfsAxios';

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
        await wfsAxios2({ authenticated }).get<FeatureCollection>(
          `${baseUrl}&cql_filter=CONTAINS(${geometryName},SRID=${spatialReferenceId};POINT ( ${latlng.lng} ${latlng.lat}))`,
        )
      )?.data;
      return data;
    },
    [baseUrl, authenticated],
  );

  const executeWfs = useCallback(
    async (
      object: Record<string, any>,
      allBy: boolean | undefined,
      pidOverride?: boolean,
    ): Promise<AxiosResponse<FeatureCollection>> => {
      const data: AxiosResponse<FeatureCollection> = await wfsAxios2({
        timeout: 20000,
        authenticated,
      }).get<FeatureCollection>(
        `${allBy ? baseAllUrl : baseUrl}&${toCqlFilter(object, pidOverride)}`,
      );
      return data;
    },
    [baseAllUrl, baseUrl, authenticated],
  );

  const { execute: findOneWhereContainsWrapped, loading: findOneWhereContainsLoading } =
    useApiRequestWrapper({
      requestFunction: useCallback(
        async (
          latlng: LatLngLiteral,
          geometryName: string = 'SHAPE',
          spatialReferenceId: number = 4326,
        ): Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>>> => {
          const data = await wfsAxios2({ authenticated }).get<
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

  const { execute: findByPid, loading: findByPidLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (pid: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        const formattedPid = pid.replace(/-/g, '');
        return executeWfs({ PID: formattedPid }, allBy, true);
      },
      [baseAllUrl, baseUrl, authenticated],
    ),
    requestName: 'findByPid',
  });

  const { execute: findByPin, loading: findByPinLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (pin: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        return executeWfs({ PIN: pin }, allBy);
      },
      [baseAllUrl, baseUrl, authenticated],
    ),
    requestName: 'findByPin',
  });

  const { execute: findByPlanNumber, loading: findByPlanNumberLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (planNumber: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        return executeWfs({ PLAN_NUMBER: planNumber }, allBy);
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
      findMetadataByLocation,
      findOneWhereContainsWrapped,
      findOneWhereContainsLoading,
    }),
    [
      findMetadataByLocation,
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
