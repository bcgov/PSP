import variables from '_variables.module.scss';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { Table } from 'components/Table';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { IPaginateParams } from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IUser, IUsersFilter } from 'interfaces';
import find from 'lodash/find';
import isEmpty from 'lodash/isEmpty';
import queryString from 'query-string';
import { useCallback, useEffect, useMemo } from 'react';
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import { FaFileExcel } from 'react-icons/fa';
import { useDispatch } from 'react-redux';
import { useAppSelector } from 'store/hooks';
import { IGenericNetworkAction } from 'store/slices/network/interfaces';
import {
  setUsersFilter,
  setUsersPageIndex,
  setUsersPageSize,
  setUsersPageSort,
  useUsers,
} from 'store/slices/users';
import styled from 'styled-components';
import { formatApiDateTime, generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';
import download from 'utils/download';

import { UsersFilterBar } from './components/UsersFilterBar';
import { columnDefinitions } from './constants';
import { IUserRecord } from './interfaces/IUserRecord';

const TableContainer = styled(Container)`
  margin-top: 10px;
  margin-bottom: 40px;
`;

const FileIcon = styled(Button)`
  background-color: #fff !important;
  color: ${variables.primaryColor} !important;
  padding: 6px 5px;
`;

const Ribbon = styled('div')`
  text-align: right;
  margin-right: 50px;
`;

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
  const organizations = useMemo(() => getByType(API.ORGANIZATION_CODE_SET_NAME), [getByType]);
  const roles = useMemo(() => getByType(API.ROLE_CODE_SET_NAME), [getByType]);
  const columns = useMemo(() => columnDefinitions, []);

  const pagedUsers = useAppSelector(state => {
    return state.users.pagedUsers;
  });

  const pageSize = useAppSelector(state => {
    return state.users.rowsPerPage;
  });

  const pageIndex = useAppSelector(state => {
    return state.users.pageIndex;
  });

  const sort = useAppSelector(state => {
    return state.users.sort;
  });

  const filter = useAppSelector(state => {
    return state.users.filter;
  });

  const users = useAppSelector(
    state => state.network[actionTypes.GET_USERS] as IGenericNetworkAction,
  );

  const onRequestData = useCallback(
    ({ pageIndex }) => {
      dispatch(setUsersPageIndex(pageIndex));
    },
    [dispatch],
  );

  const { fetchUsers } = useUsers();
  useEffect(() => {
    fetchUsers(
      toFilteredApiPaginateParams<IUsersFilter>(
        pageIndex,
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
      businessIdentifier: u.businessIdentifier,
      firstName: u.firstName,
      surname: u.surname,
      isDisabled: u.isDisabled,
      roles: u.roles ? u.roles.map(r => r.name).join(', ') : '',
      organization: u.organizations && u.organizations.length > 0 ? u.organizations[0].name : '',
      position: u.position ?? '',
      lastLogin: formatApiDateTime(u.lastLogin),
      createdOn: formatApiDateTime(u.createdOn),
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
    <div className="users-management-page">
      <UsersFilterBar
        value={filter}
        organizationLookups={organizations}
        rolesLookups={roles}
        onChange={value => {
          (value as any)?.organization
            ? dispatch(
                setUsersFilter({
                  ...value,
                  organization: (find(organizations, {
                    id: +(value as any)?.organization,
                  }) as any)?.name,
                }),
              )
            : dispatch(setUsersFilter({ ...value, organization: '' }));
          dispatch(setUsersPageIndex(0));
        }}
      />
      {
        <>
          <Ribbon>
            <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
              <FileIcon>
                <FaFileExcel data-testid="excel-icon" size={36} onClick={() => fetch('excel')} />
              </FileIcon>
            </TooltipWrapper>
          </Ribbon>
          <TableContainer fluid>
            <Table<IUserRecord>
              name="usersTable"
              columns={columns}
              pageIndex={pageIndex}
              data={userList}
              defaultCanSort={true}
              pageCount={Math.ceil(pagedUsers.total / pageSize)}
              pageSize={pageSize}
              onRequestData={onRequestData}
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
          </TableContainer>
        </>
      }
    </div>
  );
};

export default ManageUsers;
