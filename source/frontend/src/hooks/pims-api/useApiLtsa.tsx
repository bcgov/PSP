import React from 'react';

import { LtsaOrders, ParcelInfoOrder, TitleSummary } from '@/interfaces/ltsaModels';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the ltsa endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLtsa = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getTitleSummaries: (pid: number) =>
        api.get<TitleSummary[]>(`/tools/ltsa/summaries?pid=${pid}`),
      getParcelInfo: (pid: string) =>
        api.post<ParcelInfoOrder>(`/tools/ltsa/order/parcelInfo?pid=${pid}`),
      getLtsaOrders: (pid: string) => api.post<LtsaOrders>(`/tools/ltsa/all?pid=${pid}`),
    }),
    [api],
  );
};
