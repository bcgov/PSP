import { AxiosError, AxiosResponse } from 'axios';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { useApiUsers } from 'hooks/pims-api/useApiUsers';
import { IPagedItems, IUser } from 'interfaces';
import { useCallback } from 'react';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { useAppDispatch } from 'store/hooks';
import { handleAxiosResponse } from 'utils';

import { logError, logRequest, logSuccess } from '../network/networkSlice';
import { storeUserDetails, storeUsers, updateUser } from './usersSlice';

/**
 * hook that wraps calls to the users api.
 */
export const useUsers = () => {
  const dispatch = useAppDispatch();
  const { getUser, getUsersPaged, activateUser, putUser } = useApiUsers();

  /**
   * fetch all of the users from the server based on a filter.
   * @return the filtered, paged list of users.
   */
  const activate = useCallback(async (): Promise<IUser> => {
    dispatch(logRequest(actionTypes.ADD_ACTIVATE_USER));
    dispatch(showLoading());
    return activateUser()
      .then((response: AxiosResponse) => {
        dispatch(logSuccess({ name: actionTypes.ADD_ACTIVATE_USER, status: response.status }));
        dispatch(hideLoading());
        return response.data;
      })
      .catch((axiosError: AxiosError) =>
        dispatch(
          logError({
            name: actionTypes.ADD_ACTIVATE_USER,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        ),
      )
      .finally(() => dispatch(hideLoading()));
  }, [dispatch, activateUser]);

  /**
   * fetch all of the users from the server based on a filter.
   * @return the filtered, paged list of users.
   */
  const fetch = useCallback(
    async (params: API.IPaginateParams): Promise<IPagedItems<IUser>> => {
      dispatch(logRequest(actionTypes.GET_USERS));
      dispatch(showLoading());
      return getUsersPaged(params)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_USERS }));
          dispatch(storeUsers(response.data));
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(
            logError({
              name: actionTypes.GET_USERS,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          ),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch, getUsersPaged],
  );

  /**
   * fetch the detailed user based on the user id.
   * @return the detailed user.
   */
  const fetchDetail = useCallback(
    async (key: string): Promise<IUser> => {
      dispatch(logRequest(actionTypes.GET_USER_DETAIL));
      dispatch(showLoading());
      return getUser(key)
        .then((response: AxiosResponse) => {
          dispatch(logSuccess({ name: actionTypes.GET_USER_DETAIL }));
          dispatch(storeUserDetails(response.data));
          dispatch(hideLoading());
          return response.data;
        })
        .catch((axiosError: AxiosError) =>
          dispatch(
            logError({
              name: actionTypes.GET_USER_DETAIL,
              status: axiosError?.response?.status,
              error: axiosError,
            }),
          ),
        )
        .finally(() => dispatch(hideLoading()));
    },
    [dispatch, getUser],
  );

  /**
   * Update an existing user based on its id.
   * @return the updated user.
   */
  const update = useCallback(
    async (updatedUser: IUser): Promise<IUser> => {
      const axiosPromise = putUser(updatedUser).then((response: AxiosResponse) => {
        dispatch(updateUser(response.data));
        return Promise.resolve(response);
      });

      return handleAxiosResponse(dispatch, actionTypes.PUT_USER_DETAIL, axiosPromise).catch(() => {
        // swallow the exception, the error has already been displayed.
      });
    },
    [dispatch, putUser],
  );

  return {
    fetchUsers: fetch,
    fetchUserDetail: fetchDetail,
    updateUser: update,
    activateUser: activate,
  };
};

export const NEW_PIMS_USER = 201;
