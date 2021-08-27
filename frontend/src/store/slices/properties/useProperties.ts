import { AxiosError, AxiosResponse } from 'axios';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { useApiProperties } from 'hooks/pims-api';
import { IProperty } from 'interfaces';
import _ from 'lodash';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { storeProperties, storeProperty } from './propertiesSlice';

export const useProperties = () => {
  const dispatch = useDispatch();
  const {
    getProperties,
    getProperty,
    postProperty,
    putProperty,
    deleteProperty,
  } = useApiProperties();

  /**
   * fetch propertys, passing the current bounds of the map.
   */
  const fetchProperties = useCallback(
    async (propertyBounds: API.IPropertySearchParams | null) => {
      if (
        !propertyBounds ||
        (propertyBounds?.neLatitude !== propertyBounds?.swLatitude &&
          propertyBounds?.neLongitude !== propertyBounds?.swLongitude)
      ) {
        dispatch(logRequest(actionTypes.GET_PARCELS));
        dispatch(showLoading());
        return getProperties(propertyBounds)
          .then((response: AxiosResponse) => {
            dispatch(logSuccess({ name: actionTypes.GET_PARCELS }));
            dispatch(storeProperties(response.data));
            dispatch(hideLoading());
            return Promise.resolve(response);
          })
          .catch((axiosError: AxiosError) => {
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
      }

      return Promise.resolve();
    },
    [dispatch, getProperties],
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
      return getProperty(id)
        .then((response: AxiosResponse<IProperty>) => {
          dispatch(logSuccess({ name: actionTypes.GET_PARCEL_DETAIL }));
          dispatch(storeProperty({ property: response.data, position }));
          dispatch(hideLoading());
          return response.data;
        })
        .catch((axiosError: AxiosError) => {
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
    [dispatch, getProperty],
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
        dispatch(
          logError({
            name: actionTypes.ADD_PARCEL,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
        dispatch(hideLoading());
        throw Error(axiosError.response?.data.details);
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
        dispatch(
          logError({
            name: actionTypes.UPDATE_PARCEL,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
        dispatch(hideLoading());
        throw Error(axiosError.response?.data.details);
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
        dispatch(
          logError({
            name: actionTypes.DELETE_PARCEL,
            status: axiosError.response?.status,
            error: axiosError,
          }),
        );
        dispatch(hideLoading());
        throw Error(axiosError.response?.data.details);
      }
    },
    [dispatch, deleteProperty],
  );

  return {
    getProperties: fetchProperties,
    getProperty: fetchProperty,
    createProperty,
    updateProperty,
    deleteProperty: removeProperty,
  };
};
