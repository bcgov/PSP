import { Api_ResearchFile } from 'models/api/ResearchFile';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the research file endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiResearchFile = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      postResearchFile: (researchFile: Api_ResearchFile) =>
        api.post<Api_ResearchFile>(`/researchFiles`, researchFile),
    }),
    [api],
  );
};
