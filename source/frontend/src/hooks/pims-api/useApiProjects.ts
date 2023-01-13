import { IProjectFilter } from 'features/projects';
import { IPagedItems, IProjectSearchResult } from 'interfaces';
import { Api_Project } from 'models/api/Project';
import queryString from 'query-string';
import React from 'react';

import { IPaginateRequest, useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProjects = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      searchProjects: (params: IPaginateProjects | null) =>
        api.get<IPagedItems<IProjectSearchResult>>(
          `/projects/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getProject: (id: number) => api.get<Api_Project>(`/projects/${id}`),
    }),
    [api],
  );
};

export type IPaginateProjects = IPaginateRequest<IProjectFilter>;
