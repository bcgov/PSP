import { IPaginateParams } from 'constants/API';
import { IPagedItems, IUser, IUserDetails } from 'interfaces';
import React from 'react';
import * as pimsToasts from 'constants/toasts';

import { useApi } from '.';
import { LifecycleToasts } from 'customAxios';

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
  const api = useApi();
  const apiWithToasts = useApi({ lifecycleToasts: userToasts });

  return React.useMemo(
    () => ({
      activateUser: () => api.post('/auth/activate'),
      getUser: (id: string) => api.get<IUserDetails>(`/admin/users/${id}`),
      getUsersPaged: (params: IPaginateParams) =>
        api.post<IPagedItems<IUser>>(`/admin/users/my/agency`, params),
      putUser: (user: IUserDetails) => apiWithToasts.put<IUser>(`/keycloak/users/${user.id}`, user),
    }),
    [api, apiWithToasts],
  );
};
