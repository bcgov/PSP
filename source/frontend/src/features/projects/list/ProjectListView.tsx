import { StyledIconButton } from 'components/common/buttons';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { useApiProjects } from 'hooks/pims-api/useApiProjects';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { useSearch } from 'hooks/useSearch';
import { IProjectSearchResult } from 'interfaces';
import { isEmpty } from 'lodash';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import { generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';

import { IProjectFilter } from '../interfaces';
import { defaultFilter, ProjectFilter } from './ProjectFilter/ProjectFilter';
import { ProjectSearchResults } from './ProjectSearchResults/ProjectSearchResults';
import * as Styled from './styles';

/**
 * Page that displays Project files information.
 */
export const ProjectListView: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const history = useHistory();
  const { getProjects } = useApiProjects();

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
  } = useSearch<IProjectSearchResult, IProjectFilter>(defaultFilter, getProjects);

  // update internal state whenever the filter bar changes
  const changeFilter = useCallback(
    (filter: IProjectFilter) => {
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
        <Styled.PageHeader>Projects</Styled.PageHeader>
        <Styled.PageToolbar>
          <Row>
            <Col>
              <ProjectFilter filter={filter} setFilter={changeFilter} />
            </Col>
          </Row>
        </Styled.PageToolbar>
        <ProjectSearchResults
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
        ></ProjectSearchResults>
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default ProjectListView;
