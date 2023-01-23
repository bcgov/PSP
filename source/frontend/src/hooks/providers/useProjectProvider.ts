import { AxiosError, AxiosResponse } from 'axios';
import { useApiProjects } from 'hooks/pims-api/useApiProjects';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import { Api_Product } from 'models/api/Project';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

/**
 * hook that retrieves document information.
 */
export const useProjectProvider = () => {
  const { getProjectProducts } = useApiProjects();

  // Provides functionality to retrieve document metadata information
  const { execute: retrieveProjectProducts, loading: retrieveProjectProductsLoading } =
    useApiRequestWrapper<(projectId: number) => Promise<AxiosResponse<Api_Product[], any>>>({
      requestFunction: useCallback(
        async (projectId: number) => await getProjectProducts(projectId),
        [getProjectProducts],
      ),
      requestName: 'retrieveProjectProducts',
      onSuccess: useCallback(() => toast.success('Products for project retrieved'), []),
      onError: useCallback((axiosError: AxiosError<IApiError>) => {
        if (axiosError?.response?.status === 400) {
          toast.error(axiosError?.response.data.error);
        } else {
          toast.error('Retrieve products for project error. Check responses and try again.');
        }
      }, []),
    });

  return {
    retrieveProjectProducts,
    retrieveProjectProductsLoading,
  };
};
