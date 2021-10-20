import { TableSort } from 'components/Table/TableSort';
import { ILeaseFilter } from 'features/leases';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useIsMounted from 'hooks/useIsMounted';
import { ILease, IPagedItems } from 'interfaces';
import { useCallback, useState } from 'react';

import { useFetcher } from './useFetcher';

export interface SearchState {
  error?: Error;
  results: ILease[];
  filter: ILeaseFilter;
  totalItems: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  setFilter: (value: ILeaseFilter) => void;
  setSort: (value: TableSort<ILease>) => void;
  setCurrentPage: (value: number) => void;
  setPageSize: (value: number) => void;
  execute: () => void;
}

/**
 * Hook that searches for leases based on supplied filter object.
 * @param initialFilter The filter parameters.
 */
export function useSearch(
  initialFilter: ILeaseFilter,
  initialSort: TableSort<ILease> = {},
  initialPage = 0,
  initialPageSize = 10,
): SearchState {
  // search state
  const [error, setError] = useState<Error>();
  const [filter, setFilter] = useState<ILeaseFilter>(initialFilter);
  const [sort, setSort] = useState<TableSort<ILease>>(initialSort);
  const [currentPage, setCurrentPage] = useState(initialPage);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [results, setResults] = useState<ILease[]>([]);
  const [totalItems, setTotalItems] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  // manual refresh
  const [refreshIndex, setRefreshIndex] = useState(0);
  // api helper
  const searchFn = useFetcher();
  // is this component/hook mounted?
  const isMounted = useIsMounted();

  const setSearchOutput = useCallback((apiResponse?: IPagedItems<ILease>, pageSize = 10) => {
    if (apiResponse?.items) {
      setResults(apiResponse.items);
      setTotalItems(apiResponse.total);
      setTotalPages(Math.ceil(apiResponse.total / pageSize));
    } else {
      setResults([]);
      setTotalItems(0);
      setTotalPages(0);
    }
  }, []);

  // update search results whenever new data comes back from API endpoints
  useDeepCompareEffect(() => {
    if (!filter) return;

    async function callApi() {
      try {
        const { data } = await searchFn(filter, sort, currentPage, pageSize);
        if (isMounted()) {
          setSearchOutput(data);
          setError(undefined);
        }
      } catch (e) {
        setSearchOutput(undefined);
        setError((e as Error) ?? new Error('Something went wrong. Please try again.'));
      }
    }

    callApi();
  }, [isMounted, filter, currentPage, pageSize, searchFn, sort, refreshIndex, setSearchOutput]);

  // allow manual re-triggers of the API calls
  const execute = () => {
    setRefreshIndex(r => r + 1);
  };

  return {
    error,
    results,
    filter,
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
