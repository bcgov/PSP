import { ColumnWithProps } from 'components/Table';
import { ILeaseTerm } from 'interfaces/ILeaseTerm';
import { CellProps } from 'react-table';
import { prettyFormatDate } from 'utils';

export const DateCell = ({ cell: { value } }: CellProps<ILeaseTerm, string>) =>
  prettyFormatDate(value);

export const leaseTermColumns: ColumnWithProps<ILeaseTerm>[] = [
  {
    Header: 'Term ID',
    accessor: 'id',
    align: 'left',
    sortable: false,
  },
  {
    Header: 'Term Start Date',
    accessor: 'startDate',
    Cell: DateCell,
    align: 'left',
    sortable: false,
  },
  {
    Header: 'Term End Date',
    accessor: 'endDate',
    Cell: DateCell,
    align: 'left',
    sortable: false,
  },
  {
    Header: 'Date Renewed',
    accessor: 'renewalDate',
    Cell: DateCell,
    align: 'left',
    sortable: false,
  },
  {
    Header: 'Term Status',
    accessor: 'termStatus',
    align: 'left',
    sortable: false,
  },
];
