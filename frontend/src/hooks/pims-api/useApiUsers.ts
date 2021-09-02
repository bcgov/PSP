import { IPaginateParams } from 'constants/API';
import * as pimsToasts from 'constants/toasts';
import { LifecycleToasts } from 'customAxios';
import { IPagedItems, IUser } from 'interfaces';
import React from 'react';

import { useApi } from '.';

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
      getUser: (key: string) => api.get<IUser>(`/admin/users/${key}`),
      getUsersPaged: (params: IPaginateParams) =>
        api.post<IPagedItems<IUser>>(`/admin/users/my/organization`, params),
      putUser: (user: IUser) =>
        apiWithToasts.put<IUser>(`/keycloak/users/${user.keycloakUserId}`, user),
    }),
    [api, apiWithToasts],
  );
};
