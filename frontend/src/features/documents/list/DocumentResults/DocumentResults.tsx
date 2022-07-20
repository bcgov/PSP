import { ColumnWithProps, DateCell, renderTypeCode, Table } from 'components/Table';
import { TableSort } from 'components/Table/TableSort';
import { Api_Document } from 'models/api/Document';

export interface IDocumentResultProps {
  results: Api_Document[];
  loading?: boolean;

  sort: TableSort<Api_Document>;
  setSort: (value: TableSort<Api_Document>) => void;
}

const columns: ColumnWithProps<Api_Document>[] = [
  {
    Header: 'Document type',
    accessor: 'documentType',
    align: 'left',
    sortable: false,
    minWidth: 30,
    maxWidth: 40,
  },
  {
    Header: 'File name',
    accessor: 'fileName',
    sortable: true,
    width: 50,
    maxWidth: 100,
  },
  {
    Header: 'Upload date',
    accessor: 'appCreateTimestamp',
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: DateCell,
  },
  {
    Header: 'Uploaded by',
    accessor: 'appCreateUserid',
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Status',
    accessor: 'statusTypeCode',
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: renderTypeCode,
  },
];
export function DocumentResults(props: IDocumentResultProps) {
  const { results, setSort, sort, ...rest } = props;

  return (
    <Table<Api_Document>
      name="documentsTable"
      manualSortBy={false}
      lockPageSize={true}
      manualPagination={false}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={results ?? []}
      noRowsMessage="No matching Documents found"
      {...rest}
    ></Table>
  );
}
