import React from 'react';

import { ISystemConstant } from '@/store/slices/systemConstants';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to retrieve system constants.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiSystemConstants = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getSystemConstants: () => api.get<ISystemConstant[]>(`/systemConstant`),
    }),
    [api],
  );
};
