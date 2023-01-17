import { useApiProjects } from 'hooks/pims-api/useApiProjects';
import { useSearch } from 'hooks/useSearch';
import { Api_Project } from 'models/api/Project';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';

import { IProjectFilter } from '../interfaces';
import { defaultFilter, ProjectFilter } from './ProjectFilter/ProjectFilter';
import { ProjectSearchResultModel } from './ProjectSearchResults/models';
import { ProjectSearchResults } from './ProjectSearchResults/ProjectSearchResults';
import * as Styled from './styles';

/**
 * Page that displays Project files information.
 */
export const ProjectListView: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const { searchProjects } = useApiProjects();

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
  } = useSearch<Api_Project, IProjectFilter>(
    defaultFilter,
    searchProjects,
    'No matching results can be found. Try widening your search criteria.',
  );

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
          results={results.map(x => ProjectSearchResultModel.fromApi(x))}
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
