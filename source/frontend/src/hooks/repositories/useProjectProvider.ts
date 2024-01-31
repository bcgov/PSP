import { AxiosError, AxiosResponse } from 'axios';
import { useCallback, useContext, useMemo } from 'react';
import { toast } from 'react-toastify';

import { ProjectStateContext } from '@/features/projects/context/ProjectContext';
import { useApiProjects } from '@/hooks/pims-api/useApiProjects';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that retrieves Project information.
 */
export const useProjectProvider = () => {
  const { getProjectProducts, postProject, getProject, getAllProjects, putProject } =
    useApiProjects();
  const { project, setProject } = useContext(ProjectStateContext);

  const { execute: retrieveProjectProducts, loading: retrieveProjectProductsLoading } =
    useApiRequestWrapper<
      (projectId: number) => Promise<AxiosResponse<ApiGen_Concepts_Product[], any>>
    >({
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
    (
      project: ApiGen_Concepts_Project,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_Project, any>>
  >({
    requestFunction: useCallback(
      async (project: ApiGen_Concepts_Project, userOverrideCodes: UserOverrideCode[]) =>
        await postProject(project, userOverrideCodes),
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
    throwError: true,
  });

  const getProjectApi = useApiRequestWrapper<
    (projectId: number) => Promise<AxiosResponse<ApiGen_Concepts_Project, any>>
  >({
    requestFunction: useCallback(
      async (projectId: number) => await getProject(projectId),
      [getProject],
    ),
    requestName: 'RetrieveProject',
    onError: useAxiosErrorHandler('Failed to load Project'),
  });

  const getAllProjectsApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_Project[], any>>
  >({
    requestFunction: useCallback(async () => await getAllProjects(), [getAllProjects]),
    requestName: 'RetrieveAllProjects',
    onError: useAxiosErrorHandler('Failed to load Projects'),
  });

  const updateProject = useApiRequestWrapper<
    (
      project: ApiGen_Concepts_Project,
      userOverrideCodes: UserOverrideCode[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_Project, any>>
  >({
    requestFunction: useCallback(
      async (project: ApiGen_Concepts_Project, userOverrideCodes: UserOverrideCode[]) =>
        await putProject(project, userOverrideCodes),
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
