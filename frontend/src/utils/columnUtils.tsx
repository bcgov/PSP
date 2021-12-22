import _ from 'lodash';
import { FaRegTimesCircle } from 'react-icons/fa';

import { ColumnWithProps } from '../components/Table/types';

export const getColumnsWithRemove = <T extends object>(
  setRows: (rows: T[]) => void,
  cols: ColumnWithProps<T>[],
) => {
  const idIndex = cols.findIndex(col => col.accessor === 'id');
  if (idIndex >= 0) {
    cols.splice(idIndex, 1);
  }
  cols.unshift({
    Header: '',
    align: 'left',
    accessor: 'id' as any,
    maxWidth: 20,
    Cell: (props: any) => (
      <FaRegTimesCircle
        title="Click to remove"
        style={{ cursor: 'pointer' }}
        size={16}
        onClick={() => {
          setRows(_.difference(_.map(props.rows, 'original'), [props.row.original]));
        }}
      />
    ),
  });
  return cols;
};
