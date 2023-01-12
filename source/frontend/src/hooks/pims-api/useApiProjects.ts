import { Api_Project } from 'models/api/Project';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the project endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProjects = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getProjectPredictions: (query: string, top: number = 5) =>
        api.get<Api_Project[]>(`/projects/search=${query}&top=${top}`),
    }),
    [api],
  );
};
