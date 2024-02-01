import queryString from 'query-string';
import React from 'react';

import { IResearchFilter } from '@/features/research/interfaces';
import { IPagedItems } from '@/interfaces';
import { IResearchSearchResult } from '@/interfaces/IResearchSearchResult';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
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
        api.get<ApiGen_Concepts_ResearchFile>(`/researchFiles/${researchFileId}`),
      postResearchFile: (
        researchFile: ApiGen_Concepts_ResearchFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.post<ApiGen_Concepts_ResearchFile>(
          `/researchFiles?${userOverrideCodes.map(o => `userOverrideCodes=${o}`).join('&')}`,
          researchFile,
        ),
      putResearchFile: (researchFile: ApiGen_Concepts_ResearchFile) =>
        api.put<ApiGen_Concepts_ResearchFile>(`/researchFiles/${researchFile.id}`, researchFile),
      getLastUpdatedByApi: (researchFileId: number) =>
        api.get<Api_LastUpdatedBy>(`/researchFiles/${researchFileId}/updateInfo`),
      putResearchFileProperties: (
        researchFile: ApiGen_Concepts_ResearchFile,
        userOverrideCodes: UserOverrideCode[] = [],
      ) =>
        api.put<ApiGen_Concepts_ResearchFile>(
          `/researchFiles/${researchFile?.id}/properties?${userOverrideCodes
            .map(o => `userOverrideCodes=${o}`)
            .join('&')}`,
          researchFile,
        ),
      putPropertyResearchFile: (propertyResearchFile: ApiGen_Concepts_ResearchFileProperty) =>
        api.put<ApiGen_Concepts_ResearchFile>(
          `/researchFiles/${propertyResearchFile?.fileId}/properties/${propertyResearchFile.id}`,
          propertyResearchFile,
        ),
      getResearchFileProperties: (researchFileId: number) =>
        api.get<ApiGen_Concepts_ResearchFileProperty[]>(
          `/researchFiles/${researchFileId}/properties`,
        ),
    }),
    [api],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;
