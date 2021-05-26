import { showLoading, hideLoading } from 'react-redux-loading-bar';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { AxiosResponse, AxiosError } from 'axios';
import { IAgency, IAgencyDetail, IPagedItems } from 'interfaces';
import { handleAxiosResponse } from 'utils';
import { useAppDispatch } from 'store/hooks';
import { useCallback } from 'react';
import { storeAgencies, storeAgencyDetails } from './agenciesSlice';
import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { useApiAgencies } from 'hooks/pims-api/useApiAgencies';
/**
 * hook that wraps calls to the agencies api.
 */
export const useAgencies = () => {
  const dispatch = useAppDispatch();
  const { getAgency, getAgenciesPaged, deleteAgency, putAgency, postAgency } = useApiAgencies();
  /**
   * fetch all of the agencies from the server based on a filter.
   * @return the filtered, paged list of agencies.
   */
  const fetch = useCallback(
    async (params: API.IPaginateParams): Promise<IPagedItems<IAgency>> => {
      dispatch(logRequest(actionTypes.GET_AGENCIES));
      dispatch(showLoading());
      return getAgenciesPaged(params)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_AGENCIES, status: response.status }));
          dispatch(storeAgencies(response.data));
          dispatch(hideLoading());
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(
            logError({
              name: actionTypes.GET_AGENCIES,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          ),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch, getAgenciesPaged],
  );

  /**
   * fetch the detailed agency based on the agency id.
   * @return the detailed agency.
   */
  const fetchDetail = useCallback(
    async (id: number): Promise<IAgencyDetail> => {
      dispatch(logRequest(actionTypes.GET_AGENCY_DETAILS));
      dispatch(showLoading());
      return getAgency(id)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_AGENCY_DETAILS }));
          dispatch(storeAgencyDetails(response.data));
          dispatch(hideLoading());
          return response.data;
        })
        .catch(() => dispatch(logError({ name: actionTypes.GET_AGENCY_DETAILS })))
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch, getAgency],
  );

  /**
   * Update an existing agency based on its id.
   * @return the updated agency.
   */
  const update = useCallback(
    async (updatedAgency: IAgencyDetail): Promise<AxiosResponse<IAgencyDetail>> => {
      const axiosPromise = putAgency(updatedAgency).then((response: AxiosResponse) => {
        return Promise.resolve(response);
      });
      return handleAxiosResponse(
        dispatch,
        actionTypes.PUT_AGENCY_DETAILS,
        axiosPromise,
      ).catch(() => {});
    },
    [dispatch, putAgency],
  );

  /**
   * Add a new agency.
   * @return the added agency.
   */
  const add = useCallback(
    async (agency: IAgencyDetail): Promise<IAgency> => {
      dispatch(logRequest(actionTypes.ADD_AGENCY));
      dispatch(showLoading());
      try {
        const { data, status } = await postAgency(agency);
        dispatch(logSuccess({ name: actionTypes.ADD_PARCEL, status }));
        dispatch(hideLoading());
        return data;
      } catch (axiosError) {
        dispatch(
          logError({
            name: actionTypes.ADD_AGENCY,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
        dispatch(hideLoading());
        throw logError(axiosError.response?.data.details);
      }
    },
    [dispatch, postAgency],
  );

  /**
   * Delete an agency based on its id.
   * @return the deleted agency.
   */
  const remove = useCallback(
    async (agency: IAgency): Promise<IAgencyDetail> => {
      dispatch(logRequest(actionTypes.DELETE_AGENCY));
      dispatch(showLoading());
      return await deleteAgency(agency)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.DELETE_AGENCY, status: response.status }));
          dispatch(hideLoading());
          return response.data;
        })
        .catch((axiosError: AxiosError) => {
          dispatch(
            logError({
              name: actionTypes.DELETE_AGENCY,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          );
        })
        .finally(() => dispatch(hideLoading()));
    },
    [deleteAgency, dispatch],
  );

  return {
    fetchAgencies: fetch,
    fetchAgencyDetail: fetchDetail,
    updateAgency: update,
    addAgency: add,
    removeAgency: remove,
  };
};
