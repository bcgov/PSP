import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';

import { useApiBctfaOwnership } from '../pims-api/useApiBctfaOwnership';

/**
 * repository for bcfta ownership
 */
export const useBctfaOwnershipRepository = () => {
  const { updateBctfaOwnership } = useApiBctfaOwnership();

  // Provides functionality to download a template of a specific type using uploaded json
  const updateBctfaOwnershipApi = useApiRequestWrapper<
    (file: File) => Promise<AxiosResponse<void, any>>
  >({
    requestFunction: useCallback(
      async (file: File) => await updateBctfaOwnership(file),
      [updateBctfaOwnership],
    ),
    requestName: 'UpdateBctfaOwnershipApi',
    throwError: true,
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
        return Promise.resolve();
      } else {
        return Promise.reject(axiosError);
      }
    }, []),
  });

  return {
    updateBctfaOwnershipApi,
  };
};
