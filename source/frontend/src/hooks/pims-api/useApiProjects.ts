import queryString from 'query-string';
import React from 'react';

import { IProjectFilter } from '@/features/projects';
import { IPagedItems } from '@/interfaces';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

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
      postProject: (project: ApiGen_Concepts_Project, userOverrideCodes: UserOverrideCode[]) =>
        api.post<ApiGen_Concepts_Project>(
          `/projects?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          project,
        ),
      searchProject: (query: string, top = 5) =>
        api.get<ApiGen_Concepts_Project[]>(`/projects/search=${query}&top=${top}`),
      searchProjects: (params: IPaginateProjects | null) =>
        api.get<IPagedItems<ApiGen_Concepts_Project>>(
          `/projects/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getAllProjects: () => api.get<ApiGen_Concepts_Project[]>(`/projects`),
      getProject: (id: number) => api.get<ApiGen_Concepts_Project>(`/projects/${id}`),
      putProject: (project: ApiGen_Concepts_Project, userOverrideCodes: UserOverrideCode[]) =>
        api.put<ApiGen_Concepts_Project>(
          `/projects/${project.id}
        ?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          project,
        ),
      getProjectProducts: (id: number) =>
        api.get<ApiGen_Concepts_Product[]>(`/projects/${id}/products`),
    }),
    [api],
  );
};

export type IPaginateProjects = IPaginateRequest<IProjectFilter>;
