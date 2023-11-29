import { AxiosResponse } from 'axios';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import { StyledAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import { IPaginateRequest } from '@/hooks/pims-api/interfaces/IPaginateRequest';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { useSearch } from '@/hooks/useSearch';
import { IPagedItems } from '@/interfaces';
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';

import DispositionFilter from './DispositionFilter/DispositionFilter';
import { DispositionSearchResults } from './DispositionSearchResults/DispositionSearchResults';
import { DispositionFilterModel, DispositionSearchResultModel } from './models';
import * as S from './styles';

/**
 * Page that displays Disposition files information.
 */
export const DispositionListView: React.FC<unknown> = () => {
  const { hasClaim } = useKeycloakWrapper();
  const history = useHistory();

  const searchDispositionFiles = async (
    params: IPaginateRequest<Api_DispositionFilter>,
  ): Promise<AxiosResponse<IPagedItems<Api_DispositionFile>, any>> => {
    const mockResponse: any = {
      data: {
        items: [],
        page: 1,
        pageIndex: 0,
        quantity: 0,
        total: 0,
      },
    };

    return mockResponse;
  };

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
    searchDispositionFiles,
    'No matching results can be found. Try widening your search criteria.',
  );

  React.useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

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
                dispositionTeam={[]}
                physicalFileStatusOptions={[]}
                dispositionStatusOptions={[]}
                dispositionTypeOptions={[]}
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
