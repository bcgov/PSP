import { AxiosError, AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';
import { toast } from 'react-toastify';

import { useApiProducts } from '@/hooks/pims-api/useApiProducts';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';

/**
 * hook that retrieves Product information.
 */
export const useProductProvider = () => {
  const { getProductFiles, getProductAtTime } = useApiProducts();

  const { execute: retrieveProductFiles, loading: retrieveProductFilesLoading } =
    useApiRequestWrapper<
      (projectId: number) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFile[] | null, any>>
    >({
      requestFunction: useCallback(
        async (productId: number) => await getProductFiles(productId),
        [getProductFiles],
      ),
      requestName: 'retrieveProductFiles',
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
          return Promise.resolve();
        } else {
          toast.error('Retrieve files for product error. Check responses and try again.');
          return Promise.reject(axiosError);
        }
      }, []),
    });

  const getProductAtTimeApi = useApiRequestWrapper<
    (product: number, time: string) => Promise<AxiosResponse<ApiGen_Concepts_Product | null, any>>
  >({
    requestFunction: useCallback(
      async (productId: number, time: string) => await getProductAtTime(productId, time),
      [getProductAtTime],
    ),
    requestName: 'getProductAtTime',
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
        return Promise.resolve();
      } else {
        toast.error('Unable to retrieve historical product');
        return Promise.reject(axiosError);
      }
    }, []),
  });

  return useMemo(
    () => ({
      retrieveProductFiles,
      retrieveProductFilesLoading,
      getProductAtTime: getProductAtTimeApi,
    }),
    [getProductAtTimeApi, retrieveProductFiles, retrieveProductFilesLoading],
  );
};
