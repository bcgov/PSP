import { isEmpty } from 'lodash';
import { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileAlt, FaFileExcel, FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import { StyledIconButton } from '@/components/common/buttons';
import * as CommonStyled from '@/components/common/styles';
import { PaddedScrollable, StyledAddButton } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import Claims from '@/constants/claims';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useSearch } from '@/hooks/useSearch';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { generateMultiSortCriteria } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';

import { useLeaseExport } from '../hooks/useLeaseExport';
import { ILeaseFilter } from '../interfaces';
import { defaultFilter, LeaseFilter } from './LeaseFilter/LeaseFilter';
import { LeaseSearchResults } from './LeaseSearchResults/LeaseSearchResults';

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
  } = useSearch<ApiGen_Concepts_Lease, ILeaseFilter>(defaultFilter, getLeases);

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
    },
    [setFilter],
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  return (
    <CommonStyled.ListPage>
      <PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <LeaseIcon title="Lease and Licence icon" fill="currentColor" />
              <span className="ml-2">Leases &amp; Licences</span>
            </div>
            {hasClaim(Claims.LEASE_ADD) && (
              <StyledAddButton onClick={() => history.push('/mapview/sidebar/lease/new')}>
                <FaPlus />
                &nbsp;Create a Lease/Licence
              </StyledAddButton>
            )}
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <LeaseFilter filter={filter} setFilter={changeFilter} />
            </Col>
            <Col md="auto" className="px-0">
              <TooltipWrapper tooltipId="export-to-excel" tooltip="Export to Excel">
                <StyledIconButton onClick={() => fetch('excel')}>
                  <FaFileExcel data-testid="excel-icon" size={36} />
                </StyledIconButton>
              </TooltipWrapper>
            </Col>
            <Col md="auto" className="px-0">
              <TooltipWrapper tooltipId="export-to-excel" tooltip="Export to CSV">
                <StyledIconButton onClick={() => fetch('csv')}>
                  <FaFileAlt data-testid="csv-icon" size={36} />
                </StyledIconButton>
              </TooltipWrapper>
            </Col>
          </Row>
        </CommonStyled.PageToolbar>

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
      </PaddedScrollable>
    </CommonStyled.ListPage>
  );
};

export default LeaseListView;

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;

  svg {
    vertical-align: baseline;
  }
`;
