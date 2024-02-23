import isEmpty from 'lodash/isEmpty';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileExcel, FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import { StyledIconButton } from '@/components/common/buttons';
import { StyledAddButton } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Claims } from '@/constants';
import {
  DISPOSITION_FILE_STATUS_TYPES,
  DISPOSITION_STATUS_TYPES,
  DISPOSITION_TYPES,
} from '@/constants/API';
import { useApiDispositionFile } from '@/hooks/pims-api/useApiDispositionFile';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useSearch } from '@/hooks/useSearch';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { generateMultiSortCriteria, mapLookupCode } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';

import { useDispositionFileExport } from '../hooks/useDispositionFileExport';
import DispositionFilter from './DispositionFilter/DispositionFilter';
import { DispositionSearchResults } from './DispositionSearchResults/DispositionSearchResults';
import { DispositionFilterModel, DispositionSearchResultModel } from './models';
import * as S from './styles';

/**
 * Page that displays Disposition files information.
 */
export const DispositionListView: React.FC<unknown> = () => {
  const history = useHistory();
  const { hasClaim } = useKeycloakWrapper();
  const { getDispositionFilesPagedApi } = useApiDispositionFile();
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
    new DispositionFilterModel().toApi(),
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

  React.useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  React.useEffect(() => {
    loadDispositionTeam();
  }, [loadDispositionTeam]);

  // update internal state whenever the filter bar changes
  const changeFilter = React.useCallback(
    (filter: Api_DispositionFilter) => {
      setFilter(filter);
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  return (
    <S.ListPage>
      <S.Scrollable>
        <S.PageHeader>Disposition Files</S.PageHeader>
        <S.PageToolbar>
          <Row>
            <Col>
              <DispositionFilter
                filter={filter}
                setFilter={changeFilter}
                dispositionTeam={team || []}
                fileStatusOptions={dispositionFileStatusOptions}
                dispositionStatusOptions={dispositionStatusOptions}
                dispositionTypeOptions={dispositionTypeOptions}
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
        </S.PageToolbar>
        {hasClaim(Claims.DISPOSITION_ADD) && (
          <StyledAddButton onClick={() => history.push('/mapview/sidebar/disposition/new')}>
            <FaPlus />
            &nbsp;Add a Disposition File
          </StyledAddButton>
        )}
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
      </S.Scrollable>
    </S.ListPage>
  );
};

export default DispositionListView;
