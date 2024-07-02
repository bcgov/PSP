import { CellProps } from 'react-table';

import { ColumnWithProps, DateCell } from '@/components/Table';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';
import { stringToFragment } from '@/utils';

export const leasePeriodColumns: ColumnWithProps<ApiGen_Concepts_LeasePeriod>[] = [
  {
    Header: 'Period ID',
    accessor: 'id',
    align: 'left',
    sortable: false,
    Cell: ({ cell }: CellProps<ApiGen_Concepts_LeasePeriod, number | null>) =>
      stringToFragment(cell.row.index === 0 ? 'initial period' : `renewal ${cell.row.index}`),
  },
  {
    Header: 'Period Start Date',
    accessor: 'startDate',
    Cell: DateCell,
    align: 'left',
    sortable: false,
  },
  {
    Header: 'Period End Date',
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
    Header: 'Period Status',
    accessor: 'statusTypeCode',
    align: 'left',
    sortable: false,
    Cell: ({
      cell: { value },
    }: CellProps<ApiGen_Concepts_LeasePeriod, ApiGen_Base_CodeType<string> | null>) =>
      stringToFragment(value?.description ?? ''),
  },
];
