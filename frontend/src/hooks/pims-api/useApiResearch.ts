import { IPagedItems } from 'interfaces';
import { IResearchSearchResult } from 'interfaces/IResearchSearchResult';
import queryString from 'query-string';
import React from 'react';

import { IResearchFilter } from './../../features/research/interfaces';
import { IPaginateRequest, useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the lease endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiResearch = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getResearchFiles: (params: IPaginateResearch | null) =>
        api.get<IPagedItems<IResearchSearchResult>>(
          `/research/search?${params ? queryString.stringify(params) : ''}`,
        ),
    }),
    [api],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;
