import { AxiosError, AxiosResponse } from 'axios';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { IApiError } from '@/interfaces/IApiError';
import { Api_PropertyAssociations } from '@/models/api/Property';

import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

export const usePropertyAssociations = () => {
  const { getPropertyAssociationsApi } = useApiProperties();

  const getPropertyAssociationsWrapper = useApiRequestWrapper<
    (id: number) => Promise<AxiosResponse<Api_PropertyAssociations>>
  >({
    requestFunction: useCallback(
      async (id: number) => await getPropertyAssociationsApi(id),
      [getPropertyAssociationsApi],
    ),
    requestName: 'getPropertyAssociations',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      toast.error(
        `Failed to get PIMS property data. error from backend: ${axiosError?.response?.data?.error}`,
      );
    }, []),
  });

  return getPropertyAssociationsWrapper;
};
