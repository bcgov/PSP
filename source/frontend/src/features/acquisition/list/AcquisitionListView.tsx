import { Button } from 'components/common/buttons/Button';
import Claims from 'constants/claims';
import { useApiAcquisitionFile } from 'hooks/pims-api/useApiAcquisitionFile';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { useSearch } from 'hooks/useSearch';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useEffect } from 'react';
import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { AcquisitionFilter, defaultAcquisitionFilter } from './AcquisitionFilter/AcquisitionFilter';
import { AcquisitionSearchResults } from './AcquisitionSearchResults/AcquisitionSearchResults';
import { AcquisitionSearchResultModel } from './AcquisitionSearchResults/models';
import { IAcquisitionFilter } from './interfaces';
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
  } = useSearch<Api_AcquisitionFile, IAcquisitionFilter>(
    defaultAcquisitionFilter,
    getAcquisitionFiles,
    'No matching results can be found. Try widening your search criteria.',
  );

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: IAcquisitionFilter) => {
      setFilter(filter);
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  return (
    <Styled.ListPage>
      <Styled.Scrollable>
        <Styled.PageHeader>Acquisition Files</Styled.PageHeader>
        <Styled.PageToolbar>
          <Row>
            <Col>
              <AcquisitionFilter filter={filter} setFilter={changeFilter} />
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

const StyledAddButton = styled(Button)`
  &.btn.btn-primary {
    background-color: ${props => props.theme.css.completedColor};
  }
`;

export default AcquisitionListView;
