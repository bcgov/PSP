import { ColumnWithProps } from 'components/Table';
import { ILeaseSearchResult } from 'interfaces';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

const columns: ColumnWithProps<ILeaseSearchResult>[] = [
  {
    Header: 'L-File Number',
    accessor: 'lFileNo',
    align: 'left',
    clickable: true,
    sortable: true,
    maxWidth: 40,
    Cell: (props: CellProps<ILeaseSearchResult>) => (
      <Link to={`/lease/${props.row.original.id}`}>{props.row.original.lFileNo}</Link>
    ),
  },
  {
    Header: 'Tenant Name',
    accessor: 'tenantName',
    align: 'left',
    clickable: true,
    minWidth: 30,
    maxWidth: 100,
  },
  {
    Header: 'Program Name',
    accessor: 'programName',
    align: 'left',
    clickable: true,
    maxWidth: 80,
  },
  {
    Header: 'PID/PIN',
    accessor: 'pidOrPin',
    align: 'left',
    clickable: true,
    maxWidth: 50,
  },
  {
    Header: 'Civic Address',
    accessor: 'address',
    align: 'left',
    clickable: true,
    minWidth: 100,
  },
];

export default columns;
