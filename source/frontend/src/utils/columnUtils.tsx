import difference from 'lodash/difference';
import map from 'lodash/map';

import { RemoveIconButton } from '@/components/common/buttons/RemoveButton';

import { ColumnWithProps } from '../components/Table/types';

// React 18 typings do not support returning strings - workaround is to wrap it in a React fragment
export const stringToFragment = (value: string | number | undefined | null) => <>{value}</>;

export const getColumnsWithRemove = <T extends object>(
  setRows: (rows: T[]) => void,
  cols: ColumnWithProps<T>[],
  headerName?: string,
) => {
  const idIndex = cols.findIndex(col => col.accessor === 'id');
  if (idIndex >= 0) {
    cols.splice(idIndex, 1);
  }
  cols.push({
    Header: headerName ?? '',
    align: 'center',
    accessor: 'id' as any,
    maxWidth: 35,
    Cell: (props: any) => (
      <RemoveIconButton
        title="Click to remove"
        onRemove={() => {
          setRows(difference(map(props.rows, 'original'), [props.row.original]));
        }}
      />
    ),
  });
  return cols;
};
