import { IPagedItems } from '../../../interfaces/pagedItems';
import { showLoading, hideLoading } from 'react-redux-loading-bar';
import { request, success, error } from 'actions/genericActions';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import CustomAxios, { LifecycleToasts } from 'customAxios';
import { AxiosResponse, AxiosError } from 'axios';
import { IUser, IUserDetails } from 'interfaces';
import { handleAxiosResponse } from 'utils';
import * as pimsToasts from 'constants/toasts';
import { useAppDispatch } from 'store/hooks';
import { useCallback } from 'react';
import * as reducerTypes from 'constants/reducerTypes';
import { storeUserDetails, storeUsers, updateUser } from './usersSlice';

const userToasts: LifecycleToasts = {
  loadingToast: pimsToasts.user.USER_UPDATING,
  successToast: pimsToasts.user.USER_UPDATED,
  errorToast: pimsToasts.user.USER_ERROR,
};

/**
 * hook that wraps calls to the agencies api.
 */
export const useUsers = () => {
  const dispatch = useAppDispatch();

  /**
   * fetch all of the agencies from the server based on a filter.
   * @return the filtered, paged list of agencies.
   */
  const activate = useCallback(async (): Promise<IUser> => {
    dispatch(request(actionTypes.ADD_ACTIVATE_USER));
    dispatch(showLoading());
    return CustomAxios()
      .post(ENVIRONMENT.apiUrl + API.ACTIVATE_USER(), null)
      .then((response: AxiosResponse) => {
        dispatch(success(actionTypes.ADD_ACTIVATE_USER, response.status));
        dispatch(hideLoading());
        return response.data;
      })
      .catch((axiosError: AxiosError) =>
        dispatch(error(actionTypes.ADD_ACTIVATE_USER, axiosError?.response?.status, axiosError)),
      )
      .finally(() => dispatch(hideLoading()));
  }, [dispatch]);

  /**
   * fetch all of the agencies from the server based on a filter.
   * @return the filtered, paged list of agencies.
   */
  const fetch = useCallback(
    async (params: API.IPaginateParams): Promise<IPagedItems<IUser>> => {
      dispatch(request(actionTypes.GET_USERS));
      dispatch(showLoading());
      return CustomAxios()
        .post(ENVIRONMENT.apiUrl + API.POST_USERS(), params)
        .then((response: AxiosResponse) => {
          dispatch(success(actionTypes.GET_USERS));
          dispatch(storeUsers(response.data));
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(error(actionTypes.GET_USERS, axiosError?.response?.status, axiosError)),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch],
  );

  /**
   * fetch the detailed agency based on the agency id.
   * @return the detailed agency.
   */
  const fetchDetail = useCallback(
    async (id: API.IAgencyDetailParams): Promise<IUserDetails> => {
      dispatch(request(reducerTypes.GET_USER_DETAIL));
      dispatch(showLoading());
      return CustomAxios()
        .get(ENVIRONMENT.apiUrl + API.USER_DETAIL(id))
        .then((response: AxiosResponse) => {
          dispatch(success(reducerTypes.GET_USER_DETAIL));
          dispatch(storeUserDetails(response.data));
          dispatch(hideLoading());
          return response.data;
        })
        .catch(() => dispatch(error(reducerTypes.GET_USER_DETAIL)))
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch],
  );

  /**
   * Update an existing agency based on its id.
   * @return the updated agency.
   */
  const update = useCallback(
    async (
      id: API.IAgencyDetailParams,
      updatedUser: IUserDetails,
    ): Promise<AxiosResponse<IUserDetails>> => {
      const axiosPromise = CustomAxios({ lifecycleToasts: userToasts })
        .put(ENVIRONMENT.apiUrl + API.KEYCLOAK_USER_UPDATE(id), updatedUser)
        .then((response: AxiosResponse) => {
          dispatch(updateUser(response.data));
          return Promise.resolve(response);
        });

      return handleAxiosResponse(dispatch, reducerTypes.PUT_USER_DETAIL, axiosPromise).catch(() => {
        // swallow the exception, the error has already been displayed.
      });
    },
    [dispatch],
  );

  return {
    fetchUsers: fetch,
    fetchUserDetail: fetchDetail,
    updateUser: update,
    activateUser: activate,
  };
};

export const NEW_PIMS_USER = 201;
