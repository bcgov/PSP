import { Center } from 'components/common/Center/Center';
import { Scrollable } from 'components/common/Scrollable/Scrollable';
import { useCallback } from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { ILeaseFilter } from '../interfaces';
import { useSearch } from './hooks/useSearch';
import { defaultFilter, LeaseFilter } from './LeaseFilter/LeaseFilter';
import { LeaseSearchResults } from './LeaseSearchResults/LeaseSearchResults';

/**
 * Component that displays a list of leases within PSP as well as a filter bar to control the displayed leases.
 */
export const LeaseListView = () => {
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
  } = useSearch(defaultFilter);

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
    <StyledListPage>
      <Center>
        <LeaseFilter filter={filter} setFilter={changeFilter} />
      </Center>
      <StyledScrollable>
        <StyledHeader>Leases &amp; Licenses</StyledHeader>
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
      </StyledScrollable>
    </StyledListPage>
  );
};

/**
 * Get an error message corresponding to what filter fields have been entered.
 * @param {ILeaseFilter} filter
 */
// const getNoResultErrorMessage = (filter: ILeaseFilter) => {
//   let message = 'Unable to find any records';
//   if (filter.lFileNo) {
//     message = 'There are no records for this L-File #';
//   } else if (filter.pidOrPin) {
//     message = 'There are no records for this PID/PIN';
//   } else if (filter.tenantName) {
//     message = 'There are no records for this Tenant Name';
//   }
//   return message;
// };

const StyledListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  gap: 2.5rem;
  padding: 0;
`;

const StyledScrollable = styled(Scrollable)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

const StyledHeader = styled.h3`
  text-align: left;
`;

export default LeaseListView;
