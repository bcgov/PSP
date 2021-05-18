import { showLoading, hideLoading } from 'react-redux-loading-bar';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import CustomAxios, { LifecycleToasts } from 'customAxios';
import { AxiosResponse, AxiosError } from 'axios';
import * as pimsToasts from 'constants/toasts';
import _ from 'lodash';
import { useDispatch } from 'react-redux';
import { useCallback } from 'react';
import { storeBuildingDetail, storeParcelDetail, storeParcels } from './propertiesSlice';
import { IParcel, IBuilding } from 'interfaces';
import { logRequest, logSuccess, logError } from '../network/networkSlice';

const parcelCreatingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_CREATING,
  successToast: pimsToasts.parcel.PARCEL_CREATED,
  errorToast: pimsToasts.parcel.PARCEL_CREATING_ERROR,
};

const parcelDeletingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_DELETING,
  successToast: pimsToasts.parcel.PARCEL_DELETED,
  errorToast: pimsToasts.parcel.PARCEL_DELETING_ERROR,
};

const parcelUpdatingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.parcel.PARCEL_UPDATING,
  successToast: pimsToasts.parcel.PARCEL_UPDATED,
  errorToast: pimsToasts.parcel.PARCEL_UPDATING_ERROR,
};

const buildingDeletingToasts: LifecycleToasts = {
  loadingToast: pimsToasts.building.BUILDING_DELETING,
  successToast: pimsToasts.building.BUILDING_DELETED,
  errorToast: pimsToasts.building.BUILDING_DELETING_ERROR,
};

export const useProperties = () => {
  const dispatch = useDispatch();

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
        return CustomAxios()
          .get(ENVIRONMENT.apiUrl + API.PROPERTIES(parcelBounds))
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
    [dispatch],
  );

  /**
   * fetch parcels using search query parameters, such as pid or pin.
   * @param params
   */
  const fetchParcelsDetail = useCallback(
    async (params: API.IPropertySearchParams) => {
      dispatch(logRequest(actionTypes.GET_PARCEL_DETAIL));
      dispatch(showLoading());
      return CustomAxios()
        .get(ENVIRONMENT.apiUrl + API.PARCELS_DETAIL(params))
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
    [dispatch],
  );

  /**
   * Make an AJAX request to fetch the specified 'parcel' from inventory.
   * @param params unique id of the parcel
   * @param position optional override for the lat/lng of the returned parcel.
   */
  const fetchParcelDetail = useCallback(
    async (params: API.IParcelDetailParams, position?: [number, number]): Promise<IParcel> => {
      dispatch(logRequest(actionTypes.GET_PARCEL_DETAIL));
      dispatch(showLoading());
      return CustomAxios()
        .get<IParcel>(ENVIRONMENT.apiUrl + API.PARCEL_DETAIL(params))
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
    [dispatch],
  );

  /**
   * Make an AJAX request to fetch the specified 'building' from inventory.
   * @param params unique id of the building
   * @param position optional override for the lat/lng of the returned building.
   */
  const fetchBuildingDetail = useCallback(
    async (params: API.IBuildingDetailParams, position?: [number, number]): Promise<IBuilding> => {
      dispatch(logRequest(actionTypes.GET_PARCEL_DETAIL));
      dispatch(showLoading());
      return CustomAxios()
        .get<IBuilding>(ENVIRONMENT.apiUrl + API.BUILDING_DETAIL(params))
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_PARCEL_DETAIL }));
          dispatch(storeBuildingDetail({ property: response.data, position }));
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
    [dispatch],
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
        ? fetchParcelDetail({ id }, position)
        : fetchBuildingDetail({ id }, position);
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
        const { data, status } = await CustomAxios({ lifecycleToasts: parcelCreatingToasts }).post(
          ENVIRONMENT.apiUrl + API.PARCEL_ROOT,
          parcel,
        );
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
    [dispatch],
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
        const { data, status } = await CustomAxios({ lifecycleToasts: parcelUpdatingToasts }).put(
          ENVIRONMENT.apiUrl + API.PARCEL_ROOT + `/${parcel.id}`,
          parcel,
        );
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
    [dispatch],
  );

  /**
   * Make an AJAX request to delete the specified 'parcel' from inventory.
   * @param parcel IParcel object to delete from inventory.
   */
  const deleteParcel = useCallback(
    async (parcel: IParcel) => {
      dispatch(logRequest(actionTypes.DELETE_PARCEL));
      dispatch(showLoading());
      try {
        const { data, status } = await CustomAxios({
          lifecycleToasts: parcelDeletingToasts,
        }).delete(ENVIRONMENT.apiUrl + API.PARCEL_ROOT + `/${parcel.id}`, { data: parcel });
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
    [dispatch],
  );

  /**
   * Make an AJAX request to delete the specified 'building' from inventory.
   * @param parcel IBuilding object to delete from inventory.
   */
  const deleteBuilding = useCallback(
    async (building: IBuilding) => {
      dispatch(logRequest(actionTypes.DELETE_BUILDING));
      dispatch(showLoading());
      try {
        const { data, status } = await CustomAxios({
          lifecycleToasts: buildingDeletingToasts,
        }).delete(ENVIRONMENT.apiUrl + API.BUILDING_ROOT + `/${building.id}`, { data: building });
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
    [dispatch],
  );

  return {
    deleteParcel,
    deleteBuilding,
    updateParcel,
    createParcel,
    fetchPropertyDetail,
    fetchBuildingDetail,
    fetchParcelDetail,
    fetchParcelsDetail,
    fetchParcels,
  };
};
