import React from 'react';

import { IAutocompleteResponse } from '@/interfaces';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the autocomplete endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiAutocomplete = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getOrganizationPredictions: (input: string, top = 5) =>
        api.get<IAutocompleteResponse>(`autocomplete/organizations?search=${input}&top=${top}`),
    }),
    [api],
  );
};
