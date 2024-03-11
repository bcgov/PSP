import { AxiosResponse } from 'axios';
import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { toCqlFilter } from '../layer-api/layerUtils';
import { wfsAxios2 } from '../layer-api/wfsAxios';
import { IResponseWrapper, useApiRequestWrapper } from '../util/useApiRequestWrapper';

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

  findOneWhereContainsWrapped: IResponseWrapper<
    (
      latlng: LatLngLiteral,
      geometryName?: string,
      spatialReferenceId?: number,
    ) => Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>>>
  >;
  findOneWhereExactWrapped: IResponseWrapper<
    (
      latlng: LatLngLiteral,
      geometryName?: string,
      spatialReferenceId?: number,
    ) => Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>>>
  >;
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
      geometryName = 'SHAPE',
      spatialReferenceId = 4326,
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

  const findOneWhereContainsWrapped = useApiRequestWrapper({
    requestFunction: useCallback(
      async (
        latlng: LatLngLiteral,
        geometryName = 'SHAPE',
        spatialReferenceId = 4326,
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
    requestName: `findOneWhereContainsWrapped-${baseUrl}`,
  });

  const findOneWhereExactWrapped = useApiRequestWrapper({
    requestFunction: useCallback(
      async (
        latlng: LatLngLiteral,
        geometryName = 'POINT',
        spatialReferenceId = 4326,
      ): Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>>> => {
        const data = await wfsAxios2({ authenticated }).get<
          FeatureCollection<Geometry, GeoJsonProperties>
        >(
          `${baseUrl}&cql_filter=DWITHIN(${geometryName},SRID=${spatialReferenceId};POINT(${latlng.lng} ${latlng.lat}), .001, meters)`,
        );
        return data;
      },
      [baseUrl, authenticated],
    ),
    requestName: `findOneWhereContainsExactWrapped-${baseUrl}`,
  });

  const { execute: findByPid, loading: findByPidLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (pid: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        //Do not make a request if we our currently cached response matches the requested pid.
        const formattedPid = pid.replace(/[-\s]/g, '').padStart(9, '0');
        return executeWfs({ PID: formattedPid }, allBy, true);
      },
      [executeWfs],
    ),
    requestName: 'findByPid',
  });

  const { execute: findByPin, loading: findByPinLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (pin: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        return executeWfs({ PIN: pin }, allBy, true);
      },
      [executeWfs],
    ),
    requestName: 'findByPin',
  });

  const { execute: findByPlanNumber, loading: findByPlanNumberLoading } = useApiRequestWrapper({
    requestFunction: useCallback(
      async (planNumber: string, allBy?: boolean): Promise<AxiosResponse<FeatureCollection>> => {
        return executeWfs({ PLAN_NUMBER: planNumber }, allBy);
      },
      [executeWfs],
    ),
    requestName: 'planNumber',
  });

  const findMetadataByLocation = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName = 'SHAPE',
      spatialReferenceId = 4326,
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

  return {
    findOneWhereContains,
    findByPid,
    findByPidLoading,
    findByPin,
    findByPinLoading,
    findByPlanNumber,
    findByPlanNumberLoading,
    findMetadataByLocation,
    findOneWhereContainsWrapped,
    findOneWhereExactWrapped,
  };
};
