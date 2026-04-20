import isEmpty from 'lodash/isEmpty';
import React, { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileExcel, FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import DispositionFileIcon from '@/assets/images/disposition-icon.svg?react';
import { StyledIconButton } from '@/components/common/buttons';
import { SelectOption } from '@/components/common/form/Select';
import * as CommonStyled from '@/components/common/styles';
import { PaddedScrollable, StyledAddButton } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Claims } from '@/constants';
import * as API from '@/constants/API';
import {
  DISPOSITION_FILE_STATUS_TYPES,
  DISPOSITION_STATUS_TYPES,
  DISPOSITION_TYPES,
} from '@/constants/API';
import { useApiDispositionFile } from '@/hooks/pims-api/useApiDispositionFile';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { IUserInfo, useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { formatGuid, generateMultiSortCriteria, mapLookupCode } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';
import { formatApiPersonNames } from '@/utils/personUtils';

import { useDispositionFileExport } from '../hooks/useDispositionFileExport';
import DispositionFilter from './DispositionFilter/DispositionFilter';
import { DispositionSearchResults } from './DispositionSearchResults/DispositionSearchResults';
import { DispositionFilterModel, DispositionSearchResultModel } from './models';

/**
 * Page that displays Disposition files information.
 */
export const DispositionListView: React.FC<unknown> = () => {
  const history = useHistory();
  const { hasClaim, obj } = useKeycloakWrapper();
  const { sub } = obj.userInfo as IUserInfo;
  const formattedGuid = formatGuid(sub);

  const { getDispositionFilesPagedApi } = useApiDispositionFile();
  const { retrieveUserInfo, retrieveUserInfoResponse } = useUserInfoRepository();

  const {
    getAllDispositionTeamMembers: { response: team, execute: loadDispositionTeam },
  } = useDispositionProvider();

  // lookup codes to filter disposition list
  const lookupCodes = useLookupCodeHelpers();

  const dispositionFileStatusOptions = lookupCodes
    .getByType(DISPOSITION_FILE_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const dispositionStatusOptions = lookupCodes
    .getByType(DISPOSITION_STATUS_TYPES)
    .map(c => mapLookupCode(c));

  const dispositionTypeOptions = lookupCodes
    .getByType(DISPOSITION_TYPES)
    .map(c => mapLookupCode(c));

  const pimsRegionsTypes = lookupCodes.getOptionsByType(API.REGION_TYPES);
  const pimsRegionOptions: MultiSelectOption[] = pimsRegionsTypes.map<MultiSelectOption>(x => {
    return { id: x.code as string, text: x.label };
  });

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
  } = useSearch<ApiGen_Concepts_DispositionFile, Api_DispositionFilter>(
    new DispositionFilterModel(userRegionsOptions).toApi(),
    getDispositionFilesPagedApi,
    'No matching results can be found. Try widening your search criteria.',
  );

  const { exportDispositionFiles } = useDispositionFileExport();

  /**
   * @param {'excel'} accept fetch is for type of EXCEL
   */
  const fetch = (accept: 'excel') => {
    // Call API with appropriate search parameters
    const query = toFilteredApiPaginateParams<Api_DispositionFilter>(
      currentPage,
      pageSize,
      sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
      filter,
    );

    exportDispositionFiles(query, accept);
  };

  // update internal state whenever the filter bar changes
  const changeFilter = React.useCallback(
    (filter: Api_DispositionFilter) => {
      setFilter(filter);
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  const handleResetFilter = useCallback(() => {
    setFilter(new DispositionFilterModel(userRegionsOptions).toApi());
  }, [setFilter, userRegionsOptions]);

  const dispositionTeamOptions = React.useMemo<SelectOption[]>(() => {
    const arr = team || [];
    return arr.map<SelectOption>(t => ({
      value: t.personId ? `P-${t.personId}` : `O-${t.organizationId}`,
      label: t.personId && t.person ? formatApiPersonNames(t.person) : t.organization?.name ?? '',
    }));
  }, [team]);

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  useEffect(() => {
    loadDispositionTeam();
  }, [loadDispositionTeam]);

  useEffect(() => {
    formattedGuid && retrieveUserInfo(formattedGuid);
  }, [formattedGuid, retrieveUserInfo]);

  return (
    <CommonStyled.ListPage>
      <PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <DispositionFileIcon title="Disposition file Icon" fill="currentColor" />
              <span className="ml-2">Disposition Files</span>
            </div>
            {hasClaim(Claims.DISPOSITION_ADD) && (
              <StyledAddButton onClick={() => history.push('/mapview/sidebar/disposition/new')}>
                <FaPlus />
                &nbsp;Add a Disposition File
              </StyledAddButton>
            )}
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <DispositionFilter
                initialValues={DispositionFilterModel.fromApi(
                  filter,
                  dispositionTeamOptions || [],
                  userRegionsOptions,
                )}
                setFilter={changeFilter}
                dispositionTeam={team || []}
                fileStatusOptions={dispositionFileStatusOptions}
                dispositionStatusOptions={dispositionStatusOptions}
                dispositionTypeOptions={dispositionTypeOptions}
                pimsRegionsOptions={pimsRegionOptions}
                dispositionTeamOptions={dispositionTeamOptions}
                onResetFilter={handleResetFilter}
              />
            </Col>
            <Col md="auto" className="px-0">
              <TooltipWrapper tooltipId="export-to-excel" tooltip="Export to Excel">
                <StyledIconButton onClick={() => fetch('excel')}>
                  <FaFileExcel data-testid="excel-icon" size={36} />
                </StyledIconButton>
              </TooltipWrapper>
            </Col>
          </Row>
        </CommonStyled.PageToolbar>

        <DispositionSearchResults
          results={results.map(a => DispositionSearchResultModel.fromApi(a))}
          totalItems={totalItems}
          pageIndex={currentPage}
          pageSize={pageSize}
          pageCount={totalPages}
          sort={sort}
          setSort={setSort}
          setPageSize={setPageSize}
          setPageIndex={setCurrentPage}
          loading={loading}
        ></DispositionSearchResults>
      </PaddedScrollable>
    </CommonStyled.ListPage>
  );
};

export default DispositionListView;

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
