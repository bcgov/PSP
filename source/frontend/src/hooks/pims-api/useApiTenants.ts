import React from 'react';

import IApiTenants from './interfaces/IApiTenants';
import { ITenantConfig } from './interfaces/ITenantConfig';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the tenants endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiTenants = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () =>
      ({
        getSettings: () => api.get<ITenantConfig>('tenants'),
      } as IApiTenants),
    [api],
  );
};
