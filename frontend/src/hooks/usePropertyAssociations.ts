import { AxiosResponse } from 'axios';
import { useApiProperties } from 'hooks/pims-api';
import { Api_PropertyAssociations } from 'models/api/Property';
import { useCallback } from 'react';
import { toast } from 'react-toastify';
import { pidFormatter } from 'utils';

import { useApiRequestWrapper } from './pims-api/useApiRequestWrapper';

export const usePropertyAssociations = () => {
  const { getPropertyAssociations } = useApiProperties();

  const { execute, loading } = useApiRequestWrapper<
    (pid: string) => Promise<AxiosResponse<Api_PropertyAssociations>>
  >({
    requestFunction: useCallback(
      async (pid: string) => await getPropertyAssociations(pidFormatter(pid)),
      [getPropertyAssociations],
    ),
    requestName: 'getPropertyAssociations',
    onError: useCallback(axiosError => {
      toast.error(
        `Failed to get PIMS property data. error from backend: ${axiosError?.response?.data.error}`,
      );
    }, []),
  });

  return { getPropertyAssociations: execute, isLoading: loading };
};
