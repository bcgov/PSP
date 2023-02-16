import { AxiosError, AxiosResponse } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useApiUsers } from 'hooks/pims-api/useApiUsers';
import { IApiError } from 'interfaces/IApiError';
import { Api_User } from 'models/api/User';
import { useCallback, useMemo } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that retrieves user information.
 */
export const useUserInfoRepository = () => {
  const { getUserInfo } = useApiUsers();

  const {
    execute: retrieveUserInfo,
    loading: retrieveUserInfoLoading,
    response: retrieveUserInfoResponse,
  } = useApiRequestWrapper<(userGuid: string) => Promise<AxiosResponse<Api_User, any>>>({
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
  });

  return useMemo(
    () => ({
      retrieveUserInfo,
      retrieveUserInfoLoading,
      retrieveUserInfoResponse,
    }),
    [retrieveUserInfo, retrieveUserInfoLoading, retrieveUserInfoResponse],
  );
};
