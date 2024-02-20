import { AxiosResponse } from 'axios';
import { useCallback, useMemo, useReducer, useState } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { toast } from 'react-toastify';

import { SortDirection, TableSort } from '@/components/Table/TableSort';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useIsMounted from '@/hooks/util/useIsMounted';
import { IPagedItems } from '@/interfaces';

import { IPaginateRequest } from './pims-api/interfaces/IPaginateRequest';
import { useFetcher } from './useFetcher';

export interface SearchProps<IFilter, ISearchResult extends object>
  extends SearchState<IFilter, ISearchResult> {
  setFilter: (value: IFilter) => void;
  setSort: (value: TableSort<ISearchResult>) => void;
  setCurrentPage: (value: number) => void;
  setPageSize: (value: number) => void;
  execute: () => void;
}

export interface SearchState<IFilter, ISearchResult extends object> {
  error?: Error;
  results: ISearchResult[];
  filter: IFilter;
  sort: TableSort<ISearchResult>;
  totalItems: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  loading?: boolean;
}

/**
 * Hook that searches for API entities based on supplied filter object.
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
): SearchProps<IFilter, ISearchResult> {
  // search state
  const initialState = {
    sort: initialSort,
    currentPage: initialPage,
    pageSize: initialPageSize,
    results: [],
    filter: initialFilter,
    totalItems: 0,
    totalPages: 0,
  };
  const [state, setState] = useReducer(
    (
      prevState: SearchState<IFilter, ISearchResult>,
      newState: Partial<SearchState<IFilter, ISearchResult>>,
    ) => ({
      ...prevState,
      ...newState,
    }),
    initialState,
  );
  const [refreshIndex, setRefreshIndex] = useState(0);
  const searchFn = useFetcher<ISearchResult, IFilter>(apiCall);
  // is this component/hook mounted?
  const isMounted = useIsMounted();
  const dispatch = useDispatch();

  const setSearchOutput = useCallback(
    (apiResponse?: IPagedItems<ISearchResult>, pageSize = 10) => {
      if (apiResponse?.items) {
        setState({
          results: apiResponse.items,
          totalItems: apiResponse.total,
          totalPages: Math.ceil(apiResponse.total / pageSize),
        });
        if (noResultsWarning && apiResponse.items.length === 0) {
          toast.warn(noResultsWarning);
        }
      } else {
        setState({
          results: [],
          totalItems: 0,
          totalPages: 0,
        });
        if (noResultsWarning) {
          toast.warn(noResultsWarning);
        }
      }
    },
    [noResultsWarning],
  );

  const pageSize = state.pageSize;
  const sort = state.sort;
  const currentPage = state.currentPage;
  const filter = state.filter;
  const callApi = useCallback(
    async (currentPage: number, filter: IFilter) => {
      try {
        setState({ loading: true });
        dispatch(showLoading());
        const { data } = await searchFn(filter, sort, currentPage, pageSize);
        if (isMounted()) {
          setSearchOutput(data, pageSize);
          setState({ error: undefined });
        }
      } catch (e) {
        if (isMounted()) {
          setSearchOutput(undefined, 0);
          setState({ error: (e as Error) ?? new Error('Something went wrong. Please try again.') });
        }
      } finally {
        setState({ loading: false });
        dispatch(hideLoading());
      }
    },
    [dispatch, isMounted, pageSize, searchFn, setSearchOutput, sort],
  );

  // update search results whenever new data comes back from API endpoints
  useDeepCompareEffect(() => {
    if (!filter) {
      return;
    }
    callApi(currentPage, filter);
  }, [currentPage, filter, callApi, refreshIndex]);

  // allow manual re-triggers of the API calls
  const executeFn = useCallback(() => {
    setRefreshIndex(r => r + 1);
  }, []);

  const setFilterFn = useCallback((filter: IFilter) => {
    setState({ filter, currentPage: 0 });
  }, []);
  const setSortFn = useCallback((sort: TableSort<ISearchResult>) => setState({ sort }), []);
  const setCurrentPageFn = useCallback((currentPage: number) => setState({ currentPage }), []);
  const setPageSizeFn = useCallback((pageSize: number) => setState({ pageSize }), []);

  return useMemo(
    () => ({
      ...state,
      setFilter: setFilterFn,
      setSort: setSortFn,
      setCurrentPage: setCurrentPageFn,
      setPageSize: setPageSizeFn,
      execute: executeFn,
      refreshIndex,
    }),
    [state, setFilterFn, setSortFn, setCurrentPageFn, setPageSizeFn, executeFn, refreshIndex],
  );
}

// results sort handler
export const handleSortChange = <Result extends object>(
  column: string,
  nextSortDirection: SortDirection,
  sort: TableSort<Result>,
  setSort?: (result: TableSort<Result>) => void,
) => {
  if (!setSort) return null;

  let nextSort: TableSort<Result>;

  // add new column to sort criteria
  if (nextSortDirection) {
    nextSort = { [column]: nextSortDirection } as any;
  } else {
    // remove column from sort criteria
    nextSort = {};
  }
  setSort(nextSort);
};
