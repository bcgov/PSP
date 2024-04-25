import axios, { AxiosResponse } from 'axios';
import fileDownload from 'js-file-download';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import * as actionTypes from '@/constants/actionTypes';
import * as API from '@/constants/API';
import { catchAxiosError } from '@/customAxios';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { useGeoServer } from '@/hooks/layer-api/useGeoServer';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyView } from '@/models/api/generated/ApiGen_Concepts_PropertyView';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { logRequest, logSuccess } from '@/store/slices/network/networkSlice';

import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

export const useProperties = () => {
  const dispatch = useDispatch();
  const {
    getPropertiesViewPagedApi,
    getPropertiesApi,
    getPropertyConceptWithIdApi,
    exportPropertiesApi: rawApiExportProperties,
  } = useApiProperties();

  const { getPropertyWfs } = useGeoServer();

  const getPropertiesFromView = useApiRequestWrapper<
    (
      propertyBounds: IPropertyFilter | null,
    ) => Promise<AxiosResponse<ApiGen_Base_Page<ApiGen_Concepts_PropertyView>>>
  >({
    requestFunction: useCallback(
      async (propertyBounds: IPropertyFilter | null) =>
        await getPropertiesViewPagedApi(propertyBounds),
      [getPropertiesViewPagedApi],
    ),
    requestName: actionTypes.GET_PARCELS,
    throwError: true,
  });

  const { execute: getPropertyWrapped, loading: getPropertyLoading } = useApiRequestWrapper<
    (id: number) => Promise<AxiosResponse<ApiGen_Concepts_Property>>
  >({
    requestFunction: useCallback(
      async (id: number) => await getPropertyConceptWithIdApi(id),
      [getPropertyConceptWithIdApi],
    ),
    requestName: actionTypes.GET_PARCELS,
    throwError: true,
  });

  const getPropertiesById = useApiRequestWrapper<
    (propertyIds: number[]) => Promise<AxiosResponse<ApiGen_Concepts_Property[]>>
  >({
    requestFunction: useCallback(
      async (propertyIds: number[]) => await getPropertiesApi(propertyIds),
      [getPropertiesApi],
    ),
    requestName: 'getProperties',
    throwError: true,
  });

  /**
   * Make an AJAX request to fetch the specified 'property' from inventory.
   * @param params Id of the property
   */
  const fetchPropertyWithId = useCallback(
    async (id: number): Promise<ApiGen_Concepts_Property> => {
      // Due to spatial information being stored in BC Albers in the database, we need to make TWO requests here:
      //   1. to the REST API to fetch property field attributes (e.g. address, etc)
      //   2. to GeoServer to fetch latitude/longitude in expected web mercator projection (EPSG:4326)
      return Promise.all([getPropertyWrapped(id), getPropertyWfs(id)]).then(
        ([propertyResponse, wfsResponse]) => {
          const [longitude, latitude] = wfsResponse?.geometry?.coordinates || [];
          const property: ApiGen_Concepts_Property = {
            ...getEmptyProperty(),
            ...propertyResponse,
            latitude,
            longitude,
          };
          return property;
        },
      );
    },
    [getPropertyWrapped, getPropertyWfs],
  );

  /**
   * Make and AJAX request to export properties that match the specified filter.
   * Upon receiving data stream from server it triggers a download prompt on client browser.
   * Currently supports CSV and Excel output formats.
   * @param filter The filter to match for exported properties.
   * @param requestId The name/id of this network request. The state is tracked in redux [loading -> success -> error]
   * @param outputFormat The output format (csv or excel).
   */
  const exportProperties = useCallback(
    async (
      filter: API.IPaginateProperties,
      outputFormat: 'csv' | 'excel' = 'excel',
      fileName = `pims-inventory.${outputFormat === 'csv' ? 'csv' : 'xlsx'}`,
      requestId = 'properties-report',
    ) => {
      dispatch(logRequest(requestId));
      dispatch(showLoading());
      try {
        const { data, status } = await rawApiExportProperties(filter, outputFormat);
        dispatch(logSuccess({ name: requestId, status }));
        dispatch(hideLoading());
        // trigger file download in client browser

        fileDownload(data, fileName);
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, rawApiExportProperties],
  );

  return {
    getProperty: fetchPropertyWithId,
    getPropertyLoading: getPropertyLoading,
    getPropertiesFromView: getPropertiesFromView,
    getMultiplePropertiesById: getPropertiesById,
    exportProperties,
  };
};
