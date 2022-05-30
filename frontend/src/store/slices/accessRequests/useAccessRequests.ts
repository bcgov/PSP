import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { useApiAccessRequests } from 'hooks/pims-api';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IAccessRequest, IPagedItems } from 'interfaces';
import React from 'react';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import { IGenericNetworkAction } from '../network/interfaces';
import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { useAppDispatch } from './../../hooks';
import {
  deleteAccessRequest as removeAccessRequest,
  storeAccessRequests,
  updateAccessRequestsAdmin,
} from './accessRequestsSlice';

export const useAccessRequests = () => {
  const dispatch = useAppDispatch();
  const {
    getAccessRequest,
    getAccessRequestsPaged,
    postAccessRequest,
    putAccessRequest,
    deleteAccessRequest,
  } = useApiAccessRequests();
  /**
   * Get the fetchCurrent function with returns the current access request for the current user.
   * @returns The dispatchable action which will return the access request if one exists, or 204 if one doesn't
   */
  const fetchCurrent = useApiRequestWrapper({
    requestFunction: getAccessRequest,
    requestName: actionTypes.GET_REQUEST_ACCESS,
  });

  /**
   * Get the storeAccessRequest action.
   * If the 'accessRequest' is new return the POST action.
   * If the 'accessRequest' exists, return the PUT action.
   * @param accessRequest - The access request to add
   * @returns The action function to submit the access request
   */
  const add = useApiRequestWrapper({
    requestFunction: postAccessRequest,
    requestName: actionTypes.ADD_REQUEST_ACCESS,
  });

  /**
   * Get the update action which updates the passed access request (therefore it must have an id)
   * @returns The dispatchable action which will update the access request.
   */
  const update = React.useCallback(
    async (
      accessRequest: IAccessRequest,
    ): Promise<IAccessRequest | { payload: IGenericNetworkAction; type: string }> => {
      dispatch(logRequest(actionTypes.UPDATE_REQUEST_ACCESS_ADMIN));
      dispatch(showLoading());
      return putAccessRequest(accessRequest)
        .then(response => {
          dispatch(
            logSuccess({
              name: actionTypes.UPDATE_REQUEST_ACCESS_ADMIN,
              status: response.status,
            }),
          );
          dispatch(updateAccessRequestsAdmin(response.data));
          return response.data;
        })
        .catch(axiosError =>
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
    [dispatch, putAccessRequest],
  );

  /**
   * Get the fetch action which returns a list of all access requests paged and filtered by the passed parameters.
   * @returns The dispatchable action which will return a paged list of 0 or more access requests
   */
  const fetch = React.useCallback(
    async (
      params: API.IPaginateAccessRequests,
    ): Promise<IPagedItems<IAccessRequest> | { payload: IGenericNetworkAction; type: string }> => {
      dispatch(logRequest(actionTypes.GET_REQUEST_ACCESS));
      dispatch(showLoading());
      return getAccessRequestsPaged(params)
        .then(response => {
          dispatch(logSuccess({ name: actionTypes.GET_REQUEST_ACCESS, status: response.status }));
          dispatch(storeAccessRequests(response.data));

          dispatch(hideLoading());
          return response.data;
        })
        .catch(axiosError =>
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
    [dispatch, getAccessRequestsPaged],
  );

  /**
   * Get the remove action which will delete the passed access request with matching id.
   * @returns The dispatchable action which will delete the matching access request.
   */
  const remove = React.useCallback(
    async (
      id: number,
      data: IAccessRequest,
    ): Promise<IAccessRequest | { payload: IGenericNetworkAction; type: string }> => {
      dispatch(logRequest(actionTypes.DELETE_REQUEST_ACCESS_ADMIN));
      dispatch(showLoading());
      return deleteAccessRequest(data)
        .then(response => {
          dispatch(
            logSuccess({
              name: actionTypes.DELETE_REQUEST_ACCESS_ADMIN,
              status: response.status,
            }),
          );
          dispatch(removeAccessRequest(id));
          dispatch(hideLoading());
          return response.data;
        })
        .catch(axiosError =>
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
    [dispatch, deleteAccessRequest],
  );

  return {
    addAccessRequest: add,
    updateAccessRequest: update,
    fetchAccessRequests: fetch,
    fetchCurrentAccessRequest: fetchCurrent,
    removeAccessRequest: remove,
  };
};
