import React from 'react';

import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the Product endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProducts = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getProductFiles: (id: number) =>
        api.get<ApiGen_Concepts_AcquisitionFile[] | null>(`/products/${id}/acquisitionfiles`),
      getProductAtTime: (productId: number, time: string) =>
        api.get<ApiGen_Concepts_Product>(`/products/${productId}/historical?time=${time}`),
    }),
    [api],
  );
};
