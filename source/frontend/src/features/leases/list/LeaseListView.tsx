import { isEmpty } from 'lodash';
import { useCallback, useEffect, useMemo } from 'react';
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
import * as API from '@/constants/API';
import Claims from '@/constants/claims';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import useKeycloakWrapper, { IUserInfo } from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, formatGuid, generateMultiSortCriteria } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';
import { formatApiPersonNames } from '@/utils/personUtils';

import { useLeaseExport } from '../hooks/useLeaseExport';
import { ILeaseFilter } from '../interfaces';
import { LeaseFilter } from './LeaseFilter/LeaseFilter';
import { LeaseFilterModel } from './LeaseFilter/models/LeaseFilterModel';
import { LeaseSearchResults } from './LeaseSearchResults/LeaseSearchResults';

const initialLeaseStatusTypes: string[] = [
  'ACTIVE',
  'ARCHIVED',
  'DISCARD',
  'DRAFT',
  'DUPLICATE',
  'EXPIRED',
  'INACTIVE',
  'TERMINATED',
];

/**
 * Page that displays leases information.
 */
export const LeaseListView: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const history = useHistory();
  const { hasClaim, obj } = useKeycloakWrapper();
  const { sub } = obj.userInfo as IUserInfo;
  const formattedGuid = formatGuid(sub);
  const lookupCodes = useLookupCodeHelpers();

  const { getLeases } = useApiLeases();
  const { exportLeases } = useLeaseExport();
  const { retrieveUserInfo, retrieveUserInfoResponse } = useUserInfoRepository();
  const {
    getAllLeaseTeamMembers: { response: leaseTeam, execute: loadLeaseTeam },
  } = useLeaseRepository();

  const leaseProgramTypes = lookupCodes.getOptionsByType(API.LEASE_PROGRAM_TYPES);
  const leaseProgramOptions: MultiSelectOption[] = leaseProgramTypes.map<MultiSelectOption>(x => {
    return { id: x.value as string, text: x.label };
  });

  const leaseStatusTypes = lookupCodes.getOptionsByType(API.LEASE_STATUS_TYPES);
  const leaseStatusOptions: MultiSelectOption[] = leaseStatusTypes.map<MultiSelectOption>(x => {
    return { id: x.value as string, text: x.label };
  });
  const initialStatusOptions = leaseStatusOptions.filter(x =>
    initialLeaseStatusTypes.includes(x.id),
  );

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

  const leaseTeamOptions = useMemo(() => {
    if (exists(leaseTeam)) {
      return leaseTeam?.map<MultiSelectOption>(x => ({
        id: x.personId ? `P-${x.personId}` : `O-${x.organizationId}`,
        text: x.personId ? formatApiPersonNames(x.person) : x.organization?.name ?? '',
      }));
    } else {
      return [];
    }
  }, [leaseTeam]);

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
  } = useSearch<ApiGen_Concepts_Lease, ILeaseFilter>(
    new LeaseFilterModel(userRegionsOptions, initialStatusOptions).toApi(),
    getLeases,
  );

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

  const handleResetFilter = useCallback(() => {
    setFilter(new LeaseFilterModel(userRegionsOptions, initialStatusOptions).toApi());
  }, [initialStatusOptions, setFilter, userRegionsOptions]);

  useEffect(() => {
    loadLeaseTeam();
  }, [loadLeaseTeam]);

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  useEffect(() => {
    formattedGuid && retrieveUserInfo(formattedGuid);
  }, [formattedGuid, retrieveUserInfo]);

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
              <LeaseFilter
                initialValues={LeaseFilterModel.fromApi(
                  filter,
                  leaseTeam || [],
                  userRegionsOptions,
                  initialStatusOptions,
                )}
                pimsRegionsOptions={pimsRegionOptions}
                leaseStatusOptions={leaseStatusOptions}
                leaseTeamOptions={leaseTeamOptions}
                leaseProgramOptions={leaseProgramOptions}
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
