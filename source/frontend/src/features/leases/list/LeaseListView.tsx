import { isEmpty } from 'lodash';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileAlt, FaFileExcel } from 'react-icons/fa';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { Button, StyledIconButton } from '@/components/common/buttons';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import Claims from '@/constants/claims';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useSearch } from '@/hooks/useSearch';
import { ILeaseSearchResult } from '@/interfaces';
import { generateMultiSortCriteria } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';

import { useLeaseExport } from '../hooks/useLeaseExport';
import { ILeaseFilter } from '../interfaces';
import { defaultFilter, LeaseFilter } from './LeaseFilter/LeaseFilter';
import { LeaseSearchResults } from './LeaseSearchResults/LeaseSearchResults';
import * as Styled from './styles';

/**
 * Page that displays leases information.
 */
export const LeaseListView: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const history = useHistory();
  const { getLeases } = useApiLeases();
  const { hasClaim } = useKeycloakWrapper();
  const {
    results,
    filter,
    sort,
    error,
    totalItems,
    currentPage,
    totalPages,
    pageSize,
    setFilter,
    setSort,
    setCurrentPage,
    setPageSize,
    loading,
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
          <Row>
            <Col>
              <LeaseFilter filter={filter} setFilter={changeFilter} />
            </Col>
            <Col md="auto" className="px-0">
              <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
                <StyledIconButton onClick={() => fetch('excel')}>
                  <FaFileExcel data-testid="excel-icon" size={36} />
                </StyledIconButton>
              </TooltipWrapper>
            </Col>
            <Col md="auto" className="px-0">
              <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to CSV">
                <StyledIconButton onClick={() => fetch('csv')}>
                  <FaFileAlt data-testid="csv-icon" size={36} />
                </StyledIconButton>
              </TooltipWrapper>
            </Col>
          </Row>
        </Styled.PageToolbar>
        {hasClaim(Claims.LEASE_ADD) && (
          <StyledAddButton onClick={() => history.push('/mapview/sidebar/lease/new')}>
            <FaPlus />
            &nbsp;Create a Lease/License
          </StyledAddButton>
        )}
        <LeaseSearchResults
          results={results}
          totalItems={totalItems}
          pageIndex={currentPage}
          pageSize={pageSize}
          pageCount={totalPages}
          sort={sort}
          setSort={setSort}
          setPageSize={setPageSize}
          setPageIndex={setCurrentPage}
          loading={loading}
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

const StyledAddButton = styled(Button)`
  &.btn.btn-primary,
  &.btn.btn-primary:active {
    background-color: ${props => props.theme.css.completedColor};
  }
`;

export default LeaseListView;
