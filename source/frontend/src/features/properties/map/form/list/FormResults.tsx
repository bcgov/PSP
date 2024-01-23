import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { ApiGen_Concepts_FormDocumentFile } from '@/models/api/generated/ApiGen_Concepts_FormDocumentFile';
import { ApiGen_Concepts_FormDocumentType } from '@/models/api/generated/ApiGen_Concepts_FormDocumentType';

import { createFormTableColumns } from './columns';

export interface IFormResultProps {
  results: ApiGen_Concepts_FormDocumentFile[];
  loading?: boolean;
  sort: TableSort<ApiGen_Concepts_FormDocumentType>;
  setSort: (value: TableSort<ApiGen_Concepts_FormDocumentType>) => void;
  onShowForm: (form: ApiGen_Concepts_FormDocumentFile) => void;
  onDelete: (formFileId: number) => void;
}

export function FormResults(props: IFormResultProps) {
  const { results, setSort, sort, ...rest } = props;

  const columns = createFormTableColumns(props.onShowForm, props.onDelete);

  return (
    <Table<ApiGen_Concepts_FormDocumentFile>
      name="AcquisitionFormTable"
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
