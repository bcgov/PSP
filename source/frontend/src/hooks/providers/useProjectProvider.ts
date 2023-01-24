import { AxiosError, AxiosResponse } from 'axios';
import { useApiProjects } from 'hooks/pims-api/useApiProjects';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { IApiError } from 'interfaces/IApiError';
import { Api_Product, Api_Project } from 'models/api/Project';
import { useCallback, useMemo } from 'react';
import { toast } from 'react-toastify';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that retrieves Project information.
 */
export const useProjectProvider = () => {
  const { getProjectProducts, postProject } = useApiProjects();

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

  const addProjectApi = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_Project, any>>
  >({
    requestFunction: useCallback(
      async (project: Api_Project) => await postProject(project),
      [postProject],
    ),
    requestName: 'AddProject',
    onSuccess: useAxiosSuccessHandler('Project saved'),
    onError: useAxiosErrorHandler('Failed to save Project'),
  });

  return useMemo(
    () => ({
      retrieveProjectProducts,
      retrieveProjectProductsLoading,
      addProject: addProjectApi,
    }),
    [retrieveProjectProducts, retrieveProjectProductsLoading, addProjectApi],
  );
};
