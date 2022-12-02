import { StyledIconButton } from 'components/common/buttons';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { Table } from 'components/Table';
import { IPaginateParams } from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import { useApiUsers } from 'hooks/pims-api/useApiUsers';
import { useSearch } from 'hooks/useSearch';
import { IUsersFilter } from 'interfaces';
import isEmpty from 'lodash/isEmpty';
import { Api_User } from 'models/api/User';
import queryString from 'query-string';
import React, { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileExcel } from 'react-icons/fa';
import { useDispatch } from 'react-redux';
import { AnyAction } from 'redux';
import { ThunkDispatch } from 'redux-thunk';
import { RootState } from 'store/store';
import styled from 'styled-components';
import { generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';
import download from 'utils/download';

import { defaultUserFilter, UsersFilterBar } from './components/UsersFilterBar';
import { getUserColumns } from './constants';
import { FormUser } from './models';
import * as Styled from './styles';

const downloadUsers = (filter: IPaginateParams) =>
  `${ENVIRONMENT.apiUrl}/reports/users?${
    filter ? queryString.stringify({ ...filter, all: true }) : ''
  }`;

/**
 * Component to manage the user accounts.
 * Displays users in a list view.
 * Contains a filter to find users.
 * @component
 * @returns A ManagerUser component.
 */
export const ManageUsersPage = () => {
  const dispatch: ThunkDispatch<RootState, unknown, AnyAction> = useDispatch();

  const { getUsersPaged } = useApiUsers();
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
  } = useSearch<Api_User, IUsersFilter>(
    defaultUserFilter,
    getUsersPaged,
    'No matching results can be found. Try widening your search criteria.',
  );

  const columns = useMemo(() => getUserColumns(execute), [execute]);
  let userList = results.map((u: Api_User): FormUser => new FormUser(u));

  /**
   * @param {'csv' | 'excel'} accept Whether the fetch is for type of CSV or EXCEL
   */
  const fetch = (accept: 'csv' | 'excel') => {
    const query = toFilteredApiPaginateParams<IUsersFilter>(
      currentPage,
      pageSize,
      sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
      filter,
    );
    return dispatch(
      download({
        url: downloadUsers(query),
        fileName: `pims-users.${accept === 'csv' ? 'csv' : 'xlsx'}`,
        actionType: 'users',
        headers: {
          Accept: accept === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
        },
      }),
    );
  };

  return (
    <StyledPage fluid className="users-management-page">
      <StyledPageHeader>User Management</StyledPageHeader>
      <Row>
        <Col md={8}>
          <UsersFilterBar values={filter} onChange={setFilter} />
        </Col>
        <Col md={4} className="align-items-center d-flex">
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
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
          onRequestData={req => setCurrentPage(req.pageIndex)}
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
