import { AxiosError, AxiosResponse } from 'axios';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { useApiOrganizations } from 'hooks/pims-api/useApiOrganizations';
import { IOrganization, IPagedItems } from 'interfaces';
import { useCallback } from 'react';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { useAppDispatch } from 'store/hooks';
import { handleAxiosResponse } from 'utils';

import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { storeOrganizationDetails, storeOrganizations } from './organizationsSlice';
/**
 * hook that wraps calls to the organizations api.
 */
export const useOrganizations = () => {
  const dispatch = useAppDispatch();
  const {
    getOrganization,
    getOrganizationsPaged,
    deleteOrganization,
    putOrganization,
    postOrganization,
  } = useApiOrganizations();
  /**
   * fetch all of the organizations from the server based on a filter.
   * @return the filtered, paged list of organizations.
   */
  const fetch = useCallback(
    async (params: API.IPaginateParams): Promise<IPagedItems<IOrganization>> => {
      dispatch(logRequest(actionTypes.GET_ORGANIZATIONS));
      dispatch(showLoading());
      return getOrganizationsPaged(params)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_ORGANIZATIONS, status: response.status }));
          dispatch(storeOrganizations(response.data));
          dispatch(hideLoading());
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(
            logError({
              name: actionTypes.GET_ORGANIZATIONS,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          ),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch, getOrganizationsPaged],
  );

  /**
   * fetch the detailed organization based on the organization id.
   * @return the detailed organization.
   */
  const fetchDetail = useCallback(
    async (id: number): Promise<IOrganization> => {
      dispatch(logRequest(actionTypes.GET_ORGANIZATION_DETAILS));
      dispatch(showLoading());
      return getOrganization(id)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_ORGANIZATION_DETAILS }));
          dispatch(storeOrganizationDetails(response.data));
          dispatch(hideLoading());
          return response.data;
        })
        .catch(() => dispatch(logError({ name: actionTypes.GET_ORGANIZATION_DETAILS })))
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch, getOrganization],
  );

  /**
   * Update an existing organization based on its id.
   * @return the updated organization.
   */
  const update = useCallback(
    async (updatedOrganization: IOrganization): Promise<AxiosResponse<IOrganization>> => {
      const axiosPromise = putOrganization(updatedOrganization).then((response: AxiosResponse) => {
        return Promise.resolve(response);
      });
      return handleAxiosResponse(dispatch, actionTypes.PUT_ORGANIZATION_DETAILS, axiosPromise);
    },
    [dispatch, putOrganization],
  );

  /**
   * Add a new organization.
   * @return the added organization.
   */
  const add = useCallback(
    async (organization: IOrganization): Promise<IOrganization> => {
      dispatch(logRequest(actionTypes.ADD_ORGANIZATION));
      dispatch(showLoading());
      try {
        const { data, status } = await postOrganization(organization);
        dispatch(logSuccess({ name: actionTypes.ADD_PARCEL, status }));
        dispatch(hideLoading());
        return data;
      } catch (axiosError) {
        dispatch(
          logError({
            name: actionTypes.ADD_ORGANIZATION,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
        dispatch(hideLoading());
        throw logError(axiosError.response?.data.details);
      }
    },
    [dispatch, postOrganization],
  );

  /**
   * Delete an organization based on its id.
   * @return the deleted organization.
   */
  const remove = useCallback(
    async (organization: IOrganization): Promise<IOrganization> => {
      dispatch(logRequest(actionTypes.DELETE_ORGANIZATION));
      dispatch(showLoading());
      return await deleteOrganization(organization)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.DELETE_ORGANIZATION, status: response.status }));
          dispatch(hideLoading());
          return response.data;
        })
        .catch((axiosError: AxiosError) => {
          dispatch(
            logError({
              name: actionTypes.DELETE_ORGANIZATION,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        })
        .finally(() => dispatch(hideLoading()));
    },
    [deleteOrganization, dispatch],
  );

  return {
    fetchOrganizations: fetch,
    fetchOrganizationDetail: fetchDetail,
    updateOrganization: update,
    addOrganization: add,
    removeOrganization: remove,
  };
};
