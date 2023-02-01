import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the Product endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProducts = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getProductFiles: (id: number) =>
        api.get<Api_AcquisitionFile[] | null>(`/products/${id}/acquisitionfiles`),
    }),
    [api],
  );
};
