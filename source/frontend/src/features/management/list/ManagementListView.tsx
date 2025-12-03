import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import ManagementFileIcon from '@/assets/images/management-icon.svg?react';
import * as CommonStyled from '@/components/common/styles';
import { PaddedScrollable, StyledAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import { MANAGEMENT_FILE_PURPOSE_TYPES, MANAGEMENT_FILE_STATUS_TYPES } from '@/constants/API';
import { useApiManagementFile } from '@/hooks/pims-api/useApiManagementFile';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { Api_ManagementFilter } from '@/models/api/ManagementFilter';
import { mapLookupCode } from '@/utils';

import ManagementFilter from './ManagementFilter/ManagementFilter';
import { ManagementSearchResults } from './ManagementSearchResults/ManagementSearchResults';
import { ManagementFilterModel, ManagementSearchResultModel } from './models';

/**
 * Page that displays Management files information.
 */
export const ManagementListView: React.FC<unknown> = () => {
  const history = useHistory();
  const { hasClaim } = useKeycloakWrapper();
  const { getManagementFilesPagedApi } = useApiManagementFile();
  const {
    getAllManagementTeamMembers: { response: team, execute: loadManagementTeam },
  } = useManagementFileRepository();

  // lookup codes to filter management list
  const lookupCodes = useLookupCodeHelpers();

  const managementFileStatusOptions = lookupCodes
    .getByType(MANAGEMENT_FILE_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const managementPurposeOptions = lookupCodes
    .getByType(MANAGEMENT_FILE_PURPOSE_TYPES)
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
  } = useSearch<ApiGen_Concepts_ManagementFile, Api_ManagementFilter>(
    new ManagementFilterModel().toApi(),
    getManagementFilesPagedApi,
    'No matching results can be found. Try widening your search criteria.',
  );

  React.useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  React.useEffect(() => {
    loadManagementTeam();
  }, [loadManagementTeam]);

  // update internal state whenever the filter bar changes
  const changeFilter = React.useCallback(
    (filter: Api_ManagementFilter) => {
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
              <span className="ml-2">Management Files</span>
            </div>
            {hasClaim(Claims.MANAGEMENT_ADD) && (
              <StyledAddButton onClick={() => history.push('/mapview/sidebar/management/new')}>
                <FaPlus />
                &nbsp;Add a Management File
              </StyledAddButton>
            )}
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <ManagementFilter
                filter={filter}
                setFilter={changeFilter}
                managementTeam={team ?? []}
                fileStatusOptions={managementFileStatusOptions}
                managementPurposeOptions={managementPurposeOptions}
              />
            </Col>
          </Row>
        </CommonStyled.PageToolbar>

        <ManagementSearchResults
          results={results.map(a => ManagementSearchResultModel.fromApi(a))}
          totalItems={totalItems}
          pageIndex={currentPage}
          pageSize={pageSize}
          pageCount={totalPages}
          sort={sort}
          setSort={setSort}
          setPageSize={setPageSize}
          setPageIndex={setCurrentPage}
          loading={loading}
        ></ManagementSearchResults>
      </PaddedScrollable>
    </CommonStyled.ListPage>
  );
};

export default ManagementListView;

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
