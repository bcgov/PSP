import React from 'react';
import { useApi, IApiVersion, IHealthLive, IHealthReady } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the health endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiHealth = () => {
  const api = useApi();

  return React.useMemo(
    () => ({
      getVersion: () => api.get<IApiVersion>('health/env'),
      getLive: () => api.get<IHealthLive>('health/live'),
      getReady: () => api.get<IHealthReady>('health/ready'),
    }),
    [api],
  );
};
