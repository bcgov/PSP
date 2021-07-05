import { ParcelInfoOrder, TitleSummary } from 'interfaces/ltsaModels';
import React from 'react';

import { useApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the ltsa endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiLtsa = () => {
  const api = useApi();

  return React.useMemo(
    () => ({
      getTitleSummaries: (pid: number) =>
        api.get<TitleSummary[]>(`/tools/ltsa/summaries?pid=${pid}`),
      getParcelInfo: (pid: string) =>
        api.post<ParcelInfoOrder>(`/tools/ltsa/order/parcelInfo?pid=${pid}`),
    }),
    [api],
  );
};
