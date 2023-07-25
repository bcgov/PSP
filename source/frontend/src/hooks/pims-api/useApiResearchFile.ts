import queryString from 'query-string';
import React from 'react';

import { IResearchFilter } from '@/features/research/interfaces';
import { IPagedItems } from '@/interfaces';
import { IResearchSearchResult } from '@/interfaces/IResearchSearchResult';
import { Api_ResearchFile, Api_ResearchFileProperty } from '@/models/api/ResearchFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

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
      postResearchFile: (
        researchFile: Api_ResearchFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.post<Api_ResearchFile>(
          `/researchFiles?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          researchFile,
        ),
      putResearchFile: (researchFile: Api_ResearchFile) =>
        api.put<Api_ResearchFile>(`/researchFiles/${researchFile.id}`, researchFile),
      putResearchFileProperties: (
        researchFile: Api_ResearchFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<Api_ResearchFile>(
          `/researchFiles/${researchFile?.id}/properties?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          researchFile,
        ),
      putPropertyResearchFile: (propertyResearchFile: Api_ResearchFileProperty) =>
        api.put<Api_ResearchFile>(
          `/researchFiles/${propertyResearchFile?.file?.id}/properties/${propertyResearchFile.id}`,
          propertyResearchFile,
        ),
      getResearchFileProperties: (researchFileId: number) =>
        api.get<Api_ResearchFileProperty[]>(`/researchFiles/${researchFileId}/properties`),
    }),
    [api],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;
