import { IProjectFilter } from 'features/projects';
import { IPagedItems, IProjectSearchResult } from 'interfaces';
import { Api_Project } from 'models/api/Project';
import queryString from 'query-string';
import React from 'react';

import { IPaginateRequest, useAxiosApi } from '.';

let projectsTest: IProjectSearchResult[] = [
  {
    id: 1,
    projectName: 'Test 1',
    projectNumber: '11111',
    lastUpdatedBy: 'someone',
    lastUpdatedDate: new Date(),
  },
  {
    id: 2,
    projectName: 'Test 2',
    projectNumber: '2222',
    lastUpdatedBy: 'someone',
    lastUpdatedDate: new Date(),
  },
];

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProjects = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getProjects: (params: IPaginateProjects | null) =>
        api.get<IPagedItems<IProjectSearchResult>>(
          `/projects/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getProject: (id: number) => api.get<Api_Project>(`/leases/${id}`),
      getProjectsDummy: (params: IPaginateProjects | null) => projectsTest,
    }),
    [api],
  );
};

export type IPaginateProjects = IPaginateRequest<IProjectFilter>;
