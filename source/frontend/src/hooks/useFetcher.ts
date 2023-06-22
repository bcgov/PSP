import { AxiosResponse } from 'axios';
import isEmpty from 'lodash/isEmpty';
import { useCallback } from 'react';

import { TableSort } from '@/components/Table/TableSort';
import { IPagedItems } from '@/interfaces';
import { generateMultiSortCriteria } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';

import { IPaginateRequest } from './pims-api/interfaces/IPaginateRequest';

export function useFetcher<ISearchResult extends object, IFilter extends object>(
  fetch: (params: IPaginateRequest<IFilter>) => Promise<AxiosResponse<IPagedItems<ISearchResult>>>,
) {
  const fetchFn = async (
    filter: IFilter,
    sort: TableSort<ISearchResult>,
    pageIndex: number,
    pageSize: number,
  ) => {
    // Call API with appropriate search parameters
    const requestParams = getRequestParams<ISearchResult, IFilter>(
      pageIndex,
      pageSize,
      filter,
      sort,
    );
    const response = await fetch(requestParams);
    return response;
  };

  return useCallback(fetchFn, [fetch]);
}

function getRequestParams<ISearchResult extends object, IFilter extends object>(
  pageIndex: number,
  pageSize: number,
  filter: IFilter,
  sort: TableSort<ISearchResult>,
) {
  const sortParam = sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined;
  return toFilteredApiPaginateParams<IFilter>(pageIndex, pageSize, sortParam, filter);
}
