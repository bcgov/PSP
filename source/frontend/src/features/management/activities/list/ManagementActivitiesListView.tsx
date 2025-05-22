import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import ManagementFileIcon from '@/assets/images/management-icon.svg?react';
import * as CommonStyled from '@/components/common/styles';
import { PaddedScrollable } from '@/components/common/styles';
import {
  PROP_MGMT_ACTIVITY_STATUS_TYPES,
  PROP_MGMT_ACTIVITY_SUBTYPES_TYPES,
  PROP_MGMT_ACTIVITY_TYPES,
} from '@/constants/API';
import { useApiManagementActivities } from '@/hooks/pims-api/useApiManagementActivities';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';
import { mapLookupCode } from '@/utils';

import { ManagementActivityFilterModel } from '../models/ManagementActivityFilterModel';
import { ManagementActivitySearchResultModel } from '../models/ManagementActivitySearchResultModel';
import ActivitiesFilter from './filter/ActivitiesFilter';
import ManagementActivitySearchResults from './searchResults/ManagementActivitiesSearchResults';

/**
 * Page that displays Management files information.
 */
export const ManagementActivitiesListView: React.FC<unknown> = () => {
  const { getManagementActivitiesPagedApi } = useApiManagementActivities();

  // lookup codes to filter management list
  const lookupCodes = useLookupCodeHelpers();

  const activityStatusOptions = lookupCodes
    .getByType(PROP_MGMT_ACTIVITY_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const activityTypesOptions = lookupCodes
    .getByType(PROP_MGMT_ACTIVITY_TYPES)
    .map(c => mapLookupCode(c));

  const activitySubTypesOptions = lookupCodes
    .getByType(PROP_MGMT_ACTIVITY_SUBTYPES_TYPES)
    .map(c => mapLookupCode(c));

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
  } = useSearch<ApiGen_Concepts_PropertyActivity, Api_ManagementActivityFilter>(
    new ManagementActivityFilterModel().toApi(),
    getManagementActivitiesPagedApi,
    'No matching results can be found. Try widening your search criteria.',
  );

  React.useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  // update internal state whenever the filter bar changes
  const changeFilter = React.useCallback(
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
                activitySubTypesOptions={activitySubTypesOptions}
              />
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
