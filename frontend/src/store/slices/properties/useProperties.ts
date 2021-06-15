import { AxiosError, AxiosResponse } from 'axios';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { useApiProperties } from 'hooks/pims-api/useApiProperties';
import { IBuilding, IParcel } from 'interfaces';
import _ from 'lodash';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { storeBuildingDetail, storeParcelDetail, storeParcels } from './propertiesSlice';

export const useProperties = () => {
  const dispatch = useDispatch();
  const {
    getParcelDetail,
    getParcelsDetail,
    getParcels,
    putParcel,
    postParcel,
    deleteParcel,
    deleteBuilding,
    getBuilding,
  } = useApiProperties();

  /**
   * fetch parcels, passing the current bounds of the map.
   */
  const fetchParcels = useCallback(
    async (parcelBounds: API.IPropertySearchParams | null) => {
      if (
        !parcelBounds ||
        (parcelBounds?.neLatitude !== parcelBounds?.swLatitude &&
          parcelBounds?.neLongitude !== parcelBounds?.swLongitude)
      ) {
        dispatch(logRequest(actionTypes.GET_PARCELS));
        dispatch(showLoading());
        return getParcels(parcelBounds)
          .then((response: AxiosResponse) => {
            dispatch(logSuccess({ name: actionTypes.GET_PARCELS }));
            dispatch(storeParcels(response.data));
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
    [dispatch, getParcels],
  );

  /**
   * fetch parcels using search query parameters, such as pid or pin.
   * @param params
   */
  const fetchParcelsDetail = useCallback(
    async (params: API.IPropertySearchParams) => {
      dispatch(logRequest(actionTypes.GET_PARCEL_DETAIL));
      dispatch(showLoading());
      return getParcelsDetail(params)
        .then((response: AxiosResponse) => {
          if (response?.data !== undefined && response.data.length > 0) {
            dispatch(storeParcelDetail(_.first(response.data) as any));
          }
          dispatch(logSuccess({ name: actionTypes.GET_PARCEL_DETAIL }));
          dispatch(hideLoading());
          return Promise.resolve(response);
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
    [dispatch, getParcelsDetail],
  );

  /**
   * Make an AJAX request to fetch the specified 'parcel' from inventory.
   * @param params unique id of the parcel
   * @param position optional override for the lat/lng of the returned parcel.
   */
  const fetchParcelDetail = useCallback(
    async (id: number, position?: [number, number]): Promise<IParcel> => {
      dispatch(logRequest(actionTypes.GET_PARCEL_DETAIL));
      dispatch(showLoading());
      return getParcelDetail(id)
        .then((response: AxiosResponse<IParcel>) => {
          dispatch(logSuccess({ name: actionTypes.GET_PARCEL_DETAIL }));
          dispatch(storeParcelDetail({ property: response.data, position }));
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
    [dispatch, getParcelDetail],
  );

  /**
   * Make an AJAX request to fetch the specified 'building' from inventory.
   * @param params unique id of the building
   * @param position optional override for the lat/lng of the returned building.
   */
  const fetchBuildingDetail = useCallback(
    async (id: number, position?: [number, number]): Promise<IBuilding> => {
      dispatch(logRequest(actionTypes.GET_PARCEL_DETAIL));
      dispatch(showLoading());
      return getBuilding(id)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_PARCEL_DETAIL }));
          dispatch(storeBuildingDetail({ property: response.data, position }));
          dispatch(hideLoading());
          console.log(response);
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
    [dispatch, getBuilding],
  );

  /**
   * Make an AJAX request to fetch the specified property ('parcel' or 'building') from inventory.
   * @param id unique id of the property
   * @param propertyTypeId either 0 for parcel or 1 for building
   * @param position optional override for the lat/lng of the returned property.
   */
  const fetchPropertyDetail = useCallback(
    async (id: number, propertyTypeId: 0 | 1, position?: [number, number]) => {
      return propertyTypeId === 0
        ? fetchParcelDetail(id, position)
        : fetchBuildingDetail(id, position);
    },
    [fetchParcelDetail, fetchBuildingDetail],
  );

  /**
   * Make an AJAX request to add the specified 'parcel' to inventory.
   * @param parcel IParcel object to add to inventory.
   */
  const createParcel = useCallback(
    async (parcel: IParcel) => {
      dispatch(logRequest(actionTypes.ADD_PARCEL));
      dispatch(showLoading());
      try {
        const { data, status } = await postParcel(parcel);
        dispatch(logSuccess({ name: actionTypes.ADD_PARCEL, status }));
        dispatch(storeParcelDetail(data));
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
    [dispatch, postParcel],
  );

  /**
   * Make an AJAX request to update the specified 'parcel' from inventory, using the id.
   * @param parcel IParcel object to update from inventory.
   */
  const updateParcel = useCallback(
    async (parcel: IParcel) => {
      dispatch(logRequest(actionTypes.UPDATE_PARCEL));
      dispatch(showLoading());
      try {
        const { data, status } = await putParcel(parcel);
        dispatch(logSuccess({ name: actionTypes.UPDATE_PARCEL, status }));
        dispatch(storeParcelDetail(data));
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
    [dispatch, putParcel],
  );

  /**
   * Make an AJAX request to delete the specified 'parcel' from inventory.
   * @param parcel IParcel object to delete from inventory.
   */
  const removeParcel = useCallback(
    async (parcel: IParcel) => {
      dispatch(logRequest(actionTypes.DELETE_PARCEL));
      dispatch(showLoading());
      try {
        const { data, status } = await deleteParcel(parcel);
        dispatch(logSuccess({ name: actionTypes.DELETE_PARCEL, status }));
        dispatch(storeParcelDetail(null));
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
    [dispatch, deleteParcel],
  );

  /**
   * Make an AJAX request to delete the specified 'building' from inventory.
   * @param parcel IBuilding object to delete from inventory.
   */
  const removeBuilding = useCallback(
    async (building: IBuilding) => {
      dispatch(logRequest(actionTypes.DELETE_BUILDING));
      dispatch(showLoading());
      try {
        const { data, status } = await deleteBuilding(building);
        dispatch(logSuccess({ name: actionTypes.DELETE_PARCEL, status }));
        dispatch(storeParcelDetail(null));
        dispatch(hideLoading());
        return data;
      } catch (axiosError) {
        dispatch(
          logError({
            name: actionTypes.DELETE_PARCEL,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
        dispatch(hideLoading());
        throw Error(axiosError.response?.data.details);
      }
    },
    [dispatch, deleteBuilding],
  );

  return {
    removeParcel,
    removeBuilding,
    updateParcel,
    createParcel,
    fetchPropertyDetail,
    fetchBuildingDetail,
    fetchParcelDetail,
    fetchParcelsDetail,
    fetchParcels,
  };
};
