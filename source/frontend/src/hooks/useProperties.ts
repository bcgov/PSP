import axios, { AxiosResponse } from 'axios';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { catchAxiosError } from 'customAxios';
import { IPropertyFilter } from 'features/properties/filter/IPropertyFilter';
import { useApiProperties } from 'hooks/pims-api';
import { useGeoServer } from 'hooks/pims-api/useGeoServer';
import { IPagedItems, IProperty } from 'interfaces';
import fileDownload from 'js-file-download';
import { Api_Property } from 'models/api/Property';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { logRequest, logSuccess } from 'store/slices/network/networkSlice';

import { useApiRequestWrapper } from './pims-api/useApiRequestWrapper';

const ignoreErrorCodes = [404];

export const useProperties = () => {
  const dispatch = useDispatch();
  const {
    getPropertiesPaged,
    getPropertyConceptWithId,
    exportProperties: rawApiExportProperties,
  } = useApiProperties();

  const { getPropertyWfs } = useGeoServer();

  const fetchProperties = useApiRequestWrapper<
    (propertyBounds: IPropertyFilter | null) => Promise<AxiosResponse<IPagedItems<IProperty>>>
  >({
    requestFunction: useCallback(
      async (propertyBounds: IPropertyFilter | null) => await getPropertiesPaged(propertyBounds),
      [getPropertiesPaged],
    ),
    requestName: actionTypes.GET_PARCELS,
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const { execute: getPropertyWrapped, loading: getPropertyLoading } = useApiRequestWrapper<
    (id: number) => Promise<AxiosResponse<Api_Property>>
  >({
    requestFunction: useCallback(
      async (id: number) => await getPropertyConceptWithId(id),
      [getPropertyConceptWithId],
    ),
    requestName: actionTypes.GET_PARCELS,
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  /**
   * Make an AJAX request to fetch the specified 'property' from inventory.
   * @param params Id of the property
   */
  const fetchPropertyWithId = useCallback(
    async (id: number): Promise<Api_Property> => {
      // Due to spatial information being stored in BC Albers in the database, we need to make TWO requests here:
      //   1. to the REST API to fetch property field attributes (e.g. address, etc)
      //   2. to GeoServer to fetch latitude/longitude in expected web mercator projection (EPSG:4326)
      return Promise.all([getPropertyWrapped(id), getPropertyWfs(id)]).then(
        ([propertyResponse, wfsResponse]) => {
          const [longitude, latitude] = wfsResponse?.geometry?.coordinates || [];
          const property: Api_Property = {
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
    getProperties: fetchProperties,
    exportProperties,
  };
};
