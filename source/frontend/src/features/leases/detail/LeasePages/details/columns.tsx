import { CellProps } from 'react-table';

import { ColumnWithProps, DateCell } from '@/components/Table';
import { ILeaseTerm } from '@/interfaces/ILeaseTerm';
import ITypeCode from '@/interfaces/ITypeCode';
import { stringToFragment } from '@/utils';

export const leaseTermColumns: ColumnWithProps<Api_LeaseTerm>[] = [
  {
    Header: 'Term ID',
    accessor: 'id',
    align: 'left',
    sortable: false,
    Cell: ({ cell }: CellProps<Api_LeaseTerm, number | null>) =>
      stringToFragment(cell.row.index === 0 ? 'initial term' : `renewal ${cell.row.index}`),
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
    accessor: 'expiryDate',
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
    accessor: 'statusTypeCode',
    align: 'left',
    sortable: false,
    Cell: ({ cell: { value } }: CellProps<Api_LeaseTerm, Api_TypeCode<string> | null>) =>
      stringToFragment(value?.description ?? ''),
  },
];
