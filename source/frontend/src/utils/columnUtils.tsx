import difference from 'lodash/difference';
import map from 'lodash/map';
import { FaRegTimesCircle } from 'react-icons/fa';

import { ColumnWithProps } from '../components/Table/types';

// React 18 typings do not support returning strings - workaround is to wrap it in a React fragment
export const stringToFragment = (value: string | number | undefined | null) => <>{value}</>;

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
    align: 'center',
    accessor: 'id' as any,
    maxWidth: 20,
    Cell: (props: any) => (
      <FaRegTimesCircle
        title="Click to remove"
        style={{ cursor: 'pointer' }}
        size={16}
        onClick={() => {
          setRows(difference(map(props.rows, 'original'), [props.row.original]));
        }}
      />
    ),
  });
  return cols;
};
