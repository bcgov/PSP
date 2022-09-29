import { Table } from 'components/Table';
import { useApiAccessRequests } from 'hooks/pims-api';
import { useSearch } from 'hooks/useSearch';
import { Api_AccessRequest } from 'models/api/AccessRequest';
import { useEffect } from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { IAccessRequestsFilterData } from '../access-request/IAccessRequestsFilterData';
import { FormAccessRequest } from '../access-request/models';
import { ScrollXYContainer } from '../users/styles';
import { AccessRequestFilter } from './components/Filter';
import { getAccessRequestColumns } from './constants/constants';

const ManageAccessRequestsPage = () => {
  const { getAccessRequestsPaged } = useApiAccessRequests();
  const {
    results,
    filter,
    error,
    totalItems,
    totalPages,
    pageSize,
    setFilter,
    setCurrentPage,
    setPageSize,
    loading,
    execute,
  } = useSearch<Api_AccessRequest, IAccessRequestsFilterData>(
    defaultFilter,
    getAccessRequestsPaged,
    'No matching results can be found. Try widening your search criteria.',
  );
  const columnDefinitions = getAccessRequestColumns(execute);

  useEffect(() => {
    if (error) {
      toast.error(error?.message);
    }
  }, [error]);

  return (
    <StyledContainer>
      <StyledPageHeader>PIMS User Access Requests</StyledPageHeader>
      <AccessRequestFilter
        initialValues={filter}
        applyFilter={accessRequestfilter => setFilter(accessRequestfilter)}
      />
      <Table<FormAccessRequest>
        manualSortBy
        name="accessRequestsTable"
        columns={columnDefinitions}
        data={results.map(result => new FormAccessRequest(result))}
        defaultCanSort={true}
        pageCount={totalPages}
        onPageSizeChange={setPageSize}
        pageSize={pageSize}
        loading={loading}
        onRequestData={req => setCurrentPage(req.pageIndex)}
        totalItems={totalItems}
      />
    </StyledContainer>
  );
};

export const defaultFilter: IAccessRequestsFilterData = {
  organization: '',
  role: '',
  searchText: '',
};

const StyledContainer = styled(ScrollXYContainer)`
  width: 100%;
  height: 100%;
`;

const StyledPageHeader = styled.h3`
  text-align: left;
`;

export default ManageAccessRequestsPage;
