import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { Api_Note } from '@/models/api/Note';

import { createTableColumns } from './columns';

export interface INoteResultProps {
  results: Api_Note[];
  loading?: boolean;

  onShowDetails: (note: Api_Note) => void;
  onDelete: (note: Api_Note) => void;
  sort: TableSort<Api_Note>;
  setSort: (value: TableSort<Api_Note>) => void;
}

export function NoteResults(props: INoteResultProps) {
  const { results, setSort, sort, ...rest } = props;
  const columns = createTableColumns(props.onShowDetails, props.onDelete);

  return (
    <Table<Api_Note>
      name="notesTable"
      manualSortBy={false}
      manualPagination={false}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={results ?? []}
      noRowsMessage="No matching Notes found"
      {...rest}
    ></Table>
  );
}
