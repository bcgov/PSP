import { AxiosError, AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';
import { toast } from 'react-toastify';

import { IPaginateParams } from '@/constants/API';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { IUsersFilter } from '@/interfaces/IUsersFilter';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';

/**
 * hook that retrieves user information.
 */
export const useUserInfoRepository = () => {
  const { getUserInfo, getUserLookup } = useApiUsers();

  const {
    execute: retrieveUserInfo,
    loading: retrieveUserInfoLoading,
    response: retrieveUserInfoResponse,
  } = useApiRequestWrapper<(userGuid: string) => Promise<AxiosResponse<ApiGen_Concepts_User, any>>>(
    {
      requestFunction: useCallback(
        async (userGuid: string) => await getUserInfo(userGuid),
        [getUserInfo],
      ),
      requestName: 'retrieveUserInfo',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
          return Promise.resolve();
        } else {
          toast.error('Failed to retrieve user information for currently logged in user.');
          return Promise.reject(axiosError);
        }
      }, []),
    },
  );

  const {
    execute: retrieveUserLookup,
    loading: retrieveUserLookupLoading,
    response: retrieveUserLookupResponse,
  } = useApiRequestWrapper<
    (
      filter: IUsersFilter & IPaginateParams,
    ) => Promise<AxiosResponse<ApiGen_Base_Page<ApiGen_Concepts_User>, any>>
  >({
    requestFunction: useCallback(
      async (filter: IUsersFilter & IPaginateParams) => await getUserLookup(filter),
      [getUserLookup],
    ),
    requestName: 'retrieveUserLookup',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      toast.error('Failed to retrieve users.');
      return Promise.reject(axiosError);
    }, []),
  });

  return useMemo(
    () => ({
      retrieveUserInfo,
      retrieveUserInfoLoading,
      retrieveUserInfoResponse,
      retrieveUserLookup,
      retrieveUserLookupLoading,
      retrieveUserLookupResponse,
    }),
    [
      retrieveUserInfo,
      retrieveUserInfoLoading,
      retrieveUserInfoResponse,
      retrieveUserLookup,
      retrieveUserLookupLoading,
      retrieveUserLookupResponse,
    ],
  );
};
