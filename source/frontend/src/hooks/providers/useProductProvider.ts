import { AxiosError, AxiosResponse } from 'axios';
import { useApiProducts } from 'hooks/pims-api/useApiProducts';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { useCallback, useMemo } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that retrieves Product information.
 */
export const useProductProvider = () => {
  const { getProductFile } = useApiProducts();

  const { execute: retrieveProductFile, loading: retrieveProductFileLoading } =
    useApiRequestWrapper<
      (projectId: number) => Promise<AxiosResponse<Api_AcquisitionFile | null, any>>
    >({
      requestFunction: useCallback(
        async (productId: number) => await getProductFile(productId),
        [getProductFile],
      ),
      requestName: 'retrieveProductFile',
      onSuccess: useCallback(() => toast.success('File for project retrieved'), []),
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Retrieve file for product error. Check responses and try again.');
        }
      }, []),
    });

  return useMemo(
    () => ({
      retrieveProductFile,
      retrieveProductFileLoading,
    }),
    [retrieveProductFile, retrieveProductFileLoading],
  );
};
