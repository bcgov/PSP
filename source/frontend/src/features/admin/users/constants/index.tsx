import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { ReactComponent as Active } from '@/assets/images/active.svg';
import { ReactComponent as Inactive } from '@/assets/images/inactive.svg';
import { ColumnWithProps, renderTypeCode } from '@/components/Table';
import { DateTimeCell } from '@/components/Table/DateCell';
import { stringToFragment } from '@/utils';

import { RowActions } from '../components/RowActions';
import { FormUser } from '../models';

export const getUserColumns = (refresh: () => void): ColumnWithProps<FormUser>[] => [
  {
    Header: 'Active',
    accessor: 'isDisabled',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 60,
    Cell: (props: CellProps<FormUser>) =>
      props.row.original.isDisabled ? <Inactive /> : <Active />,
  },
  {
    Header: 'IDIR/BCeID',
    accessor: 'businessIdentifierValue',
    align: 'left',
    minWidth: 150,
    Cell: (props: CellProps<FormUser>) => {
      return (
        <Link to={`/admin/user/${props.row.original.id}`}>
          {props.row.original.businessIdentifierValue}
        </Link>
      );
    },
    sortable: true,
  },
  {
    Header: 'First name',
    accessor: 'firstName',
    align: 'left',
    clickable: true,
    sortable: true,
  },
  {
    Header: 'Last name',
    accessor: 'surname',
    align: 'left',
    clickable: true,
    sortable: true,
  },
  {
    Header: 'Email',
    accessor: 'email',
    align: 'left',
    clickable: true,
    sortable: true,
    minWidth: 200,
  },
  {
    Header: 'Position',
    accessor: 'position',
    align: 'left',
    clickable: true,
    sortable: true,
  },
  {
    Header: 'User Type',
    accessor: 'userTypeCode',
    align: 'left',
    clickable: true,
    Cell: renderTypeCode,
  },
  {
    Header: 'Roles',
    accessor: 'roles',
    align: 'left',
    clickable: true,
    minWidth: 200,
    Cell: (props: CellProps<FormUser>) => {
      const rolesString = props.row?.original?.roles?.map(userRole => userRole?.name);
      return stringToFragment(rolesString?.join(', '));
    },
  },
  {
    Header: 'MoTI region(s)',
    accessor: 'regions',
    align: 'left',
    clickable: true,
    minWidth: 200,
    Cell: (props: CellProps<FormUser>) => {
      const regionsString = props.row?.original?.regions?.map(
        regionUser => regionUser?.description,
      );
      return stringToFragment(regionsString?.join(', '));
    },
  },
  {
    Header: 'Last login',
    accessor: 'lastLogin',
    align: 'left',
    clickable: true,
    minWidth: 100,
    Cell: DateTimeCell,
  },
  {
    Header: ' ',
    Cell: (props: CellProps<FormUser>) => {
      return <RowActions {...props} refresh={refresh} />;
    },
    width: 75,
  },
];
