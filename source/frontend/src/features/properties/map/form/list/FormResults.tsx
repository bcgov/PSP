import { Table } from 'components/Table';
import { TableSort } from 'components/Table/TableSort';
import { Api_FileForm, Api_Form } from 'models/api/Form';

import { createFormTableColumns } from './columns';

export interface IFormResultProps {
  results: Api_FileForm[];
  loading?: boolean;
  sort: TableSort<Api_Form>;
  setSort: (value: TableSort<Api_Form>) => void;
  onShowForm: (form: Api_FileForm) => void;
  onDelete: (formFileId: number) => void;
}

export function FormResults(props: IFormResultProps) {
  const { results, setSort, sort, ...rest } = props;

  const columns = createFormTableColumns(props.onShowForm, props.onDelete);

  return (
    <Table<Api_FileForm>
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
