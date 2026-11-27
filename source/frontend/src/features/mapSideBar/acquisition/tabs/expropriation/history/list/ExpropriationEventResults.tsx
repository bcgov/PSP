import React, { useMemo } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';

import { ExpropriationEventRow } from '../models';
import { getExpropriationEventColumns } from './columns';

export interface IExpropriationEventResultsProps {
  results: ExpropriationEventRow[];
  loading?: boolean;
  sort: TableSort<ExpropriationEventRow>;
  setSort: (value: TableSort<ExpropriationEventRow>) => void;
  onUpdate: (expropriationEventId: number) => void;
  onDelete: (expropriationEventId: number) => void;
  canEdit?: boolean;
}

export const ExpropriationEventResults: React.FunctionComponent<
  IExpropriationEventResultsProps
> = ({ results, loading, setSort, sort, onUpdate, onDelete, canEdit }) => {
  const columns = useMemo(
    () => getExpropriationEventColumns(onUpdate, onDelete, canEdit),
    [onUpdate, onDelete, canEdit],
  );

  return (
    <Table<ExpropriationEventRow>
      name="expropriationHistoryTable"
      loading={loading}
      data={results ?? []}
      totalItems={results.length}
      pageSize={results.length}
      columns={columns}
      manualPagination={false}
      manualSortBy={false}
      externalSort={{ sort, setSort }}
      hidePagination
      lockPageSize
      noRowsMessage="No matching expropriations found"
    ></Table>
  );
};
