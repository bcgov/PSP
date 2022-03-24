import { Button } from 'components/common/form';
import { useApiLeases } from 'hooks/pims-api/useApiLeases';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { useSearch } from 'hooks/useSearch';
import { ILeaseSearchResult } from 'interfaces';
import { isEmpty } from 'lodash';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { useHistory } from 'react-router';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import { generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';

import { useLeaseExport } from '../hooks/useLeaseExport';
import { ILeaseFilter, IResearchFilter } from '../interfaces';
import * as Styled from './styles';

/**
 * Page that displays leases information.
 */
export const ResearchListView: React.FunctionComponent = () => {
  const history = useHistory();
  const { getLeases } = useApiResearch();
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
  } = useSearch<ILeaseSearchResult, IResearchFilter>(defaultFilter, getLeases);

  const { exportLeases } = useLeaseExport();

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
      setCurrentPage(0);
    },
    [setFilter, setCurrentPage],
  );

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  return <Styled.ListPage>Research List</Styled.ListPage>;
};

const StyledAddButton = styled(Button)`
  &.btn.btn-primary {
    background-color: ${props => props.theme.css.completedColor};
  }
`;

export default ResearchListView;
