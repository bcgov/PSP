import { isEmpty } from 'lodash';
import { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileAlt, FaFileExcel } from 'react-icons/fa';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import ManagementFileIcon from '@/assets/images/management-icon.svg?react';
import { StyledIconButton } from '@/components/common/buttons/IconButton';
import * as CommonStyled from '@/components/common/styles';
import { PaddedScrollable } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { MGMT_ACTIVITY_STATUS_TYPES, MGMT_ACTIVITY_TYPES } from '@/constants/API';
import { useApiManagementActivities } from '@/hooks/pims-api/useApiManagementActivities';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';
import { generateMultiSortCriteria, mapLookupCode } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';

import { useManagementActivityExport } from '../../hooks/useManagementActivityExport';
import { ManagementActivityFilterModel } from '../models/ManagementActivityFilterModel';
import { ManagementActivitySearchResultModel } from '../models/ManagementActivitySearchResultModel';
import ActivitiesFilter from './filter/ActivitiesFilter';
import ManagementActivitySearchResults from './searchResults/ManagementActivitiesSearchResults';

/**
 * Page that displays Management Activities information.
 */
export const ManagementActivitiesListView: React.FC<unknown> = () => {
  const { getManagementActivitiesPagedApi } = useApiManagementActivities();

  // lookup codes to filter management list
  const lookupCodes = useLookupCodeHelpers();

  const activityStatusOptions = lookupCodes
    .getByType(MGMT_ACTIVITY_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const activityTypesOptions = lookupCodes
    .getByType(MGMT_ACTIVITY_TYPES)
    .map(c => mapLookupCode(c));

  const { exportManagementActivities } = useManagementActivityExport();

  /**
   * @param {'csv' | 'excel'} accept Whether the fetch is for type of CSV or EXCEL
   * @param {boolean} getAllFields Enable this field to generate report with additional fields. For SRES only.
   */
  const fetch = (accept: 'csv' | 'excel') => {
    // Call API with appropriate search parameters
    const query = toFilteredApiPaginateParams<Api_ManagementActivityFilter>(
      currentPage,
      pageSize,
      sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
      filter,
    );

    exportManagementActivities(query, accept);
  };

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
  } = useSearch<ApiGen_Concepts_ManagementActivity, Api_ManagementActivityFilter>(
    new ManagementActivityFilterModel().toApi(),
    getManagementActivitiesPagedApi,
    'No matching results can be found. Try widening your search criteria.',
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: Api_ManagementActivityFilter) => {
      setFilter(filter);
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  return (
    <CommonStyled.ListPage>
      <PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <ManagementFileIcon title="Management file Icon" fill="currentColor" />
              <span className="ml-2">Management Activities</span>
            </div>
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <ActivitiesFilter
                filter={filter}
                setFilter={changeFilter}
                activityStatusOptions={activityStatusOptions}
                activityTypesOptions={activityTypesOptions}
              />
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

        <ManagementActivitySearchResults
          results={results.map(a => ManagementActivitySearchResultModel.fromApi(a))}
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

export default ManagementActivitiesListView;

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;

  svg {
    vertical-align: baseline;
  }
  svg g path {
    fill: currentColor;
  }
`;
