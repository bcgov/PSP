import { ColumnWithProps } from '@/components/Table';
import { PidCell } from '@/components/Table/PidCell';

import { SearchResultProperty } from '../models';

const columns: ColumnWithProps<SearchResultProperty>[] = [
  {
    Header: 'PID',
    accessor: 'pid',
    align: 'left',
    maxWidth: 20,
    minWidth: 20,
    Cell: PidCell,
  },
  {
    Header: 'PIN',
    accessor: 'pin',
    align: 'left',
    width: 20,
    maxWidth: 20,
  },
  {
    Header: 'Plan #',
    accessor: 'planNumber',
    align: 'left',
    width: 20,
    maxWidth: 20,
  },
  {
    Header: 'Address',
    accessor: 'address',
    align: 'left',
    width: 20,
    maxWidth: 20,
  },
  {
    Header: 'Legal Description',
    accessor: 'legalDescription',
    align: 'left',
    width: 30,
    maxWidth: 30,
  },
];

export default columns;
