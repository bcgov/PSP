import { useMemo } from 'react';

import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { DocumentRow } from '@/features/documents/ComposedDocument';
import { Api_Document, Api_DocumentRelationship } from '@/models/api/Document';

import { getDocumentColumns } from './DocumentResultsColumns';

export interface IDocumentResultProps {
  results: DocumentRow[];
  loading?: boolean;
  sort: TableSort<Api_Document>;
  setSort: (value: TableSort<Api_Document>) => void;
  onViewDetails: (values: Api_DocumentRelationship) => void;
  onDelete: (values: Api_DocumentRelationship) => void;
}

export const DocumentResults: React.FunctionComponent<
  React.PropsWithChildren<IDocumentResultProps>
> = ({ results, setSort, sort, onViewDetails, onDelete, ...rest }) => {
  const columns = useMemo(
    () => getDocumentColumns({ onViewDetails, onDelete }),
    [onViewDetails, onDelete],
  );

  return (
    <Table<DocumentRow>
      name="documentsTable"
      manualSortBy={false}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={results ?? []}
      noRowsMessage="No matching Documents found"
      pageSize={1}
      manualPagination={false}
      {...rest}
    ></Table>
  );
};
