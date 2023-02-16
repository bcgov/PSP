import { AxiosResponse } from 'axios';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { useCallback } from 'react';
import { pidFormatter } from 'utils';

import { useApiLtsa } from './pims-api/useApiLtsa';
import { useApiRequestWrapper } from './pims-api/useApiRequestWrapper';

/**
 * hook retrieves data from ltsa
 */
export const useLtsa = () => {
  const { getLtsaOrders } = useApiLtsa();

  const ltsaRequestWrapper = useApiRequestWrapper<
    (pid: string) => Promise<AxiosResponse<LtsaOrders>>
  >({
    requestFunction: useCallback(
      async (pid: string) => await getLtsaOrders(pidFormatter(pid)),
      [getLtsaOrders],
    ),
    requestName: 'getLtsaData',
  });

  return ltsaRequestWrapper;
};
