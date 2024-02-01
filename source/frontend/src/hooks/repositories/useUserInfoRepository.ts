import { AxiosError, AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';
import { toast } from 'react-toastify';

import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';

/**
 * hook that retrieves user information.
 */
export const useUserInfoRepository = () => {
  const { getUserInfo } = useApiUsers();

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
        } else {
          toast.error('Failed to retrieve user information for currently logged in user.');
        }
      }, []),
    },
  );

  return useMemo(
    () => ({
      retrieveUserInfo,
      retrieveUserInfoLoading,
      retrieveUserInfoResponse,
    }),
    [retrieveUserInfo, retrieveUserInfoLoading, retrieveUserInfoResponse],
  );
};
