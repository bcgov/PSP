import { CellProps } from 'react-table';

import { ColumnWithProps, DateCell } from '@/components/Table';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_LeaseTerm } from '@/models/api/generated/ApiGen_Concepts_LeaseTerm';
import { stringToFragment } from '@/utils';

export const leaseTermColumns: ColumnWithProps<ApiGen_Concepts_LeaseTerm>[] = [
  {
    Header: 'Term ID',
    accessor: 'id',
    align: 'left',
    sortable: false,
    Cell: ({ cell }: CellProps<ApiGen_Concepts_LeaseTerm, number | null>) =>
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
    Cell: ({
      cell: { value },
    }: CellProps<ApiGen_Concepts_LeaseTerm, ApiGen_Base_CodeType<string> | null>) =>
      stringToFragment(value?.description ?? ''),
  },
];
