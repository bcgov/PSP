import { TableSort } from 'components/Table/TableSort';
import { IUserRecord } from 'features/admin/users/interfaces/IUserRecord';
import { IPagedItems, IUser, IUsersFilter } from 'interfaces';

export interface IUsersState {
  pagedUsers: IPagedItems<IUser>;
  rowsPerPage: number;
  filter: IUsersFilter;
  sort: TableSort<IUserRecord>;
  pageIndex: number;
  userDetail: IUser;
}
