import { Table } from '@/components/Table';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

import { createSubFilesTableColumns } from './colums';

export interface ISubFilesResultsTableProps {
  results: ApiGen_Concepts_AcquisitionFile[];
  currentAcquisitionFileId: number;
  routeURL: string;
}

export function SubFilesResultsTable(props: ISubFilesResultsTableProps) {
  const { results, ...rest } = props;
  const columns = createSubFilesTableColumns(props.currentAcquisitionFileId, props.routeURL);

  return (
    <Table<ApiGen_Concepts_AcquisitionFile>
      name="AcquisitionCompensationTable"
      manualSortBy={false}
      lockPageSize={true}
      showSelectedRowCount={false}
      hidePagination={true}
      columns={columns}
      data={results ?? []}
      {...rest}
    ></Table>
  );
}
