import { AxiosResponse } from 'axios';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { useCallback } from 'react';
import { toast } from 'react-toastify';
import { pidFormatter } from 'utils';

import { useApiLtsa } from './pims-api/useApiLtsa';
import { useApiRequestWrapper } from './pims-api/useApiRequestWrapper';

/**
 * hook retrieves data from ltsa
 */
export const useLtsa = () => {
  const { getLtsaOrders } = useApiLtsa();

  const { execute, loading } = useApiRequestWrapper<
    (pid: string) => Promise<AxiosResponse<LtsaOrders>>
  >({
    requestFunction: useCallback(async (pid: string) => await getLtsaOrders(pidFormatter(pid)), [
      getLtsaOrders,
    ]),
    requestName: 'getLtsaData',
    onError: useCallback(axiosError => {
      toast.error(`Failed to get LTSA data. error from LTSA: ${axiosError?.response?.data.error}`);
    }, []),
  });

  return { getLtsaData: execute, ltsaLoading: loading };
};
