import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';

import { IUpdateNotesStrategy } from '../../models/IUpdateNotesStrategy';
import { createTableColumns } from './columns';

export interface INoteResultProps {
  results: ApiGen_Concepts_Note[];
  loading?: boolean;
  statusSolver?: IUpdateNotesStrategy;

  onShowDetails: (note: ApiGen_Concepts_Note) => void;
  onDelete: (note: ApiGen_Concepts_Note) => void;
  sort: TableSort<ApiGen_Concepts_Note>;
  setSort: (value: TableSort<ApiGen_Concepts_Note>) => void;
}

export function NoteResults(props: INoteResultProps) {
  const { results, statusSolver, setSort, sort, ...rest } = props;
  const columns = createTableColumns(props.onShowDetails, props.onDelete, statusSolver);

  return (
    <Table<ApiGen_Concepts_Note>
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
