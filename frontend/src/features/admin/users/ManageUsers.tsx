import { IconButton } from 'components/common/buttons';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { Table } from 'components/Table';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { IPaginateParams } from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IUser, IUsersFilter } from 'interfaces';
import isEmpty from 'lodash/isEmpty';
import queryString from 'query-string';
import { useCallback, useEffect, useMemo, useState } from 'react';
import Container from 'react-bootstrap/Container';
import { FaFileExcel } from 'react-icons/fa';
import { useDispatch } from 'react-redux';
import { useAppSelector } from 'store/hooks';
import {
  setUsersFilter,
  setUsersPageIndex,
  setUsersPageSize,
  setUsersPageSort,
  useUsers,
} from 'store/slices/users';
import { formatApiDateTime, generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';
import download from 'utils/download';

import { UsersFilterBar } from './components/UsersFilterBar';
import { columnDefinitions } from './constants';
import { IUserRecord } from './interfaces/IUserRecord';
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
export const ManageUsers = () => {
  const dispatch = useDispatch();
  const { getByType } = useLookupCodeHelpers();
  const roles = useMemo(() => getByType(API.ROLE_TYPES), [getByType]);
  const columns = useMemo(() => columnDefinitions, []);
  const [pageIndex, setPageIndex] = useState(0);

  const pagedUsers = useAppSelector(state => {
    return state.users.pagedUsers;
  });

  const pageSize = useAppSelector(state => {
    return state.users.rowsPerPage;
  });

  const sort = useAppSelector(state => {
    return state.users.sort;
  });

  const filter = useAppSelector(state => {
    return state.users.filter;
  });

  const users = useAppSelector(state => state.network[actionTypes.GET_USERS]);

  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  const { fetchUsers } = useUsers();
  useEffect(() => {
    fetchUsers(
      toFilteredApiPaginateParams<IUsersFilter>(
        pageIndex ?? 0,
        pageSize,
        sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
        filter,
      ),
    );
  }, [sort, pageIndex, pageSize, filter, fetchUsers]);

  let userList = pagedUsers.items.map(
    (u: IUser): IUserRecord => ({
      id: u.id,
      keycloakUserId: u.keycloakUserId,
      email: u.email,
      businessIdentifierValue: u.businessIdentifier,
      firstName: u.firstName,
      surname: u.surname,
      isDisabled: u.isDisabled,
      roles: u.roles ? u.roles.map(r => r.name).join(', ') : '',
      position: u.position ?? '',
      lastLogin: formatApiDateTime(u.lastLogin),
      appCreateTimestamp: formatApiDateTime(u.appCreateTimestamp),
      rowVersion: u.rowVersion,
    }),
  );

  /**
   * @param {'csv' | 'excel'} accept Whether the fetch is for type of CSV or EXCEL
   */
  const fetch = (accept: 'csv' | 'excel') => {
    const query = toFilteredApiPaginateParams<IUsersFilter>(
      pageIndex,
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
    <Styled.ListView fluid className="users-management-page">
      <Container fluid className="filter-container p-0 border-bottom">
        <Styled.WithShadow fluid>
          <UsersFilterBar
            value={filter}
            rolesLookups={roles}
            onChange={value => {
              dispatch(setUsersFilter({ ...value }));
              dispatch(setUsersPageIndex(0));
            }}
          />
        </Styled.WithShadow>
      </Container>
      <Styled.ScrollContainer>
        <Styled.Ribbon>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
            <IconButton onClick={() => fetch('excel')}>
              <FaFileExcel data-testid="excel-icon" size={36} />
            </IconButton>
          </TooltipWrapper>
        </Styled.Ribbon>
        <Styled.TableContainer fluid>
          <Table<IUserRecord>
            name="usersTable"
            columns={columns}
            pageIndex={pageIndex}
            data={userList}
            defaultCanSort={true}
            pageCount={Math.ceil(pagedUsers.total / pageSize)}
            pageSize={pageSize}
            totalItems={pagedUsers.total}
            onRequestData={updateCurrentPage}
            onSortChange={(column, direction) => {
              if (!!direction) {
                dispatch(setUsersPageSort({ [column]: direction }));
              } else {
                dispatch(setUsersPageSort({}));
              }
            }}
            sort={sort}
            onPageSizeChange={size => dispatch(setUsersPageSize(size))}
            loading={!(users && !users.isFetching)}
            clickableTooltip="Click IDIR/BCeID link to view User Information page"
          />
        </Styled.TableContainer>
      </Styled.ScrollContainer>
    </Styled.ListView>
  );
};

export default ManageUsers;
