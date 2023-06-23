import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { Api_FormDocumentFile, Api_FormDocumentType } from '@/models/api/FormDocument';

import { createFormTableColumns } from './columns';

export interface IFormResultProps {
  results: Api_FormDocumentFile[];
  loading?: boolean;
  sort: TableSort<Api_FormDocumentType>;
  setSort: (value: TableSort<Api_FormDocumentType>) => void;
  onShowForm: (form: Api_FormDocumentFile) => void;
  onDelete: (formFileId: number) => void;
}

export function FormResults(props: IFormResultProps) {
  const { results, setSort, sort, ...rest } = props;

  const columns = createFormTableColumns(props.onShowForm, props.onDelete);

  return (
    <Table<Api_FormDocumentFile>
      name="AcqusitionFormTable"
      manualSortBy={false}
      lockPageSize={true}
      manualPagination={false}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={results ?? []}
      noRowsMessage="No matching Form(s) found"
      {...rest}
    ></Table>
  );
}
