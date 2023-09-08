import queryString from 'query-string';
import React from 'react';

import { IProjectFilter } from '@/features/projects';
import { IPagedItems } from '@/interfaces';
import { Api_Product, Api_Project } from '@/models/api/Project';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the Project endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProjects = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      postProject: (project: Api_Project) => api.post<Api_Project>(`/projects`, project),
      searchProject: (query: string, top: number = 5) =>
        api.get<Api_Project[]>(`/projects/search=${query}&top=${top}`),
      searchProjects: (params: IPaginateProjects | null) =>
        api.get<IPagedItems<Api_Project>>(
          `/projects/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getAllProjects: () => api.get<Api_Project[]>(`/projects`),
      getProject: (id: number) => api.get<Api_Project>(`/projects/${id}`),
      putProject: (project: Api_Project) =>
        api.put<Api_Project>(`/projects/${project.id}`, project),
      getProjectProducts: (id: number) => api.get<Api_Product[]>(`/projects/${id}/products`),
    }),
    [api],
  );
};

export type IPaginateProjects = IPaginateRequest<IProjectFilter>;
