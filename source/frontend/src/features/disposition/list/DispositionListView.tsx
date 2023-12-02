import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import { StyledAddButton } from '@/components/common/styles';
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
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { mapLookupCode } from '@/utils';

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
  } = useSearch<Api_DispositionFile, Api_DispositionFilter>(
    new DispositionFilterModel().toApi(),
    getDispositionFilesPagedApi,
    'No matching results can be found. Try widening your search criteria.',
  );

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
