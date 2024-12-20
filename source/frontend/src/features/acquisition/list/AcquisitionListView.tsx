import { isEmpty } from 'lodash';
import React, { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileExcel, FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';

import AcquisitionFileIcon from '@/assets/images/acquisition-icon.svg?react';
import { StyledIconButton } from '@/components/common/buttons/IconButton';
import { StyledAddButton } from '@/components/common/styles';
import * as CommonStyled from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import Claims from '@/constants/claims';
import { useApiAcquisitionFile } from '@/hooks/pims-api/useApiAcquisitionFile';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useSearch } from '@/hooks/useSearch';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';
import { generateMultiSortCriteria } from '@/utils/utils';

import { useAcquisitionFileExport } from '../hooks/useAcquisitionFileExport';
import { AcquisitionFilter } from './AcquisitionFilter/AcquisitionFilter';
import { AcquisitionSearchResults } from './AcquisitionSearchResults/AcquisitionSearchResults';
import { AcquisitionSearchResultModel } from './AcquisitionSearchResults/models';
import { AcquisitionFilterModel, ApiGen_Concepts_AcquisitionFilter } from './interfaces';
import * as Styled from './styles';

/**
 * Page that displays acquisition files information.
 */
export const AcquisitionListView: React.FunctionComponent<
  React.PropsWithChildren<unknown>
> = () => {
  const history = useHistory();
  const { getAcquisitionFiles } = useApiAcquisitionFile();
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
  } = useSearch<ApiGen_Concepts_AcquisitionFile, ApiGen_Concepts_AcquisitionFilter>(
    new AcquisitionFilterModel().toApi(),
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

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  const {
    getAllAcquisitionFileTeamMembers: { response: team, execute: loadAcquisitionTeam },
  } = useAcquisitionProvider();

  useEffect(() => {
    loadAcquisitionTeam();
  }, [loadAcquisitionTeam]);

  return (
    <Styled.ListPage>
      <Styled.Scrollable>
        <CommonStyled.H1>
          <AcquisitionFileIcon title="Acquisition file icon" fill="currentColor" />
          <span className="ml-2">Acquisition Files</span>
        </CommonStyled.H1>
        <Styled.PageToolbar>
          <Row>
            <Col>
              <AcquisitionFilter
                filter={filter}
                setFilter={changeFilter}
                acquisitionTeam={team || []}
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
        </Styled.PageToolbar>
        {hasClaim(Claims.ACQUISITION_ADD) && (
          <StyledAddButton onClick={() => history.push('/mapview/sidebar/acquisition/new')}>
            <FaPlus />
            &nbsp;Create an acquisition file
          </StyledAddButton>
        )}
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
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default AcquisitionListView;
