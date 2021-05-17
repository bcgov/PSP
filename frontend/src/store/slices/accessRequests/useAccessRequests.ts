import { useAppDispatch } from './../../hooks';
import { showLoading, hideLoading } from 'react-redux-loading-bar';
import CustomAxios from 'customAxios';
import { ENVIRONMENT } from 'constants/environment';
import { AxiosResponse, AxiosError } from 'axios';
import { IAccessRequest } from 'interfaces';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import React from 'react';
import {
  updateAccessRequestsAdmin,
  storeAccessRequests,
  deleteAccessRequest,
  storeAccessRequest,
} from './accessRequestsSlice';
import { logRequest, logSuccess, logError } from '../network/networkSlice';

export const useAccessRequests = () => {
  const dispatch = useAppDispatch();
  /**
   * Get the fetchCurrent function with returns the current access request for the current user.
   * @returns The dispatchable action which will return the access request if one exists, or 204 if one doesn't
   */
  const fetchCurrent = React.useCallback(async (): Promise<IAccessRequest> => {
    dispatch(logRequest(actionTypes.GET_REQUEST_ACCESS));
    dispatch(showLoading());
    return CustomAxios()
      .get(ENVIRONMENT.apiUrl + API.REQUEST_ACCESS())
      .then((response: AxiosResponse) => {
        dispatch(logSuccess({ name: actionTypes.GET_REQUEST_ACCESS }));
        dispatch(storeAccessRequest(response.data));
        return response.data;
      })
      .catch((axiosError: AxiosError) =>
        dispatch(
          logError({
            name: actionTypes.GET_REQUEST_ACCESS,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        ),
      )
      .finally(() => dispatch(hideLoading()));
  }, [dispatch]);

  /**
   * Get the storeAccessRequest action.
   * If the 'accessRequest' is new return the POST action.
   * If the 'accessRequest' exists, return the PUT action.
   * @param accessRequest - The access request to add
   * @returns The action function to submit the access request
   */
  const add = React.useCallback(
    async (accessRequest: IAccessRequest): Promise<IAccessRequest> => {
      dispatch(logRequest(actionTypes.ADD_REQUEST_ACCESS));
      dispatch(showLoading());

      return CustomAxios()
        .request({
          url: ENVIRONMENT.apiUrl + API.REQUEST_ACCESS(accessRequest.id),
          method: accessRequest.id === 0 ? 'post' : 'put',
          data: accessRequest,
        })
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.ADD_REQUEST_ACCESS, status: response.status }));
          dispatch(storeAccessRequest(response.data));
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(
            logError({
              name: actionTypes.ADD_REQUEST_ACCESS,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          ),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch],
  );

  /**
   * Get the update action which updates the passed access request (therefore it must have an id)
   * @returns The dispatchable action which will update the access request.
   */
  const update = React.useCallback(
    async (accessRequest: IAccessRequest): Promise<IAccessRequest> => {
      dispatch(logRequest(actionTypes.UPDATE_REQUEST_ACCESS_ADMIN));
      dispatch(showLoading());
      return CustomAxios()
        .put(ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_ADMIN(), accessRequest)
        .then((response: AxiosResponse) => {
          dispatch(
            logSuccess({ name: actionTypes.UPDATE_REQUEST_ACCESS_ADMIN, status: response.status }),
          );
          dispatch(updateAccessRequestsAdmin(response.data));
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(
            logError({
              name: actionTypes.UPDATE_REQUEST_ACCESS_ADMIN,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          ),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch],
  );

  /**
   * Get the fetch action which returns a list of all access requests paged and filtered by the passed parameters.
   * @returns The dispatchable action which will return a paged list of 0 or more access requests
   */
  const fetch = React.useCallback(
    async (params: API.IPaginateAccessRequests): Promise<IAccessRequest[]> => {
      dispatch(logRequest(actionTypes.GET_REQUEST_ACCESS));
      dispatch(showLoading());
      return CustomAxios()
        .get(ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_LIST(params))
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_REQUEST_ACCESS, status: response.status }));
          dispatch(storeAccessRequests(response.data));

          dispatch(hideLoading());
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(
            logError({
              name: actionTypes.GET_REQUEST_ACCESS,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          ),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch],
  );

  /**
   * Get the remove action which will delete the passed access request with matching id.
   * @returns The dispatchable action which will delete the matching access request.
   */
  const remove = React.useCallback(
    async (id: number, data: IAccessRequest): Promise<IAccessRequest> => {
      dispatch(logRequest(actionTypes.DELETE_REQUEST_ACCESS_ADMIN));
      dispatch(showLoading());
      return CustomAxios()
        .request({
          method: 'DELETE',
          url: ENVIRONMENT.apiUrl + API.REQUEST_ACCESS_DELETE(id),
          data,
        })
        .then((response: AxiosResponse) => {
          dispatch(
            logSuccess({ name: actionTypes.DELETE_REQUEST_ACCESS_ADMIN, status: response.status }),
          );
          dispatch(deleteAccessRequest(id));
          dispatch(hideLoading());
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(
            logError({
              name: actionTypes.DELETE_REQUEST_ACCESS_ADMIN,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          ),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch],
  );

  return {
    addAccessRequest: add,
    updateAccessRequest: update,
    fetchAccessRequests: fetch,
    fetchCurrentAccessRequest: fetchCurrent,
    removeAccessRequest: remove,
  };
};
