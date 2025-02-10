import React, { useEffect } from 'react';
import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import ResearchFileIcon from '@/assets/images/research-icon.svg?react';
import * as CommonStyled from '@/components/common/styles';
import { StyledAddButton } from '@/components/common/styles';
import Claims from '@/constants/claims';
import { useApiResearchFile } from '@/hooks/pims-api/useApiResearchFile';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useSearch } from '@/hooks/useSearch';
import {
  IResearchSearchResult,
  ResearchSearchResultModel,
} from '@/interfaces/IResearchSearchResult';

import { IResearchFilter } from '../interfaces';
import ResearchFilter, { defaultResearchFilter } from './ResearchFilter/ResearchFilter';
import { ResearchSearchResults } from './ResearchSearchResults/ResearchSearchResults';

/**
 * Page that displays leases information.
 */
export const ResearchListView: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const history = useHistory();
  const { getResearchFiles } = useApiResearchFile();
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
  } = useSearch<IResearchSearchResult, IResearchFilter>(
    defaultResearchFilter,
    getResearchFiles,
    'No matching results can be found. Try widening your search criteria.',
  );

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: IResearchFilter) => {
      setFilter(filter);
    },
    [setFilter],
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  return (
    <CommonStyled.ListPage>
      <CommonStyled.PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <ResearchFileIcon title="Research file icon" fill="currentColor" />
              <span className="ml-2">Research Files</span>
            </div>
            {hasClaim(Claims.RESEARCH_ADD) && (
              <StyledAddButton onClick={() => history.push('/mapview/sidebar/research/new')}>
                <FaPlus />
                &nbsp;Create a Research File
              </StyledAddButton>
            )}
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <ResearchFilter filter={filter} setFilter={changeFilter} />
            </Col>
          </Row>
        </CommonStyled.PageToolbar>

        <ResearchSearchResults
          results={results.map(r => ResearchSearchResultModel.fromApi(r))}
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
      </CommonStyled.PaddedScrollable>
    </CommonStyled.ListPage>
  );
};

export default ResearchListView;

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;
`;
