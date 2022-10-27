import React from 'react';

import { IApiHealth, IApiVersion, IHealthLive, IHealthReady, useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the health endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiHealth = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () =>
      ({
        getVersion: () => api.get<IApiVersion>('health/env'),
        getLive: () => api.get<IHealthLive>('health/live'),
        getReady: () => api.get<IHealthReady>('health/ready'),
      } as IApiHealth),
    [api],
  );
};
