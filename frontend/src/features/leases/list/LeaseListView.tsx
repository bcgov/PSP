import { Button } from 'components/common/form';
import TooltipWrapper from 'components/common/TooltipWrapper';
import Claims from 'constants/claims';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { useSearch } from 'hooks/useSearch';
import { ILeaseSearchResult } from 'interfaces';
import { isEmpty } from 'lodash';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { FaFileAlt, FaFileExcel } from 'react-icons/fa';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import { generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';

import { useLeaseExport } from '../hooks/useLeaseExport';
import { ILeaseFilter } from '../interfaces';
import { defaultFilter, LeaseFilter } from './LeaseFilter/LeaseFilter';
import { LeaseSearchResults } from './LeaseSearchResults/LeaseSearchResults';
import * as Styled from './styles';

/**
 * Component that displays a list of leases within PSP as well as a filter bar to control the displayed leases.
 */
export const LeaseListView: React.FunctionComponent = () => {
  const history = useHistory();
  const { getLeases } = useApiLeases();
  const { hasClaim } = useKeycloakWrapper();
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

  const { exportLeases } = useLeaseExport();

  /**
   * @param {'csv' | 'excel'} accept Whether the fetch is for type of CSV or EXCEL
   * @param {boolean} getAllFields Enable this field to generate report with additional fields. For SRES only.
   */
  const fetch = (accept: 'csv' | 'excel') => {
    // Call API with appropriate search parameters
    const query = toFilteredApiPaginateParams<ILeaseFilter>(
      currentPage,
      pageSize,
      sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
      filter,
    );

    exportLeases(query, accept);
  };

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: ILeaseFilter) => {
      setFilter(filter);
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  return (
    <Styled.ListPage>
      <Styled.Scrollable>
        <Styled.PageHeader>Leases &amp; Licenses</Styled.PageHeader>
        <Styled.PageToolbar>
          <LeaseFilter filter={filter} setFilter={changeFilter} />
          <Styled.Spacer />
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
            <Styled.FileIcon>
              <FaFileExcel data-testid="excel-icon" size={36} onClick={() => fetch('excel')} />
            </Styled.FileIcon>
          </TooltipWrapper>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to CSV">
            <Styled.FileIcon>
              <FaFileAlt data-testid="csv-icon" size={36} onClick={() => fetch('csv')} />
            </Styled.FileIcon>
          </TooltipWrapper>
          <Styled.Spacer />
        </Styled.PageToolbar>
        {hasClaim(Claims.LEASE_ADD) && (
          <StyledAddButton onClick={() => history.push('/lease/new')}>
            <FaPlus />
            &nbsp;Add A Lease/License
          </StyledAddButton>
        )}
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

export const StyledAddButton = styled(Button)`
  &.btn.btn-primary {
    background-color: ${props => props.theme.css.completedColor};
  }
`;

export default LeaseListView;
