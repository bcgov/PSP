import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { LtsaOrders, SpcpOrder } from '@/interfaces/ltsaModels';
import { pidFormatter } from '@/utils';

import { useApiLtsa } from './pims-api/useApiLtsa';
import { useApiRequestWrapper } from './util/useApiRequestWrapper';

/**
 * hook retrieves data from ltsa
 */
export const useLtsa = () => {
  const { getLtsaOrders, getSPCPInfo } = useApiLtsa();

  const ltsaRequestWrapperApi = useApiRequestWrapper<
    (pid: string) => Promise<AxiosResponse<LtsaOrders>>
  >({
    requestFunction: useCallback(
      async (pid: string) => await getLtsaOrders(pidFormatter(pid)),
      [getLtsaOrders],
    ),
    requestName: 'getLtsaData',
  });

  // Strata Plan Common Property
  const ltsaStrataPlanCommonPropertyRequestApi = useApiRequestWrapper<
    (planNumber: string) => Promise<AxiosResponse<SpcpOrder>>
  >({
    requestFunction: useCallback(
      async (strataPlanNumber: string) => await getSPCPInfo(strataPlanNumber),
      [getSPCPInfo],
    ),
    requestName: 'getLtsaData',
  });

  return useMemo(
    () => ({
      ltsaRequestWrapper: ltsaRequestWrapperApi,
      getStrataPlanCommonProperty: ltsaStrataPlanCommonPropertyRequestApi,
    }),
    [ltsaRequestWrapperApi, ltsaStrataPlanCommonPropertyRequestApi],
  );
};
