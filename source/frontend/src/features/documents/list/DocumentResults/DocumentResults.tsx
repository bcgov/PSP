import { Table } from 'components/Table';
import { TableSort } from 'components/Table/TableSort';
import { Api_Document } from 'models/api/Document';
import { useMemo } from 'react';

import { getDocumentColumns } from './DocumentResultsColumns';

export interface IDocumentResultProps {
  results: Api_Document[];
  loading?: boolean;
  sort: TableSort<Api_Document>;
  setSort: (value: TableSort<Api_Document>) => void;
  onViewDetails: (values: Api_Document) => void;
  onDelete: (values: Api_Document) => void;
}

export const DocumentResults: React.FunctionComponent<IDocumentResultProps> = ({
  results,
  setSort,
  sort,
  onViewDetails,
  onDelete,
  ...rest
}) => {
  const columns = useMemo(() => getDocumentColumns({ onViewDetails, onDelete }), [
    onViewDetails,
    onDelete,
  ]);

  return (
    <Table<Api_Document>
      name="documentsTable"
      manualSortBy={false}
      manualPagination={false}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={results ?? []}
      noRowsMessage="No matching Documents found"
      {...rest}
    ></Table>
  );
};
