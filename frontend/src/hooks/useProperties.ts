import axios, { AxiosResponse } from 'axios';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { catchAxiosError } from 'customAxios';
import { useApiProperties } from 'hooks/pims-api';
import { useGeoServer } from 'hooks/pims-api/useGeoServer';
import { IPagedItems, IProperty } from 'interfaces';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { logRequest, logSuccess } from 'store/slices/network/networkSlice';
import { pidFormatter, pidPadded } from 'utils';
import { downloadFile } from 'utils/download';

import { useApiRequestWrapper } from './pims-api/useApiRequestWrapper';

const ignoreErrorCodes = [404];

export const useProperties = () => {
  const dispatch = useDispatch();
  const {
    getPropertiesPaged,
    getPropertyWithPid,
    exportProperties: rawApiExportProperties,
  } = useApiProperties();

  const { getPropertyWithPidWfs } = useGeoServer();

  const fetchProperties = useApiRequestWrapper<
    (
      propertyBounds: API.IPaginateProperties | null,
    ) => Promise<AxiosResponse<IPagedItems<IProperty>>>
  >({
    requestFunction: useCallback(
      async (propertyBounds: API.IPaginateProperties | null) =>
        await getPropertiesPaged(propertyBounds),
      [getPropertiesPaged],
    ),
    requestName: actionTypes.GET_PARCELS,
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const {
    execute: getPropertyWithPidWrapped,
    loading: getPropertyWithPidLoading,
  } = useApiRequestWrapper<(pid: string) => Promise<AxiosResponse<IPropertyApiModel>>>({
    requestFunction: useCallback(
      async (pid: string) => await getPropertyWithPid(pidFormatter(pid)),
      [getPropertyWithPid],
    ),
    requestName: actionTypes.GET_PARCELS,
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  /**
   * Make an AJAX request to fetch the specified 'property' from inventory.
   * @param params PID of the property
   */
  const fetchPropertyWithPid = useCallback(
    async (pid: string): Promise<IPropertyApiModel> => {
      // Due to spatial information being stored in BC Albers in the database, we need to make TWO requests here:
      //   1. to the REST API to fetch property field attributes (e.g. address, etc)
      //   2. to GeoServer to fetch latitude/longitude in expected web mercator projection (EPSG:4326)
      return Promise.all([
        getPropertyWithPidWrapped(pidFormatter(pid)),
        getPropertyWithPidWfs(pidPadded(pid)),
      ]).then(([propertyResponse, wfsResponse]) => {
        const [longitude, latitude] = wfsResponse?.geometry?.coordinates || [];
        const property: IPropertyApiModel = {
          ...propertyResponse,
          latitude,
          longitude,
        };
        return property;
      });
    },
    [getPropertyWithPidWrapped, getPropertyWithPidWfs],
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
        downloadFile(fileName, data);
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, rawApiExportProperties],
  );

  return {
    getProperties: fetchProperties,
    getPropertyWithPid: fetchPropertyWithPid,
    getPropertyWithPidLoading: getPropertyWithPidLoading,
    exportProperties,
  };
};
