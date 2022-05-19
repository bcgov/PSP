import { ColumnWithProps } from 'components/Table';
import { PidCell } from 'components/Table/PidCell';

import { IMapProperty } from '../models';

const columns: ColumnWithProps<IMapProperty>[] = [
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
];

export default columns;
