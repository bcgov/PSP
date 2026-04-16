import React, { useCallback, useEffect, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import ManagementFileIcon from '@/assets/images/management-icon.svg?react';
import { SelectOption } from '@/components/common/form/Select';
import * as CommonStyled from '@/components/common/styles';
import { PaddedScrollable, StyledAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import * as API from '@/constants/API';
import { MANAGEMENT_FILE_PURPOSE_TYPES, MANAGEMENT_FILE_STATUS_TYPES } from '@/constants/API';
import { useApiManagementFile } from '@/hooks/pims-api/useApiManagementFile';
import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { IUserInfo, useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { Api_ManagementFilter } from '@/models/api/ManagementFilter';
import { formatGuid, mapLookupCode } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import ManagementFilter from './ManagementFilter/ManagementFilter';
import { ManagementSearchResults } from './ManagementSearchResults/ManagementSearchResults';
import { ManagementFilterModel, ManagementSearchResultModel } from './models';

/**
 * Page that displays Management files information.
 */
export const ManagementListView: React.FC<unknown> = () => {
  const history = useHistory();
  const { hasClaim, obj } = useKeycloakWrapper();
  const { sub } = obj.userInfo as IUserInfo;
  const formattedGuid = formatGuid(sub);

  const { retrieveUserInfo, retrieveUserInfoResponse } = useUserInfoRepository();
  const { getManagementFilesPagedApi } = useApiManagementFile();
  const {
    getAllManagementTeamMembers: { response: team, execute: loadManagementTeam },
  } = useManagementFileRepository();

  // lookup codes to filter management list
  const lookupCodes = useLookupCodeHelpers();

  const pimsRegionsTypes = lookupCodes.getOptionsByType(API.REGION_TYPES);
  const pimsRegionOptions: MultiSelectOption[] = pimsRegionsTypes.map<MultiSelectOption>(x => {
    return { id: x.code as string, text: x.label };
  });

  const managementFileStatusOptions = lookupCodes
    .getByType(MANAGEMENT_FILE_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const managementPurposeOptions = lookupCodes
    .getByType(MANAGEMENT_FILE_PURPOSE_TYPES)
    .map(c => mapLookupCode(c));

  const userRegionsIds: string[] =
    retrieveUserInfoResponse?.userRegions.map(x => x.regionCode.toString()) ?? [];
  const userRegionsOptions: MultiSelectOption[] = pimsRegionsTypes
    .filter(opt => userRegionsIds.includes(opt.code))
    .map<MultiSelectOption>(x => {
      return { id: x.code as string, text: x.label };
    });

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
    new ManagementFilterModel(userRegionsOptions).toApi(),
    getManagementFilesPagedApi,
    'No matching results can be found. Try widening your search criteria.',
  );

  const managementTeamOptions = useMemo<SelectOption[]>(() => {
    const arr = team ?? [];
    return arr.map<SelectOption>(t => ({
      value: t.personId ? `P-${t.personId}` : `O-${t.organizationId}`,
      label: t.personId && t.person ? formatApiPersonNames(t.person) : t.organization?.name ?? '',
    }));
  }, [team]);

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: Api_ManagementFilter) => {
      setFilter(filter);
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  const handleResetFilter = useCallback(() => {
    setFilter(new ManagementFilterModel(userRegionsOptions).toApi());
  }, [setFilter, userRegionsOptions]);

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  useEffect(() => {
    loadManagementTeam();
  }, [loadManagementTeam]);

  useEffect(() => {
    formattedGuid && retrieveUserInfo(formattedGuid);
  }, [formattedGuid, retrieveUserInfo]);

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
                initialValues={ManagementFilterModel.fromApi(
                  filter,
                  managementTeamOptions ?? [],
                  userRegionsOptions,
                )}
                managementTeam={team ?? []}
                fileStatusOptions={managementFileStatusOptions}
                managementPurposeOptions={managementPurposeOptions}
                pimsRegionsOptions={pimsRegionOptions}
                managementTeamOptions={managementTeamOptions}
                setFilter={changeFilter}
                onResetFilter={handleResetFilter}
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
