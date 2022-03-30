import axios from 'axios';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { catchAxiosError } from 'customAxios';
import { useApiProperties } from 'hooks/pims-api';
import { useGeoServer } from 'hooks/pims-api/useGeoServer';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { logError, logRequest, logSuccess } from 'store/slices/network/networkSlice';
import { pidFormatter, pidPadded } from 'utils';
import { downloadFile } from 'utils/download';

export const useProperties = () => {
  const dispatch = useDispatch();
  const {
    getPropertiesPaged,
    getPropertyWithPid,
    exportProperties: rawApiExportProperties,
  } = useApiProperties();

  const { getPropertyWithPidWfs } = useGeoServer();

  /**
   * fetch properties, passing the current bounds of the map.
   */
  const fetchProperties = useCallback(
    async (propertyBounds: API.IPaginateProperties | null) => {
      dispatch(logRequest(actionTypes.GET_PARCELS));
      dispatch(showLoading());
      return getPropertiesPaged(propertyBounds)
        .then(response => {
          dispatch(logSuccess({ name: actionTypes.GET_PARCELS }));
          dispatch(hideLoading());
          return Promise.resolve(response);
        })
        .catch(axiosError => {
          dispatch(
            logError({
              name: actionTypes.GET_PARCELS,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
          return Promise.reject(axiosError);
        })
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch, getPropertiesPaged],
  );

  /**
   * Make an AJAX request to fetch the specified 'property' from inventory.
   * @param params PID of the property
   */
  const fetchPropertyWithPid = useCallback(
    async (pid: string): Promise<IPropertyApiModel> => {
      dispatch(logRequest(actionTypes.GET_PARCEL_DETAIL));
      dispatch(showLoading());
      // Due to spatial information being stored in BC Albers in the database, we need to make TWO requests here:
      //   1. to the REST API to fetch property field attributes (e.g. address, etc)
      //   2. to GeoServer to fetch latitude/longitude in expected web mercator projection (EPSG:4326)
      return Promise.all([
        getPropertyWithPid(pidFormatter(pid)),
        getPropertyWithPidWfs(pidPadded(pid)),
      ])
        .then(([propertyResponse, wfsResponse]) => {
          const [longitude, latitude] = wfsResponse?.geometry?.coordinates || [];
          const property: IPropertyApiModel = {
            ...propertyResponse.data,
            latitude,
            longitude,
          };
          dispatch(logSuccess({ name: actionTypes.GET_PARCEL_DETAIL }));
          dispatch(hideLoading());
          return property;
        })
        .catch(axiosError => {
          dispatch(
            logError({
              name: actionTypes.GET_PARCEL_DETAIL,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
          return Promise.reject(axiosError);
        })
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch, getPropertyWithPid, getPropertyWithPidWfs],
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
    exportProperties,
  };
};
