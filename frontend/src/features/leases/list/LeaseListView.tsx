import { Center } from 'components/common/Center/Center';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import { useSearch } from 'hooks/useSearch';
import { ILeaseSearchResult } from 'interfaces';
import { useCallback } from 'react';
import { FaFileAlt, FaFileExcel } from 'react-icons/fa';
import { toast } from 'react-toastify';

import { ILeaseFilter } from '../interfaces';
import { defaultFilter, LeaseFilter } from './LeaseFilter/LeaseFilter';
import { LeaseSearchResults } from './LeaseSearchResults/LeaseSearchResults';
import * as Styled from './styles';

/**
 * Component that displays a list of leases within PSP as well as a filter bar to control the displayed leases.
 */
export const LeaseListView = () => {
  const { getLeases } = useApiLeases();
  const {
    results,
    filter,
    sort,
    error,
    currentPage,
    totalPages,
    pageSize,
    setFilter,
    setSort,
    setCurrentPage,
    setPageSize,
  } = useSearch<ILeaseSearchResult, ILeaseFilter>(defaultFilter, getLeases);

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: ILeaseFilter) => {
      setFilter(filter);
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  if (error) {
    toast.error(error.message);
  }

  return (
    <Styled.ListPage>
      <Center>
        <LeaseFilter filter={filter} setFilter={changeFilter} />
      </Center>
      <Styled.Scrollable>
        <Styled.PageToolbar>
          <Styled.PageHeader>Leases &amp; Licenses</Styled.PageHeader>
          <Styled.Spacer />
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
            <Styled.FileIcon disabled>
              <FaFileExcel data-testid="excel-icon" size={36} />
            </Styled.FileIcon>
          </TooltipWrapper>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to CSV">
            <Styled.FileIcon disabled>
              <FaFileAlt data-testid="csv-icon" size={36} />
            </Styled.FileIcon>
          </TooltipWrapper>
        </Styled.PageToolbar>

        <LeaseSearchResults
          results={results}
          pageIndex={currentPage}
          pageSize={pageSize}
          pageCount={totalPages}
          sort={sort}
          setSort={setSort}
          setPageSize={setPageSize}
          setPageIndex={setCurrentPage}
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default LeaseListView;
