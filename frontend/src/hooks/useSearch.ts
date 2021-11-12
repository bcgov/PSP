import { AxiosResponse } from 'axios';
import { TableSort } from 'components/Table/TableSort';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useIsMounted from 'hooks/useIsMounted';
import { IPagedItems } from 'interfaces';
import { useCallback, useState } from 'react';
import { toast } from 'react-toastify';

import { IPaginateRequest } from './pims-api';
import { useFetcher } from './useFetcher';

export interface SearchState<IFilter, ISearchResult extends object> {
  error?: Error;
  results: ISearchResult[];
  filter: IFilter;
  sort: TableSort<ISearchResult>;
  totalItems: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  setFilter: (value: IFilter) => void;
  setSort: (value: TableSort<ISearchResult>) => void;
  setCurrentPage: (value: number) => void;
  setPageSize: (value: number) => void;
  execute: () => void;
  loading?: boolean;
}

/**
 * Hook that searches for leases based on supplied filter object.
 * @param initialFilter The filter parameters.
 * @param apiCall a function wrapping an api endpoint call that will handle this filter and return paginated results.
 * @param initialSort Any sort parameters to apply to the filter.
 * @param initialPage The initial paginated page for this filter.
 * @param initialPageSize The initial page size for this filter.
 */
export function useSearch<ISearchResult extends object, IFilter extends object>(
  initialFilter: IFilter,
  apiCall: (
    params: IPaginateRequest<IFilter>,
  ) => Promise<AxiosResponse<IPagedItems<ISearchResult>>>,
  noResultsWarning?: string,
  initialSort: TableSort<ISearchResult> = {},
  initialPage = 0,
  initialPageSize = 10,
): SearchState<IFilter, ISearchResult> {
  // search state
  const [error, setError] = useState<Error>();
  const [filter, setFilter] = useState<IFilter>(initialFilter);
  const [sort, setSort] = useState<TableSort<ISearchResult>>(initialSort);
  const [currentPage, setCurrentPage] = useState(initialPage);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [results, setResults] = useState<ISearchResult[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [loading, setLoading] = useState(false);
  // manual refresh
  const [refreshIndex, setRefreshIndex] = useState(0);
  const searchFn = useFetcher<ISearchResult, IFilter>(apiCall);
  // is this component/hook mounted?
  const isMounted = useIsMounted();

  const setSearchOutput = useCallback(
    (apiResponse?: IPagedItems<ISearchResult>, pageSize = 10) => {
      if (apiResponse?.items) {
        setResults(apiResponse.items);
        setTotalItems(apiResponse.total);
        setTotalPages(Math.ceil(apiResponse.total / pageSize));
        if (noResultsWarning && apiResponse.items.length === 0) {
          toast.warn(noResultsWarning);
        }
      } else {
        setResults([]);
        setTotalItems(0);
        setTotalPages(0);
        if (noResultsWarning) {
          toast.warn(noResultsWarning);
        }
      }
    },
    [noResultsWarning],
  );

  // update search results whenever new data comes back from API endpoints
  useDeepCompareEffect(() => {
    if (!filter) return;

    async function callApi() {
      try {
        setLoading(true);
        const { data } = await searchFn(filter, sort, currentPage, pageSize);
        if (isMounted()) {
          setSearchOutput(data, pageSize);
          setError(undefined);
        }
      } catch (e) {
        if (isMounted()) {
          setSearchOutput(undefined, 0);
          setError((e as Error) ?? new Error('Something went wrong. Please try again.'));
        }
      } finally {
        setLoading(false);
      }
    }

    callApi();
  }, [isMounted, filter, currentPage, pageSize, searchFn, sort, refreshIndex, setSearchOutput]);

  // allow manual re-triggers of the API calls
  const execute = () => {
    setRefreshIndex(r => r + 1);
  };

  return {
    loading,
    error,
    results,
    filter,
    sort,
    totalItems,
    totalPages,
    currentPage,
    pageSize,
    setFilter,
    setSort,
    setCurrentPage,
    setPageSize,
    execute: useCallback(execute, []),
  };
}
