import React, { useMemo } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';

import { ExpropriationEventRow } from '../models';
import { getExpropriationEventColumns } from './columns';

export interface IDocumentResultProps {
  results: ExpropriationEventRow[];
  loading?: boolean;
  sort: TableSort<ExpropriationEventRow>;
  setSort: (value: TableSort<ExpropriationEventRow>) => void;
  onUpdate: (expropriationEventId: number) => void;
  onDelete: (expropriationEventId: number) => void;
}

export const ExpropriationEventResults: React.FunctionComponent<IDocumentResultProps> = ({
  results,
  loading,
  setSort,
  sort,
  onUpdate,
  onDelete,
}) => {
  const columns = useMemo(
    () => getExpropriationEventColumns(onUpdate, onDelete),
    [onUpdate, onDelete],
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
