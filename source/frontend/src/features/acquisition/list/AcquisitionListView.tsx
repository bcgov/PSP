import { isEmpty } from 'lodash';
import React, { useCallback, useEffect, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileExcel, FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import AcquisitionFileIcon from '@/assets/images/acquisition-icon.svg?react';
import { StyledIconButton } from '@/components/common/buttons/IconButton';
import { PaddedScrollable, StyledAddButton } from '@/components/common/styles';
import * as CommonStyled from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import * as API from '@/constants/API';
import { ACQUISITION_FILE_STATUS_TYPES } from '@/constants/API';
import Claims from '@/constants/claims';
import { useApiAcquisitionFile } from '@/hooks/pims-api/useApiAcquisitionFile';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import useKeycloakWrapper, { IUserInfo } from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';
import { mapLookupCode } from '@/utils/mapLookupCode';
import { formatApiPersonNames } from '@/utils/personUtils';
import { exists, formatGuid, generateMultiSortCriteria } from '@/utils/utils';

import { useAcquisitionFileExport } from '../hooks/useAcquisitionFileExport';
import { AcquisitionFilter } from './AcquisitionFilter/AcquisitionFilter';
import { AcquisitionSearchResults } from './AcquisitionSearchResults/AcquisitionSearchResults';
import { AcquisitionSearchResultModel } from './AcquisitionSearchResults/models';
import { AcquisitionFilterModel, ApiGen_Concepts_AcquisitionFilter } from './interfaces';

/**
 * Page that displays acquisition files information.
 */
export const AcquisitionListView: React.FunctionComponent<
  React.PropsWithChildren<unknown>
> = () => {
  const history = useHistory();
  const { hasClaim, obj } = useKeycloakWrapper();
  const { sub } = obj.userInfo as IUserInfo;
  const formattedGuid = formatGuid(sub);

  const lookupCodes = useLookupCodeHelpers();
  const { retrieveUserInfo, retrieveUserInfoResponse } = useUserInfoRepository();
  const { getAcquisitionFiles } = useApiAcquisitionFile();
  const {
    getAllAcquisitionFileTeamMembers: { response: team, execute: loadAcquisitionTeam },
  } = useAcquisitionProvider();

  const pimsRegionsTypes = lookupCodes.getOptionsByType(API.REGION_TYPES);
  const pimsRegionOptions: MultiSelectOption[] = pimsRegionsTypes.map<MultiSelectOption>(x => {
    return { id: x.code as string, text: x.label };
  });

  const acquisitionStatusOptions = lookupCodes
    .getByType(ACQUISITION_FILE_STATUS_TYPES)
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
  } = useSearch<ApiGen_Concepts_AcquisitionFile, ApiGen_Concepts_AcquisitionFilter>(
    new AcquisitionFilterModel(userRegionsOptions).toApi(),
    getAcquisitionFiles,
    'No matching results can be found. Try widening your search criteria.',
  );

  const { exportAcquisitionFiles } = useAcquisitionFileExport();

  /**
   * @param {'excel'} accept fetch is for type of EXCEL
   */
  const fetch = (accept: 'excel') => {
    // Call API with appropriate search parameters
    const query = toFilteredApiPaginateParams<ApiGen_Concepts_AcquisitionFilter>(
      currentPage,
      pageSize,
      sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
      filter,
    );

    exportAcquisitionFiles(query, accept);
  };

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: ApiGen_Concepts_AcquisitionFilter) => {
      setFilter(filter);
    },
    [setFilter],
  );

  const handleResetFilter = useCallback(() => {
    setFilter(new AcquisitionFilterModel(userRegionsOptions).toApi());
  }, [setFilter, userRegionsOptions]);

  const acquisitionTeamOptions = useMemo(() => {
    if (exists(team)) {
      return team?.map<MultiSelectOption>(x => ({
        id: x.personId ? `P-${x.personId}` : `O-${x.organizationId}`,
        text: x.personId ? formatApiPersonNames(x.person) : x.organization?.name ?? '',
      }));
    } else {
      return [];
    }
  }, [team]);

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  useEffect(() => {
    loadAcquisitionTeam();
  }, [loadAcquisitionTeam]);

  useEffect(() => {
    formattedGuid && retrieveUserInfo(formattedGuid);
  }, [formattedGuid, retrieveUserInfo]);

  return (
    <CommonStyled.ListPage>
      <PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <AcquisitionFileIcon title="Acquisition file icon" fill="currentColor" />
              <span className="ml-2">Acquisition Files</span>
            </div>
            {hasClaim(Claims.ACQUISITION_ADD) && (
              <StyledAddButton onClick={() => history.push('/mapview/sidebar/acquisition/new')}>
                <FaPlus />
                &nbsp;Create an Acquisition File
              </StyledAddButton>
            )}
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <AcquisitionFilter
                initialValues={AcquisitionFilterModel.fromApi(
                  filter,
                  team || [],
                  userRegionsOptions,
                )}
                acquisitionTeamOptions={acquisitionTeamOptions || []}
                pimsRegionsOptions={pimsRegionOptions}
                acquisitionStatusOptions={acquisitionStatusOptions}
                setFilter={changeFilter}
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

        <AcquisitionSearchResults
          results={results.map(a => AcquisitionSearchResultModel.fromApi(a))}
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

export default AcquisitionListView;

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
