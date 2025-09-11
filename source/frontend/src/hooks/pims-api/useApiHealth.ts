import React from 'react';

import { IApiHealth } from './interfaces/IApiHealth';
import IApiVersion from './interfaces/IApiVersion';
import IHealthLive from './interfaces/IHealthLive';
import IHealthReady from './interfaces/IHealthReady';
import ISystemCheck from './interfaces/ISystemCheck';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the health endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiHealth = () => {
  const api = useAxiosApi();

  return React.useMemo<IApiHealth>(
    () => ({
      getVersion: () => api.get<IApiVersion>('health/env'),
      getLive: () => api.get<IHealthLive>('health/live'),
      getReady: () => api.get<IHealthReady>('health/ready'),
      getSystemCheck: () => api.get<ISystemCheck>('health/system'),
    }),
    [api],
  );
};
