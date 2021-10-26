import { ColumnWithProps } from 'components/Table';
import { ILeaseSearchResult } from 'interfaces';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

const columns: ColumnWithProps<ILeaseSearchResult>[] = [
  {
    Header: 'L-File Number',
    accessor: 'lFileNo',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 40,
    Cell: (props: CellProps<ILeaseSearchResult>) => (
      <Link to={`/lease/${props.row.original.id}`}>{props.row.original.lFileNo}</Link>
    ),
  },
  {
    Header: 'Tenant Name',
    accessor: 'tenantName',
    align: 'left',
    clickable: true,
    width: 60,
    maxWidth: 100,
  },
  {
    Header: 'Program Name',
    accessor: 'programName',
    align: 'left',
    clickable: true,
    width: 40,
    maxWidth: 80,
  },
  {
    Header: 'PID/PIN',
    accessor: 'pidOrPin',
    align: 'right',
    clickable: true,
    maxWidth: 40,
  },
  {
    Header: 'Civic Address',
    accessor: 'address',
    align: 'left',
    clickable: true,
    minWidth: 100,
    width: 150,
  },
];

export default columns;
