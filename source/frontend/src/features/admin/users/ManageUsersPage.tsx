import fileDownload from 'js-file-download';
import isEmpty from 'lodash/isEmpty';
import { useCallback, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileExcel } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Table } from '@/components/Table';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { useSearch } from '@/hooks/useSearch';
import { IUsersFilter } from '@/interfaces';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';
import { generateMultiSortCriteria } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';

import { defaultUserFilter, UsersFilterBar } from './components/UsersFilterBar';
import { getUserColumns } from './constants';
import { FormUser } from './models';
import * as Styled from './styles';

/**
 * Component to manage the user accounts.
 * Displays users in a list view.
 * Contains a filter to find users.
 * @component
 * @returns A ManagerUser component.
 */
export const ManageUsersPage = () => {
  const { getUsersPaged, exportUsers } = useApiUsers();
  const {
    results,
    filter,
    totalItems,
    totalPages,
    pageSize,
    setFilter,
    setCurrentPage,
    currentPage,
    setPageSize,
    loading,
    sort,
    setSort,
    execute,
  } = useSearch<ApiGen_Concepts_User, IUsersFilter>(
    defaultUserFilter,
    getUsersPaged,
    'No matching results can be found. Try widening your search criteria.',
  );

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setCurrentPage && setCurrentPage(pageIndex),
    [setCurrentPage],
  );

  const columns = useMemo(() => getUserColumns(execute), [execute]);
  const userList = results.map((u: ApiGen_Concepts_User): FormUser => new FormUser(u));

  /**
   * @param {'csv' | 'excel'} accept Whether the fetch is for type of CSV or EXCEL
   */
  const fetch = async (accept: 'csv' | 'excel') => {
    const query = toFilteredApiPaginateParams<IUsersFilter>(
      currentPage,
      pageSize,
      sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
      filter,
    );

    const { data } = await exportUsers(query, accept);
    return fileDownload(data, `pims-users.${accept === 'csv' ? 'csv' : 'xlsx'}`);
  };

  return (
    <StyledPage fluid className="users-management-page">
      <StyledPageHeader>User Management</StyledPageHeader>
      <Row>
        <Col md={8}>
          <UsersFilterBar values={filter} onChange={setFilter} />
        </Col>
        <Col md={4} className="align-items-center d-flex">
          <TooltipWrapper tooltipId="export-to-excel" tooltip="Export to Excel">
            <StyledIconButton onClick={() => fetch('excel')}>
              <FaFileExcel data-testid="excel-icon" size={36} />
            </StyledIconButton>
          </TooltipWrapper>
        </Col>
      </Row>
      <StyledScrollContainer>
        <Table<FormUser>
          name="usersTable"
          loading={loading}
          columns={columns}
          pageIndex={currentPage}
          data={userList}
          defaultCanSort={true}
          pageCount={totalPages}
          pageSize={pageSize}
          totalItems={totalItems}
          onRequestData={updateCurrentPage}
          externalSort={{ sort: sort, setSort: setSort }}
          onPageSizeChange={setPageSize}
          clickableTooltip="Click IDIR/BCeID link to view User Information page"
        />
      </StyledScrollContainer>
    </StyledPage>
  );
};

const StyledScrollContainer = styled(Styled.ScrollXYContainer)`
  padding: 1.5rem;
`;

const StyledPage = styled(Styled.ListView)`
  padding: 1rem 2rem;
`;

const StyledPageHeader = styled.h3`
  text-align: left;
`;
export default ManageUsersPage;
