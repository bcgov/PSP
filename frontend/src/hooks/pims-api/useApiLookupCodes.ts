import React from 'react';
import { ILookupCode } from 'store/slices/lookupCodes';

import { useApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lookup code endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiLookupCodes = () => {
  const api = useApi();

  return React.useMemo(
    () => ({
      getLookupCodes: () => api.get<ILookupCode>(`/lookup/all`),
    }),
    [api],
  );
};
