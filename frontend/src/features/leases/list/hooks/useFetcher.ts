import { TableSort } from 'components/Table/TableSort';
import { ILeaseFilter } from 'features/leases';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { ILeaseSearchResult } from 'interfaces';
import isEmpty from 'lodash/isEmpty';
import { useCallback } from 'react';
import { generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';

export function useFetcher() {
  const { getLeases } = useApiLeases();

  const fetchFn = async (
    filter: ILeaseFilter,
    sort: TableSort<ILeaseSearchResult>,
    pageIndex: number,
    pageSize: number,
  ) => {
    // Call API with appropriate search parameters
    const requestParams = getRequestParams(pageIndex, pageSize, filter, sort);
    const response = await getLeases(requestParams);
    return response;
  };

  return useCallback(fetchFn, [getLeases]);
}

function getRequestParams(
  pageIndex: number,
  pageSize: number,
  filter: ILeaseFilter,
  sort: TableSort<ILeaseSearchResult>,
) {
  const sortParam = sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined;
  return toFilteredApiPaginateParams<ILeaseFilter>(pageIndex, pageSize, sortParam, filter);
}
