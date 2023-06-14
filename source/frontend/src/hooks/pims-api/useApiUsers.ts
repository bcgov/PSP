import queryString from 'query-string';
import React from 'react';

import { IPaginateParams } from '@/constants/API';
import * as pimsToasts from '@/constants/toasts';
import { LifecycleToasts } from '@/customAxios';
import { IPagedItems, IUser } from '@/interfaces';
import { Api_User } from '@/models/api/User';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the user endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

const userToasts: LifecycleToasts = {
  loadingToast: pimsToasts.user.USER_UPDATING,
  successToast: pimsToasts.user.USER_UPDATED,
  errorToast: pimsToasts.user.USER_ERROR,
};

export const useApiUsers = () => {
  const api = useAxiosApi();
  const apiWithToasts = useAxiosApi({ lifecycleToasts: userToasts });

  return React.useMemo(
    () => ({
      activateUser: () => api.post<IUser>('/auth/activate'),
      getUser: (key: string) => api.get<Api_User>(`/admin/users/${key}`),
      getUserInfo: (key: string) => api.get<Api_User>(`/users/info/${key}`),
      getUsersPaged: (params: IPaginateParams) =>
        api.post<IPagedItems<Api_User>>(`/admin/users/filter`, params),
      putUser: (user: Api_User) =>
        apiWithToasts.put<Api_User>(`/keycloak/users/${user.guidIdentifierValue}`, user),
      exportUsers: (filter: IPaginateParams, accept: string) =>
        api.get<Blob>(
          `/reports/users?${filter ? queryString.stringify({ ...filter, all: true }) : ''}`,
          {
            responseType: 'blob',
            headers: {
              Accept: accept === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
            },
          },
        ),
    }),
    [api, apiWithToasts],
  );
};
