import React from 'react';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper for bctfa ownership api calls
 * @returns Object containing functions to make requests to the PIMS API.
 */

export const useApiBctfaOwnership = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      updateBctfaOwnership: (file: File) => {
        const formData = new FormData();
        formData.append('ownershipFile', file);
        return api.put<number[]>(`/tools/bctfa/ownership`, formData);
      },
    }),
    [api],
  );
};
