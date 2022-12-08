import { AxiosError, AxiosResponse } from 'axios';
import { useApiProperties } from 'hooks/pims-api';
import { IApiError } from 'interfaces/IApiError';
import { Api_PropertyAssociations } from 'models/api/Property';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { useApiRequestWrapper } from './pims-api/useApiRequestWrapper';

export const usePropertyAssociations = () => {
  const { getPropertyAssociations } = useApiProperties();

  const getPropertyAssociationsWrapper = useApiRequestWrapper<
    (id: number) => Promise<AxiosResponse<Api_PropertyAssociations>>
  >({
    requestFunction: useCallback(
      async (id: number) => await getPropertyAssociations(id),
      [getPropertyAssociations],
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
