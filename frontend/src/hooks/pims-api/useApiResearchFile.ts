import { IResearchFilter } from 'features/research/interfaces';
import { IPagedItems } from 'interfaces';
import { IResearchSearchResult } from 'interfaces/IResearchSearchResult';
import { Api_ResearchFile } from 'models/api/ResearchFile';
import queryString from 'query-string';
import React from 'react';

import { IPaginateRequest, useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the research file endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiResearchFile = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getResearchFiles: (params: IPaginateResearch | null) =>
        api.get<IPagedItems<IResearchSearchResult>>(
          `/researchFiles/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getResearchFile: (researchFileId: number) =>
        api.get<Api_ResearchFile>(`/researchFiles/${researchFileId}`),
      postResearchFile: (researchFile: Api_ResearchFile) =>
        api.post<Api_ResearchFile>(`/researchFiles`, researchFile),
      putResearchFile: (researchFile: Api_ResearchFile) =>
        api.put<Api_ResearchFile>(`/researchFiles/${researchFile.id}`, researchFile),
    }),
    [api],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;
