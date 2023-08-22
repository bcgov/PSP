import { AxiosError, AxiosResponse } from 'axios';
import { useCallback, useContext, useMemo } from 'react';
import { toast } from 'react-toastify';

import { ProjectStateContext } from '@/features/projects/context/ProjectContext';
import { useApiProjects } from '@/hooks/pims-api/useApiProjects';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { Api_Product, Api_Project } from '@/models/api/Project';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that retrieves Project information.
 */
export const useProjectProvider = () => {
  const { getProjectProducts, postProject, getProject, getAllProjects, putProject } =
    useApiProjects();
  const { project, setProject } = useContext(ProjectStateContext);

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
    (project: Api_Project) => Promise<AxiosResponse<Api_Project, any>>
  >({
    requestFunction: useCallback(
      async (project: Api_Project) => await postProject(project),
      [postProject],
    ),
    requestName: 'AddProject',
    onSuccess: useAxiosSuccessHandler('Project saved'),
    onError: useCallback((axiosError: AxiosError<IApiError>) => {
      if (axiosError?.response?.status === 409) {
        toast.error(axiosError?.response.data as any);
      } else {
        toast.error('Failed to save project.');
      }
    }, []),
  });

  const getProjectApi = useApiRequestWrapper<
    (projectId: number) => Promise<AxiosResponse<Api_Project, any>>
  >({
    requestFunction: useCallback(
      async (projectId: number) => await getProject(projectId),
      [getProject],
    ),
    requestName: 'RetrieveProject',
    onError: useAxiosErrorHandler('Failed to load Project'),
  });

  const getAllProjectsApi = useApiRequestWrapper<() => Promise<AxiosResponse<Api_Project[], any>>>({
    requestFunction: useCallback(async () => await getAllProjects(), [getAllProjects]),
    requestName: 'RetrieveAllProjects',
    onError: useAxiosErrorHandler('Failed to load Projects'),
  });

  const updateProject = useApiRequestWrapper<
    (project: Api_Project) => Promise<AxiosResponse<Api_Project, any>>
  >({
    requestFunction: useCallback(
      async (project: Api_Project) => await putProject(project),
      [putProject],
    ),
    requestName: 'UpdateProject',
    throwError: true,
  });

  return useMemo(
    () => ({
      project,
      setProject,
      retrieveProjectProducts,
      retrieveProjectProductsLoading,
      addProject: addProjectApi,
      getProject: getProjectApi,
      updateProject: updateProject,
      getAllProjects: getAllProjectsApi,
    }),
    [
      project,
      setProject,
      retrieveProjectProducts,
      retrieveProjectProductsLoading,
      addProjectApi,
      getProjectApi,
      updateProject,
      getAllProjectsApi,
    ],
  );
};
