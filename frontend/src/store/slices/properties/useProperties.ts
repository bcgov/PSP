import axios from 'axios';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { catchAxiosError } from 'customAxios';
import { useApiProperties } from 'hooks/pims-api';
import { useGeoServer } from 'hooks/pims-api/useGeoServer';
import { IProperty } from 'interfaces';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { downloadFile } from 'utils/download';

import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { storeProperties, storeProperty } from './propertiesSlice';

export const useProperties = () => {
  const dispatch = useDispatch();
  const {
    getPropertiesPaged,
    getProperty,
    postProperty,
    putProperty,
    deleteProperty,
    exportProperties: rawApiExportProperties,
  } = useApiProperties();

  const { getPropertyWfs } = useGeoServer();

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
          dispatch(storeProperties(response.data.items));
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
   * @param params unique id of the property
   * @param position optional override for the lat/lng of the returned property.
   */
  const fetchProperty = useCallback(
    async (id: number, position?: [number, number]): Promise<IProperty> => {
      dispatch(logRequest(actionTypes.GET_PARCEL_DETAIL));
      dispatch(showLoading());
      // Due to spatial information being stored in BC Albers in the database, we need to make TWO requests here:
      //   1. to the REST API to fetch property field attributes (e.g. address, etc)
      //   2. to GeoServer to fetch latitude/longitude in expected web mercator projection (EPSG:4326)
      return Promise.all([getProperty(id), getPropertyWfs(id)])
        .then(([apiProperty, wfsResponse]) => {
          const [longitude, latitude] = wfsResponse?.geometry?.coordinates || [];
          const property: IProperty = {
            ...apiProperty.data,
            latitude,
            longitude,
          };
          dispatch(logSuccess({ name: actionTypes.GET_PARCEL_DETAIL }));
          dispatch(storeProperty({ property, position }));
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
    [dispatch, getProperty, getPropertyWfs],
  );

  /**
   * Make an AJAX request to add the specified 'property' to inventory.
   * @param property IProperty object to add to inventory.
   */
  const createProperty = useCallback(
    async (property: IProperty) => {
      dispatch(logRequest(actionTypes.ADD_PARCEL));
      dispatch(showLoading());
      try {
        const { data, status } = await postProperty(property);
        dispatch(logSuccess({ name: actionTypes.ADD_PARCEL, status }));
        dispatch(storeProperty(data));
        dispatch(hideLoading());
        return data;
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.ADD_PARCEL);
        }
      }
    },
    [dispatch, postProperty],
  );

  /**
   * Make an AJAX request to update the specified 'property' from inventory, using the id.
   * @param property IProperty object to update from inventory.
   */
  const updateProperty = useCallback(
    async (property: IProperty) => {
      dispatch(logRequest(actionTypes.UPDATE_PARCEL));
      dispatch(showLoading());
      try {
        const { data, status } = await putProperty(property);
        dispatch(logSuccess({ name: actionTypes.UPDATE_PARCEL, status }));
        dispatch(storeProperty(data));
        dispatch(hideLoading());
        return data;
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.UPDATE_PARCEL);
        }
      }
    },
    [dispatch, putProperty],
  );

  /**
   * Make an AJAX request to delete the specified 'property' from inventory.
   * @param property IProperty object to delete from inventory.
   */
  const removeProperty = useCallback(
    async (property: IProperty) => {
      dispatch(logRequest(actionTypes.DELETE_PARCEL));
      dispatch(showLoading());
      try {
        const { data, status } = await deleteProperty(property);
        dispatch(logSuccess({ name: actionTypes.DELETE_PARCEL, status }));
        dispatch(storeProperty(null));
        dispatch(hideLoading());
        return data;
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, deleteProperty],
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
    getProperty: fetchProperty,
    createProperty,
    updateProperty,
    deleteProperty: removeProperty,
    exportProperties,
  };
};
