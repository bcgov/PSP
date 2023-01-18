import { AxiosResponse } from 'axios';
import { useApiProjects } from 'hooks/pims-api/useApiProjects';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_Project } from 'models/api/Project';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

export const useProjectProvider = () => {
  const { postProject } = useApiProjects();

  const addProjectApi = useApiRequestWrapper<
    (...args: any[]) => Promise<AxiosResponse<Api_Project, any>>
  >({
    requestFunction: useCallback(async (pro: Api_Project) => await postProject(pro), [postProject]),
    requestName: 'AddProject',
    onSuccess: useAxiosSuccessHandler('Project saved'),
    onError: useAxiosErrorHandler('Failed to save Project'),
  });

  return useMemo(
    () => ({
      addProject: addProjectApi,
    }),
    [addProjectApi],
  );
};
